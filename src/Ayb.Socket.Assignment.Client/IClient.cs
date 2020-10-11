using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Client
{
    public interface IClient
    {
        public ServiceResponse ConnectToServer();

        public ServiceResponse<string> Send(string text);

        public ServiceResponse<string> ReceiveResponse();

        public void Exit();
    }
}
