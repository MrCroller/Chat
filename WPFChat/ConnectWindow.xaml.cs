using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace WPFChat
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        string ip;
        string port;
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
            Client Pup = new Client();
            Pup.Name = Name_TextBox.Text;

            ConnectAdress(Ip_Port_TextBox.Text);

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
                MainWindow Chat = new MainWindow();
                Chat.Show();

                ConnectWindow1.Close(); // закрытие окна входа
            }
        }

        /// <summary>
        /// Метод проверки и вычленения адреса
        /// </summary>
        /// <param name="adress">Адрес</param>
        /// <returns></returns>
        private void ConnectAdress(string s)
        {
            Regex Regex = new Regex(@"^(?<ip>(\d{1,3}.){3}\d{1,3})(:(?<port>\d{4,5}))?$"); // регулярка для адреса

            if (Regex.IsMatch(s)) // Проверка адреса
            {
                flag_ad = true;
                var address = Regex.Match(s);
                ip = address.Groups["ip"].Value; // Значения ip
                port = address.Groups["port"].Value; // Значение порта
            }
            else
            {
                ch++;
                flag_ad = false;
            }
        }
    }
}
