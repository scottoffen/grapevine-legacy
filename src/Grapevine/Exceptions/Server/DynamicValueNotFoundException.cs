using System;

namespace Grapevine.Exceptions.Server
{
    public class DynamicValueNotFoundException : Exception
    {
        public DynamicValueNotFoundException(string key) : base($"Propery {key} not found") { }
    }
}
