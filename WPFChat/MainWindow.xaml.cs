using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

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

            Chat_ListBox.Items.Add("Вы вошли в чат");
            GetList();

            //#region Кожевенник
            //billy = new Client("Billy Herrington");
            //users.Add(billy);
            //#endregion

            //users.Add(ConnectWindow.Me);
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
            finally
            {
                //Disconnect();
            }
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
                Chat_ListBox.Items.Add(MsgBox.Text);
                SendMsg(MsgBox.Text, ConnectWindow.Me);
            }
        }

        /// <summary>
        /// Метод принятия сообщений
        /// </summary>
        public void GetMsg()
        {
            try
            {
                while (true)
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
                    System.Windows.Application.Current.Dispatcher.Invoke(() => AddClient(message));
                    System.Windows.Application.Current.Dispatcher.Invoke(() => Chat_ListBox.Items.Add(message));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() => Chat_ListBox.Items.Add($"Подключение прервано!\n{ex.Message}"));
                Disconnect();
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

        /// <summary>
        /// Метод получения списка клиентов при первом входе
        /// </summary>
        private void GetList()
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
            string list = builder.ToString();

            Regex reg = new Regex(@"(\w|\s){1,16}");//регулярка для имен пользавателей\
            MatchCollection matchedClients = reg.Matches(list);
            for (int i = 0; i < matchedClients.Count; i++)
            {
                var cl = new Client(matchedClients[i].Value);
                users.Add(cl);
            }
        }

        /// <summary>
        /// Метод вычленения имени клиента и добавление его в список
        /// </summary>
        /// <param name="chek"></param>
        private void AddClient(string chek)
        {
            Regex reg = new Regex(@"^SERVER: (?<clientNAME>(\w|\s){1,16}).{12}");
            var newclient = reg.Match(chek);
            var cl = new Client(newclient.Groups["clientNAME"].Value);
            users.Add(cl);
        }

        protected internal void Disconnect()
        {
            if (ConnectWindow.stream != null)
                ConnectWindow.stream.Close(); //отключение потока
            if (ConnectWindow.TCPclient != null)
            {
                ConnectWindow.TCPclient.Close();//отключение клиента
                ConnectWindow.TCPclient.Dispose();
            }
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

        private void Chat_Closed(object sender, EventArgs e)
        {
            Disconnect();
        }
    }
}
