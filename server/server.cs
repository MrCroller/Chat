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
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            try
            {
                server.Bind(ipPoint);
                Console.WriteLine("server started...");
                while (true)
                {
                    server.Listen(5);
                    string data = null;
                    Socket client = server.Accept();
                    byte[] buffer = new byte[1024];
                    client.Receive(buffer);
                    data = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine(data);
                    Console.WriteLine("message:");
                    string response = Console.ReadLine();
                    byte[] buffer2 = Encoding.UTF8.GetBytes(response);
                    client.Send(buffer2);
                }
                //Socket client = server.Accept();
                //byte[] buffer = new byte[1024];
                //client.Receive(buffer);
                //Console.WriteLine(Encoding.UTF8.GetString(buffer));
                //Console.WriteLine("напиши ответ");
                //string message = Console.ReadLine();
                //byte[] buffer2 = Encoding.UTF8.GetBytes(message);
                //client.Send(buffer2);
                //Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
