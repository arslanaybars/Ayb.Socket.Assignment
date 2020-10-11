using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Client
{
    public class TcpClient : IClient
    {
        private readonly System.Net.Sockets.Socket _clientSocket;

        private readonly string _ip;
        private readonly int _port;
        private readonly string _username;

        public TcpClient(string ip, int port, string username)
        {
            _ip = ip;
            _port = port;
            _username = username;
            _clientSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        /// <summary>
        /// Connection to the server
        /// </summary>
        /// <returns></returns>
        public ServiceResponse ConnectToServer()
        {
            try
            {
                var address = IPAddress.Parse(_ip);
                _clientSocket.Connect(address, _port);
                Send("auth:");
                return new ServiceResponse(true, Constant.Messages.Success);
            }
            catch (SocketException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
                Console.ReadLine();
                return new ServiceResponse(false, Constant.Messages.Fail, e);
            }
        }

        /// <summary>
        /// Sending message to server
        /// </summary>
        /// <param name="text">The message that you are sending</param>
        /// <returns></returns>
        public ServiceResponse<string> Send(string text)
        {
            try
            {
                var buffer = Encoding.ASCII.GetBytes(_username + ": " + text);
                _clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                return new ServiceResponse<string>(true, Constant.Messages.Success, text);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ServiceResponse<string>(false, Constant.Messages.Fail, e);
            }
        }

        /// <summary>
        /// Receive messages from the server
        /// </summary>
        /// <returns></returns>
        public ServiceResponse<string> ReceiveResponse()
        {
            try
            {
                var buffer = new byte[2048];
                var received = _clientSocket.Receive(buffer, SocketFlags.None);
                if (received > 0)
                {
                    var data = new byte[received];
                    Array.Copy(buffer, data, received);
                    var text = Encoding.ASCII.GetString(data);
                    return new ServiceResponse<string>(true, Constant.Messages.Success, text);
                }

                return new ServiceResponse<string>(true, Constant.Messages.Success, string.Empty);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ServiceResponse<string>(false, Constant.Messages.Fail, e);
            }
        }

        public void Exit()
        {
            _clientSocket.Shutdown(SocketShutdown.Both);
            _clientSocket.Close();
        }
    }
}
