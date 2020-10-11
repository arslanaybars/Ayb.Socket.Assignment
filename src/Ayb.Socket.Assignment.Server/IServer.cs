using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Server
{
    public interface IServer
    {
        public void SetupServer();

        public ServiceResponse CloseAllSockets();

        public ServiceResponse<string> Send(string message, bool isServerMessage = false);
    }
}
