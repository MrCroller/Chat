﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class server
    {
        static Socket SocServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);

        static void Main(string[] args)
        {
            Console.Title = "Server";
            try
            {
                SocServer.Bind(ipPoint);
                SocServer.Listen(5);
                Console.WriteLine("server started...");

                Socket client = SocServer.Accept();
                Console.WriteLine("Новое подключение.");

                while (true)
                {
                    byte[] buffer = new byte[1024];
                    client.Receive(buffer);
                    Console.WriteLine(Encoding.UTF8.GetString(buffer));

                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
