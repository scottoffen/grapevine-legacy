using System;

namespace Grapevine.Server.Exceptions
{
    /// <summary>
    /// Thrown when the http host is unable to start.
    /// </summary>
    public class UnableToStartHostException : Exception
    {
        public UnableToStartHostException(string message, Exception inner) : base(message, inner) { }
    }
}