using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Client
{
    public interface IClient
    {
        ServiceResponse ConnectToServer();

        ServiceResponse<string> Send(string text);

        ServiceResponse<string> ReceiveResponse();

        void Exit();
    }
}
