using System;

namespace Grapevine
{
    public class ServerStateException : Exception
    {
        public ServerStateException() : base("Host, Port and MaxThreads cannot be modified while the server is running.")
        {
        }

        public ServerStateException(string message) : base(message)
        {
        }

        public ServerStateException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
