using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace server
{
    class server
    {
        static ServerObject _server; // сервер
        static Thread listenThread; // поток для прослушивания

        static void Main(string[] args)
        {
            Console.Title = "Server";

            try
            {
                _server = new ServerObject();
                listenThread = new Thread(new ThreadStart(_server.Listen)); //запускается новый поток, который обращается к методу Listen() объекта ServerObject
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                _server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
