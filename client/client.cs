using System;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class client
    {
        static Socket SocClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static bool flag = false;

        static void Main(string[] args)
        {
            Console.Title = "Client";
            try
            {
                SocClient.Connect("127.0.0.1", 8080);
                Console.WriteLine("connected");

                while (true)
                {
                    SendMsg();
                    //ResiveMsg();
                    System.Threading.Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        static void SendMsg()
        {
            Console.WriteLine("\nmessage:");
            string message = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            SocClient.Send(buffer);
        }

        static void ResiveMsg()
        {
            byte[] buffer = new byte[1024];
            SocClient.Receive(buffer);
            Console.WriteLine(Encoding.UTF8.GetString(buffer));
            SendMsg();
        }
    }
}
