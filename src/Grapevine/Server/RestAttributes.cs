using System;
using System.Linq;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server
{
    /// <summary>
    /// <para>Class attribute for defining RestResource</para>
    /// <para>Targets: Class</para>
    /// <para>&#160;</para>
    /// <para>A class with the RestResource attribute can be scanned for RestRoute attributed methods</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RestResource : Attribute
    {
        /// <summary>
        /// This value will be prepended to the PathInfo value on all RestRoutes in the class, defaults to an empty string
        /// </summary>
        public string BaseUrl { get; set; }
        public string Scope { get; set; }

        public RestResource()
        {
            BaseUrl = string.Empty;
            Scope = string.Empty;
        }
    }

    /// <summary>
    /// <para>Method attribute for defining a RestRoute</para>
    /// <para>Targets: Method, AllowMultipe: true</para>
    /// <para>&#160;</para>
    /// <para>A method with the RestRoute attribute can have traffic routed to it by a RestServer if the request matches the assigned Method and PathInfo properties</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RestRoute : Attribute
    {
        /// <summary>
        /// The HttpMethod this route will respond to; defaults to HttpMethod.ALL
        /// </summary>
        public HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// A parameterized pattern or regular expression string to match against the value of HttpContext.Request.PathInfo; defaults to @"^.*$"
        /// </summary>
        public string PathInfo { get; set; }

        public RestRoute()
        {
            HttpMethod = HttpMethod.ALL;
            PathInfo = string.Empty;
        }
    }

    public static class RestAttributeExtentions
    {
        /// <summary>
        /// Returns true if the method is a valid RestRoute
        /// </summary>
        internal static bool IsRestRoute(this MethodInfo method)
        {
            return !method.IsAbstract && !method.IsConstructor && !method.IsPrivate && !method.IsVirtual &&
                   !method.IsSpecialName && method.DeclaringType == method.ReflectedType &&
                   method.GetCustomAttributes(true).Any(a => a is RestRoute);
        }

        /// <summary>
        /// Returns true if the type is a valid RestResource
        /// </summary>
        internal static bool IsRestResource(this Type type)
        {
            return !type.IsAbstract && type.IsClass && type.GetCustomAttributes(true).Any(a => a is RestResource);
        }

        /// <summary>
        /// Returns the value of the RestResource attribute; returns null if the type is not have a RestResource attribute
        /// </summary>
        internal static RestResource RestResource(this Type type)
        {
            if (!type.IsRestResource()) return null;
            return (RestResource)type.GetCustomAttributes(true).First(a => a is RestResource);
        }
    }
}
