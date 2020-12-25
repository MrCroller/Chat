using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server
{
    public class ServerObject
    {
        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все клиенты храняться здесь(подключения)
        int port = 8080;

        /// <summary>
        /// Добавление клиента (подключения)
        /// </summary>
        /// <param name="clientObject"></param>
        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        /// <summary>
        /// Удаление клиента (подключения)
        /// </summary>
        /// <param name="id"></param>
        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }

        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                Console.WriteLine($"{IPAddress.Any.ToString()}:{8080}\nСервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientObject clientObject = new ClientObject(tcpClient, this);
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        /// <summary>
        /// Трансляции сообщений всем клиентам
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id"></param>
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }

        /// <summary>
        /// Отключение всех клиентов
        /// </summary>
        protected internal void Disconnect()
        {
            tcpListener.Stop(); //остановка сервера

            for (int i = 0; i < clients.Count; i++)
            {
                clients[i].Close(); //отключение клиента
            }
            Environment.Exit(0); //завершение процесса
        }

        /// <summary>
        /// Метод преобразования имен клиентов в строку
        /// </summary>
        /// <returns></returns>
        internal string ClListToString()
        {
            var buildSTR = new StringBuilder();
            for (int i = 0; i < clients.Count; i++) buildSTR.Append($"{clients[i].userName},");
            return buildSTR.ToString();
        }
    }
}
