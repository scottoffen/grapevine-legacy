using System;namespace Grapevine.Server.Exceptions
{
    public class DynamicPropertyTypeMismatchException : Exception
    {
        public DynamicPropertyTypeMismatchException(string key, string actualType, string expectedType):base($"Property {key} is of type {actualType}, not {expectedType}") { }
    }
}
