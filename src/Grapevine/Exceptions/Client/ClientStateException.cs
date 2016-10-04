using System;

namespace Grapevine.Exceptions.Client
{
    /// <summary>
    /// Thrown when attempting to access the PathInfo property of Client.RestRequest where not all resource tokens are able to be resolved.
    /// </summary>
    public class ClientStateException : Exception
    {
        public ClientStateException(string message) : base(message) { }
    }

}
