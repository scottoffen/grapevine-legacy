using System;
using Grapevine.Server;

namespace Grapevine.Util
{
    /// <summary>
    /// Thrown when there is an attempt to modify the Protocol, Host, Port or Connections property of a running instance of RestServer.
    /// </summary>
    public class ServerStateException : Exception
    {
        public ServerStateException() : base("Protocol, Host, Port and Connections properties cannot be modified while the server is running.") { }

        public ServerStateException(string message) : base(message) { }

        public ServerStateException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when a method being used as a Route has multiple or invalid arguments.
    /// </summary>
    public class RouteMethodArgumentException : Exception
    {
        public RouteMethodArgumentException() { }

        public RouteMethodArgumentException(string message) : base(message) { }

        public RouteMethodArgumentException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when attempting to access the PathInfo property of a RESTRequest where not all resource tokens are able to be resolved.
    /// </summary>
    public class ClientStateException : Exception
    {
        public ClientStateException() : base("Unresolved Resource Tokens") { }

        public ClientStateException(string message) : base(message) { }

        public ClientStateException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when the property key does not exist in the Dynamic property collection.
    /// </summary>
    public class DynamicValueNotFoundException : Exception
    {
        public DynamicValueNotFoundException() { }

        public DynamicValueNotFoundException(string propertyName)
        : base($"Key '{propertyName}' not found; use HasDynamicKey to determine if property exists first") { }

        public DynamicValueNotFoundException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when the value of the property key in the Dynamic property collection does not match the type it is attempting to be cast to.
    /// </summary>
    public class DynamicPropertyTypeMismatch : Exception
    {
        public DynamicPropertyTypeMismatch() { }

        public DynamicPropertyTypeMismatch(string propertyName, string propertyType, string expectedType)
        : base($"Value for key {propertyName} of type {propertyType} is not of type {expectedType}") { }

        public DynamicPropertyTypeMismatch(string message) : base(message) { }

        public DynamicPropertyTypeMismatch(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when the value of the property key in the Dynamic property collection does not match the type it is attempting to be cast to.
    /// </summary>
    public class PatternNotParseableToValidRegex : Exception
    {
        public PatternNotParseableToValidRegex() { }

        public PatternNotParseableToValidRegex(string message) : base(message) { }

        public PatternNotParseableToValidRegex(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when no routes are found for the provided context.
    /// </summary>
    public class RouteNotFound : Exception
    {
        public RouteNotFound() { }

        public RouteNotFound(string message) : base(message) { }

        public RouteNotFound(IHttpContext context) : base($"Route Not Found For {context.Request.HttpMethod} {context.Request.PathInfo}") { }

        public RouteNotFound(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when the http host is unable to start.
    /// </summary>
    public class CantStartHostException : Exception
    {
        public CantStartHostException() { }

        public CantStartHostException(string message) : base(message) { }

        public CantStartHostException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when the http host is unable to stop.
    /// </summary>
    public class CantStopHostException : Exception
    {
        public CantStopHostException() { }

        public CantStopHostException(string message) : base(message) { }

        public CantStopHostException(string message, Exception inner) : base(message, inner) { }
    }

    /// <summary>
    /// Thrown when the http host encounters an exception while running.
    /// </summary>
    public class HostRunningException : Exception
    {
        public HostRunningException() { }

        public HostRunningException(string message) : base(message) { }

        public HostRunningException(string message, Exception inner) : base(message, inner) { }
    }
}
