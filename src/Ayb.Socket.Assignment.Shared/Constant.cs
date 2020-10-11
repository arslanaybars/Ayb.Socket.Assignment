namespace Ayb.Socket.Assignment.Shared
{
    public static class Constant
    {
        public const string Ip = "127.0.0.1";

        public const int Port = 3010;

        public const int BufferSize = 2048;

        public const int AllowedMessageIntervalAsSecond = -1;

        public const string Eof = "EOF";

        public static class Messages
        {
            public const string Fail = "fail";
            public const string Success = "success";

            public const string Info = "To exit from app please write 'EOF' to the text";
            public const string Text = "Type your text : ";
            public const string EnterYourUsername = "Enter your username : ";

            public const string ConnectionDropped = "Your connection finished";
            public const string ConnectionWarning = "You type message less than 1 minute, if you do the same you will chat will finished.";
        }
    }
}
