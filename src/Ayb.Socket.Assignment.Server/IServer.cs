using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Server
{
    public interface IServer
    {
        void SetupServer();

        ServiceResponse CloseAllSockets();

        ServiceResponse<string> Send(string message, bool isServerMessage = false);
    }
}
