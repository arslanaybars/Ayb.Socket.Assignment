using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Ayb.Socket.Assignment.Shared;

namespace Ayb.Socket.Assignment.Server
{
    public class TcpServer : IServer
    {
        private System.Net.Sockets.Socket _serverSocket;
        private System.Net.Sockets.Socket _current;

        private readonly int _port;

        private const int BufferSize = Constant.BufferSize;
        private readonly List<System.Net.Sockets.Socket> _clientSockets = new List<System.Net.Sockets.Socket>();
        private readonly byte[] _buffer = new byte[BufferSize];
        private readonly List<User> _users = new List<User>();

        private readonly Dictionary<string, User> _store = new Dictionary<string, User>();

        public TcpServer(int port)
        {
            _port = port;
        }

        /// <summary>
        /// Setup the server
        /// </summary>
        public void SetupServer()
        {
            try
            {
                Console.WriteLine("Setting up server...");
                _serverSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(IPAddress.Any, _port));
                _serverSocket.Listen(5);
                _serverSocket.BeginAccept(AcceptCallback, null);
                Console.WriteLine("Server setup complete");
                Console.WriteLine("Listening on port: " + _port);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Close the connections
        /// </summary>
        /// <returns></returns>
        public ServiceResponse CloseAllSockets()
        {
            try
            {
                foreach (System.Net.Sockets.Socket socket in _clientSockets)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }

                _serverSocket.Close();
                return new ServiceResponse(true, Constant.Messages.Success);
            }
            catch (Exception e)
            {
                return new ServiceResponse(false, Constant.Messages.Fail, e);
            }
        }

        /// <summary>
        /// Sending message to connected sockets
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isServerMessage">if is sever message skip the RemoteEndpoint condition</param>
        /// <returns></returns>
        public ServiceResponse<string> Send(string message, bool isServerMessage = false)
        {
            try
            {
                var data = Encoding.ASCII.GetBytes(message);
                foreach (var clientSocket in _clientSockets)
                {
                    if (ValidateSender(isServerMessage, clientSocket))
                    {
                        var (discipline, response) = Discipline();
                        if (discipline) return response;

                        continue;
                    }

                    clientSocket.Send(data);
                }

                return new ServiceResponse<string>(true, Constant.Messages.Success, message);
            }
            catch (Exception e)
            {
                return new ServiceResponse<string>(false, Constant.Messages.Fail, e);
            }
        }

        /// <summary>
        /// User has discipline punish or not!
        /// </summary>
        /// <returns></returns>
        private (bool, ServiceResponse<string>) Discipline()
        {
            var user = _store[_current.RemoteEndPoint.ToString()];
            if (user.Status == Status.Yellow)
            {
                user.Socket.Send(Encoding.ASCII.GetBytes(Constant.Messages.ConnectionWarning));
                {
                    return (true,
                        new ServiceResponse<string>(true, Constant.Messages.Success, Constant.Messages.ConnectionWarning));
                }
            }

            if (user.Status == Status.Red)
            {
                user.Socket.Send(Encoding.ASCII.GetBytes(Constant.Messages.ConnectionDropped));
                {
                    user.Socket.Close();
                    return (true,
                        new ServiceResponse<string>(true, Constant.Messages.Success, Constant.Messages.ConnectionDropped));
                }
            }

            return (false, null);
        }

        private bool ValidateSender(bool isServerMessage, System.Net.Sockets.Socket clientSocket)
        {
            if (clientSocket.RemoteEndPoint == _current.RemoteEndPoint && !isServerMessage)
            {
                var user = _store[_current.RemoteEndPoint.ToString()];
                if (user.LastMessage > DateTime.Now.AddSeconds(Constant.AllowedMessageIntervalAsSecond))
                {
                    user.Alert();
                }

                user.LastMessage = DateTime.Now;
                return true;
            }

            return false;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            System.Net.Sockets.Socket socket;

            try
            {
                socket = _serverSocket.EndAccept(ar);
                _clientSockets.Add(socket);
                socket.BeginReceive(_buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, socket);
                Console.WriteLine("Client connected: " + socket.RemoteEndPoint);
                _serverSocket.BeginAccept(AcceptCallback, null);
            }
            catch (Exception e)
            {
                // Log.Error
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            _current = (System.Net.Sockets.Socket)ar.AsyncState;
            int received;

            try
            {
                received = _current.EndReceive(ar);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                _current.Close();
                _clientSockets.Remove(_current);
                return;
            }

            var text = GenerateTextFromReceived(received);
            if (text.Contains("auth:"))
            {
                var newUser = text.Replace("auth:", string.Empty);
                var user = new User(newUser, _current, _current.RemoteEndPoint.ToString());
                _users.Add(user);
                _store.Add(user.RemoteEndPoint, user);
                Console.WriteLine(newUser + " has joined the room.\n");
            }
            else
            {
                Send(text);
            }

            if (_current != null && (_current.Connected || !_current.Blocking))
            {
                _current.BeginReceive(_buffer, 0, BufferSize, SocketFlags.None, ReceiveCallback, _current);
            }
        }

        private string GenerateTextFromReceived(int received)
        {
            var recBuf = new byte[received];
            Array.Copy(_buffer, recBuf, received);
            var text = Encoding.ASCII.GetString(recBuf);
            return text;
        }
    }
}
