using System;

namespace Grapevine.Server.Exceptions
{
    public class DynamicValueNotFoundException : Exception
    {
        public DynamicValueNotFoundException(string key) : base($"Propery {key} not found") { }
    }
}
