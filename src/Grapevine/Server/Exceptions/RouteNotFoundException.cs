using System;

namespace Grapevine.Server.Exceptions
{
    /// <summary>
    /// Thrown when no routes are found for the provided context.
    /// </summary>
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException(IHttpContext context) : base($"Route Not Found For {context.Request.HttpMethod} {context.Request.PathInfo}") { }
    }
}