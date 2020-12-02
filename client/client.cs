using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace client
{
    class client
    {
        static void Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8080);
            try
            {
                client.Connect("127.0.0.1", 8080);
                Console.WriteLine("connected");
                while (true)
                {
                    Console.WriteLine("message:");
                    string message = Console.ReadLine();
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    client.Send(buffer);
                    message = null;

                    byte[] buffer2 = new byte[1024];
                    client.Receive(buffer2);
                    Console.WriteLine(Encoding.UTF8.GetString(buffer2));
                }
                //string message = "hi server";
                //byte[] buffer = Encoding.UTF8.GetBytes(message);
                //client.Send(buffer);

                //byte[] buffer2 = new byte[1024];
                //client.Receive(buffer2);
                //Console.WriteLine(Encoding.UTF8.GetString(buffer2));
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
