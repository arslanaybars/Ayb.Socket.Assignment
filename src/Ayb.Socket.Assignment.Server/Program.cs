using System;
using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpServer s = new TcpServer(Constant.Port);
            s.SetupServer();

            Console.WriteLine("Click 'M' for sending a message to all connected users");
            Console.WriteLine("Click 'E' for EXIT!");

            string str;
            do
            {
                str = Console.ReadLine();
                if (str == "M")
                {
                    Console.Write(Constant.Messages.Text);
                    var message = Console.ReadLine();
                    s.Send(message, true);
                }

            } while (str != "E");

            Console.ReadLine();
            s.CloseAllSockets();
        }
    }
}
