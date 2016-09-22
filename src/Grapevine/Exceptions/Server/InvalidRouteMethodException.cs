using System;

namespace Grapevine.Exceptions.Server
{
    /// <summary>
    /// Thrown when a method is not eligible to be used as a route
    /// </summary>
    public class InvalidRouteMethodException : Exception
    {
        public InvalidRouteMethodException(string message) : base(message) { }
    }

    /// <summary>
    /// An aggregate of exceptions thrown when a method is not eligible to be used as a route
    /// </summary>
    public class InvalidRouteMethodExceptions : AggregateException
    {
        public InvalidRouteMethodExceptions(Exception[] exceptions) : base(exceptions) { }
    }
}
