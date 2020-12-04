using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFChat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Client> users;
        private bool bot_flag = false;
        Client billy;

        public MainWindow()
        {
            InitializeComponent();
            users = new ObservableCollection<Client> { };
            Client_ListBox.ItemsSource = users;
            Client_ListBox.DisplayMemberPath = "Name";

            #region Кожевенник
            billy = new Client();
            billy.Name = "Billy Herrington";
            users.Add(billy);
            #endregion

            users.Add(ConnectWindow.Me);
            //Gachi();

            try
            {
                Thread receiveThread = new Thread(new ThreadStart(GetMsg));
                receiveThread.Start(); //старт потока

            }
            catch (Exception ex)
            {
                Chat_ListBox.Items.Add(ex.Message);
            }
            //finally
            //{
            //    Disconnect();
            //}
        }

        /// <summary>
        /// Отправка по нажатию кнопки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            if (MsgBox.Text != string.Empty)
            {
                SendMsg(MsgBox.Text, ConnectWindow.Me);
            }
        }

        /// <summary>
        /// Метод принятия сообщений
        /// </summary>
        private void GetMsg()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = ConnectWindow.stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (ConnectWindow.stream.DataAvailable);

                    string message = builder.ToString();
                    Chat_ListBox.Items.Add(message);
                }
                catch (Exception ex)
                {
                    //Chat_ListBox.Items.Add("Подключение прервано!");
                    //Chat_ListBox.Items.Add(ex.Message);
                }
            }
        }

        /// <summary>
        /// Метод отправки сообщения
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cl"></param>
        private void SendMsg(string msg, Client cl)
        {
            //Chat_ListBox.Items.Add(msg_en);
            MsgBox.Text = string.Empty;

            byte[] data = Encoding.Unicode.GetBytes(msg);
            ConnectWindow.stream.Write(data, 0, data.Length);
        }

        /// <summary>
        /// Метод по нажатию кнопки отключиться
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Disconnect_Click(object sender, RoutedEventArgs e)
        {
            Disconnect();
            var Cnct = new ConnectWindow();
            Cnct.Show();
            Chat.Close();
        }

        /// <summary>
        /// Отправка по нажатию клавиши Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MsgBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                Enter_Click(sender, e);
            }
        }


        /// <summary>
        /// Метод гачи
        /// </summary>
        private async void Gachi()
        {
            if (!bot_flag)
            {
                await Task.Delay(5000);
                Chat_ListBox.Items.Add($"{billy.Name}: Друг. Кажется ты ошибся дверью");
                await Task.Delay(4000);
                Chat_ListBox.Items.Add($"{billy.Name}: Клуб кожевенного ремесла в конце кода");
                await Task.Delay(4000);
                var ricardo = new Client();
                ricardo.Name = "Ricardo Milos";
                Chat_ListBox.Items.Add($"{ricardo.Name} подключился");
                users.Add(ricardo);
                bot_flag = true;
            }
        }

        static void Disconnect()
        {
            if (ConnectWindow.stream != null)
                ConnectWindow.stream.Close();//отключение потока
            if (ConnectWindow.TCPclient != null)
                ConnectWindow.TCPclient.Close();//отключение клиента
        }
    }
}
