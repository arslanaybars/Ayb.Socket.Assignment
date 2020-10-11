using System;
using System.Threading;
using Ayb.Socket.Assignment.Client;
using Ayb.Socket.Assignment.Server;
using Ayb.Socket.Assignment.Shared;
using Xunit;

namespace Ayb.Socket.Assignment.Tests
{
    public class Tests
    {
        private readonly IClient _client;

        public Tests()
        {
            // may write FakeTcpClient and FakeTcpServer
            Thread t = new Thread(delegate ()
            {
                IServer s = new TcpServer(Constant.Port);
                s.SetupServer();
            });
            t.Start();

            _client = FakeTcpClientFactory.Instance;
            _client.ConnectToServer();

        }

        [Fact]
        public void Send_Message_Should_Return_Success()
        {
            // Arrange
            var message = "text";

            // Act
            var response = _client.Send(message);

            // Assert
            Assert.True(response.Success);
            Assert.True(response.Data == message);
        }

        [Fact]
        public void Send_Message_To_Server_Should_Not_Throw_Exception()
        {
            // Arrange

            // Act
            var ex = Record.Exception(() => _client.Send("message"));

            // Assert
            Assert.IsNotType<Exception>(ex);
        }

        [Fact]
        public void Send_Message_From_Multiple_Client_To_Server_Should_Not_Throw_Exception()
        {
            // Arrange
            var secondClient = new TcpClient(Constant.Ip, Constant.Port, Guid.NewGuid().ToString());
            secondClient.ConnectToServer();

            // Act
            var ex = Record.Exception(() => _client.Send("message"));
            var act = Record.Exception(() => secondClient.Send("message2"));

            // Assert
            Assert.IsNotType<Exception>(ex);
            Assert.IsNotType<Exception>(act);
        }
    }
}
