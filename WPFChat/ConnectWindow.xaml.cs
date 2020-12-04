using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace WPFChat
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        internal static TcpClient TCPclient;
        internal static NetworkStream stream;
        public static Client Me = new Client();
        string host;
        string Cnsl;
        int port;
        bool flag_ad; // Флажок успешно ли приобразование
        int ch = 0; // Счетчик попыток

        public ConnectWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Нажатие кнопки ввода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Me.Name = Name_TextBox.Text;

            ChekAddress(Ip_Port_TextBox.Text);

            if (!flag_ad && (ch == 1 || (ch > 6 && ch < 11) || ch > 11)) MessageBox.Show("Введен неккорректный адрес");

            #region Бесполезный код
            if (!flag_ad && ch == 2) MessageBox.Show("Может попробуешь еще раз?");
            if (!flag_ad && ch == 3) MessageBox.Show("У тебя почти получилось");
            if (!flag_ad && ch == 4) MessageBox.Show("Ты главное не сдавайся");
            if (!flag_ad && ch == 5)
            {
                MessageBox.Show("Поздравляю, вы подключились!");
                System.Threading.Thread.Sleep(2000);
                MessageBox.Show("Шучу");
            }
            if (!flag_ad && ch == 6) MessageBox.Show("Все, я отстаю");
            if (!flag_ad && ch == 11) MessageBox.Show("НУ пиздец какой то");

            #endregion

            if (flag_ad) // открытие окна чата
            {
                ConnectAddress();
            }
        }

        /// <summary>
        /// Метод проверки и вычленения адреса
        /// </summary>
        /// <param name="adress">Адрес</param>
        /// <returns></returns>
        private void ChekAddress(string s)
        {
            Regex Regex = new Regex(@"^(?<ip>(\d{1,3}.){3}\d{1,3})(:(?<port>\d{4,5}))?$"); // регулярка для адреса

            if (Regex.IsMatch(s)) // Проверка адреса
            {
                flag_ad = true;
                var address = Regex.Match(s);
                host = address.Groups["ip"].Value; // Значения ip
                port = Int32.Parse(address.Groups["port"].Value); // Значение порта
            }
            else
            {
                ch++;
                flag_ad = false;
            }
        }

        /// <summary>
        /// Подключение
        /// </summary>
        private void ConnectAddress()
        {
            TCPclient = new TcpClient();
            try
            {
                TCPclient.Connect(host, port); //подключение клиента
                stream = TCPclient.GetStream(); // получаем поток

                byte[] data = Encoding.Unicode.GetBytes(Me.Name);
                stream.Write(data, 0, data.Length);

                var Chat = new MainWindow();
                Chat.Show();

                ConnectWnd.Close(); // закрытие окна входа
            }
            catch (Exception ex)
            {
                ButtonConnect.IsEnabled = true;
                Cnsl += $"{DateTime.UtcNow.ToString("HH:mm:ss")}: {ex.Message}\n";
                CslBox.Text = Cnsl;
            }
        }
    }
}
