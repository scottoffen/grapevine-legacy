using System;
using Grapevine.Interfaces.Server;

namespace Grapevine.Exceptions.Server
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FileNotFoundException : NotFoundException
    {
        public FileNotFoundException(IHttpContext context) : base($"File {context.Request.PathInfo} was not found") { }
    }

    /// <summary>
    /// Thrown when no routes are found for the provided context.
    /// </summary>
    public class RouteNotFoundException : NotFoundException
    {
        public RouteNotFoundException(IHttpContext context) : base($"Route Not Found For {context.Request.HttpMethod} {context.Request.PathInfo}") { }
    }
}
