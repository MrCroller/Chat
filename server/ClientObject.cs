using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace server
{
    public class ClientObject
    {
        protected internal string Id { get; private set; } // Уникальный идентификатор
        protected internal NetworkStream Stream { get; private set; } // свойство Stream, хранящее поток для взаимодействия с клиентом
        internal string userName { get; private set; }
        TcpClient client;
        bool human = true;
        ServerObject server; // объект сервера

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this); //При создании нового объекта в конструкторе будет происходить его добавление в коллекцию подключений класса ServerObject
        }

        /// <summary>
        /// Свойство для бота
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serverObject"></param>
        public ClientObject(string userName, TcpClient tcpClient, ServerObject serverObject)
        {
            human = false;
            //Id = Guid.NewGuid().ToString();
            Id = "bot";
            client = tcpClient;
            server = serverObject;
            userName = this.userName;
            serverObject.AddConnection(this);
        }

        /// <summary>
        /// Протокол для обмена сообщениями с клиентом
        /// </summary>
        public void Process()
        {
            if (human)
            {
                try
                {
                    Stream = client.GetStream();
                    // получаем имя пользователя
                    string message = GetMessage();
                    userName = message;

                    byte[] data = Encoding.Unicode.GetBytes(server.ClListToString()); //Отправляем список клиентов на сервере
                    Stream.Write(data, 0, data.Length);

                    message = $"SERVER: {userName} вошел в чат";
                    // посылаем сообщение о входе в чат всем подключенным пользователям
                    server.BroadcastMessage(message, this.Id);
                    Console.WriteLine(message);
                    // в бесконечном цикле получаем сообщения от клиента
                    while (true)
                    {
                        try
                        {
                            message = GetMessage();
                            message = String.Format($"{userName}: {message}");
                            Console.WriteLine(message);
                            server.BroadcastMessage(message, this.Id);
                        }
                        catch
                        {
                            message = String.Format($"{userName}: покинул чат");
                            //Console.WriteLine(message);
                            server.BroadcastMessage(message, this.Id);
                            server.RemoveConnection(this.Id);
                            Close();
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    // в случае выхода из цикла закрываем ресурсы
                    server.RemoveConnection(this.Id);
                    Close();
                }
            }
        }

        /// <summary>
        /// Чтение входящего сообщения и преобразование его в строку
        /// </summary>
        /// <returns></returns>
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        /// <summary>
        /// Закрытие подключения
        /// </summary>
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
