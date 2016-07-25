using System;

namespace Grapevine.Server.Exceptions
{
    /// <summary>
    /// Thrown when a method being used as a Route has multiple or invalid arguments.
    /// </summary>
    public class RouteMethodArgumentException : Exception
    {
        public RouteMethodArgumentException(string message) : base(message) { }
    }
}