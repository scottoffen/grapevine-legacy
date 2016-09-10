using System;
using System.Linq;

namespace Grapevine.Server.Attributes
{
    /// <summary>
    /// <para>Class attribute for defining a GetRestResource</para>
    /// <para>Targets: Class</para>
    /// <para>&#160;</para>
    /// <para>A class with the GetRestResource attribute can be scanned for RestRoute attributed methods</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RestResource : Attribute
    {
        /// <summary>
        /// This value will be prepended to the PathInfo value on all RestRoutes in the class, defaults to an empty string
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// When set, will limit the accessibility of RestRoutes to Servers with the same Scope; defaults to no scope (an empty string)
        /// </summary>
        public string Scope { get; set; }

        public RestResource()
        {
            BasePath = string.Empty;
            Scope = string.Empty;
        }
    }

    public static class RestResourceExtensions
    {
        /// <summary>
        /// Returns true if the type is a valid GetRestResource
        /// </summary>
        internal static bool IsRestResource(this Type type)
        {
            if (!type.IsClass) return false;
            if (type.IsAbstract) return false;
            return type.GetCustomAttributes(true).Any(a => a is RestResource);
        }

        /// <summary>
        /// Returns the value of the GetRestResource attribute; returns null if the type does not have a GetRestResource attribute
        /// </summary>
        internal static RestResource GetRestResource(this Type type)
        {
            if (!type.IsRestResource()) return null;
            return (RestResource)type.GetCustomAttributes(true).First(a => a is RestResource);
        }
    }
}