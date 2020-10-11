using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Client
{
    public class TcpClientFactory
    {
        private static TcpClient _instance = null;
        private static readonly object Padlock = new object();

        protected TcpClientFactory() { }

        public static TcpClient Instance(string s)
        {
            lock (Padlock)
            {
                return _instance ??= new TcpClient(Constant.Ip, Constant.Port, s);
            }
        }
    }
}