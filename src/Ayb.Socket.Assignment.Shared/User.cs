using System;

namespace Ayb.Socket.Assignment.Shared
{
    public sealed class User
    {
        public User(string name, System.Net.Sockets.Socket socket, string remoteEndPoint)
        {
            Name = name;
            Socket = socket;
            RemoteEndPoint = remoteEndPoint;
            Status = Status.Green;
        }

        /// <summary>
        /// Alert user if he/she send message less than 1 sec interval
        /// </summary>
        public void Alert()
        {
            switch (Status)
            {
                case Status.Green:
                    Status = Status.Yellow;
                    break;
                case Status.Yellow:
                    Status = Status.Red;
                    break;
            }
        }

        public string Name { get; set; }

        public DateTime LastMessage { get; set; }

        public string RemoteEndPoint { get; set; }

        public Status Status { get; set; }

        public System.Net.Sockets.Socket Socket { get; set; }
    }
}
