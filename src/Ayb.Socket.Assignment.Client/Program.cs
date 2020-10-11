using System;
using System.ComponentModel;
using System.Threading;
using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Client
{
    class Program
    {
        private static readonly BackgroundWorker MessageReceiver = new BackgroundWorker();

        static void Main(string[] args)
        {
            Console.Write(Constant.Messages.EnterYourUsername);
            var username = Console.ReadLine();

            var c = TcpClientFactory.Instance(username);
            c.ConnectToServer();

            Console.WriteLine();
            Console.WriteLine(Constant.Messages.Info);
            Console.WriteLine();

            MessageReceiver.DoWork += MessageReceiver_ReceiveResponses;
            MessageReceiver.RunWorkerAsync(argument: username);
            MessageReceiver.RunWorkerCompleted += MessageReceiver_RunWorkerCompleted;

            string str;
            do
            {
                str = Console.ReadLine();
                c.Send(str);
            } while (str != Constant.Eof);

            c.Exit();
            Console.ReadKey();
        }

        private static void MessageReceiver_ReceiveResponses(object sender, DoWorkEventArgs e)
        {
            var username = (string)e.Argument;
            e.Result = username;
            if (!string.IsNullOrEmpty(username))
            {
                Console.WriteLine(TcpClientFactory.Instance(username).ReceiveResponse().Data);
            }
        }

        private static void MessageReceiver_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!MessageReceiver.IsBusy)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                MessageReceiver.RunWorkerAsync(e.Result);
            }
        }
    }
}
