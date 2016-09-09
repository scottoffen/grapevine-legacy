using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Server.Exceptions;
using Grapevine.Util;

namespace Grapevine.Server.Attributes
{
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
        /// A parameterized pattern or regular expression string to match against the value of HttpContext.Request.PathInfo; defaults to an empty string
        /// </summary>
        public string PathInfo { get; set; }

        public RestRoute()
        {
            HttpMethod = HttpMethod.ALL;
            PathInfo = string.Empty;
        }
    }

    public static class RestRouteExtensions
    {
        /// <summary>
        /// Returns an enumerable of all RestRoute attributes on a method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static IEnumerable<RestRoute> GetRouteAttributes(this MethodInfo method)
        {
            return method.GetCustomAttributes(true).Where(a => a is RestRoute).Cast<RestRoute>();
        }

        /// <summary>
        /// Returns a value indicating whether the method is a valid RestRoute
        /// </summary>
        internal static bool IsRestRoute(this MethodInfo method, bool throwExceptionWhenFalse = false)
        {
            return method.GetCustomAttributes(true).Any(a => a is RestRoute) && method.IsRestRouteEligible(throwExceptionWhenFalse);
        }

        /// <summary>
        /// Returns a value indicating that the method can be used to create a Route object
        /// </summary>
        /// <param name="method"></param>
        /// <param name="throwExceptionWhenFalse"></param>
        /// <returns></returns>
        internal static bool IsRestRouteEligible(this MethodInfo method, bool throwExceptionWhenFalse = false)
        {

            if (method == null) throw new ArgumentNullException(nameof(method));

            var exceptions = new List<Exception>();

            // Can the method be invoked?
            if (!method.CanInvoke()) exceptions.Add(new InvalidRouteMethodException($"{method.Name} cannot be invoked"));

            // Can not have a special name (getters and setters)
            if (method.IsSpecialName) exceptions.Add(new InvalidRouteMethodException($"{method.Name} may be treated in a special way by some compilers (such as property accessors and operator overloading methods)"));

            // Method must have a return value of IHttpContext
            if (method.ReturnType != typeof(IHttpContext)) exceptions.Add(new InvalidRouteMethodException($"{method.Name} must have a return value of type {typeof(IHttpContext).Name}"));

            var args = method.GetParameters();

            // Method must have only one argument
            if (args.Length != 1) exceptions.Add(new InvalidRouteMethodException($"{method.Name} must accept one and only one argument"));

            // First argument to method must be of type IHttpContext
            if (args.Length > 0 && args[0].ParameterType != typeof(IHttpContext)) exceptions.Add(new InvalidRouteMethodException($"{method.Name}: first argument must be of type {typeof(IHttpContext).Name}"));

            // Return boolean value
            if (exceptions.Count == 0) return true;
            if (!throwExceptionWhenFalse) return false;

            // Throw exeception
            throw new InvalidRouteMethodExceptions(exceptions.ToArray());
        }

        /// <summary>
        /// Returns a value indicating that the method can be invoked via reflection
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static bool CanInvoke(this MethodInfo method)
        {
            // static methods can always be invoked
            return !method.IsAbstract && (method.ReflectedType == null || !method.ReflectedType.IsAbstract);
        }
    }
}