using System;
using Ayb.Socket.Assignment.Client;
using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Tests
{
    public class FakeTcpClientFactory
    {
        private static TcpClient _instance = null;

        private static readonly object Padlock = new object();

        protected FakeTcpClientFactory() { }

        public static TcpClient Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TcpClient(Constant.Ip, Constant.Port, Guid.NewGuid().ToString());
                        _instance.ConnectToServer();
                        return _instance;
                    }
                    return _instance;
                }
            }
        }
    }
}
