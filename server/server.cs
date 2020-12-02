using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    class server
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            Socket SocServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            try
            {
                SocServer.Bind(ipPoint);
                Console.WriteLine("server started...");
                SocServer.Listen(5);
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
