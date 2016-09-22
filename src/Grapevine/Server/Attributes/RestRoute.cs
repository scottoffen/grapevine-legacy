using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;

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
            if (method.GetCustomAttributes(true).Any(a => a is RestRoute))
                return method.IsRestRouteEligible(throwExceptionWhenFalse);
            if (!throwExceptionWhenFalse) return false;

            var exception = new InvalidRouteMethodException($"{method.Name} does not have the RestRoute attribute");
            throw new InvalidRouteMethodExceptions(new Exception[] {exception});
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

            // Does the type have a parameterless constructor?
            if (method.ReflectedType != null && !method.ReflectedType.HasParameterlessConstructor()) exceptions.Add(new InvalidRouteMethodException($"{method.Name} does not have a parameterless constructor"));

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
            /*
             * The first set of checks are on the method itself:
             * - Static methods can always be invoked
             * - Abstract methods can never be invoked
             */
            if (method.IsStatic) return true;
            if (method.IsAbstract) return false;

            /*
             * The second set of checks are on the type the method
             * comes from. This uses the ReflectedType property,
             * which will be the same property used by the Route
             * class to invoke the method later on.
             * - ReflectedType can not be null
             * - ReflectedType can not be abstract
             * - ReflectedType must be a class (vs an interface or struct, etc.)
             */
            var type = method.ReflectedType;
            if (type == null) return false;
            if (!type.IsClass) return false;
            if (type.IsAbstract) return false;

            /*
             * If these checks have all passed, then we can be fairly certain
             * that the method can be invoked later on during routing.
             */

            return (true);
        }

        /// <summary>
        /// Returns a value indicating whether the type has a constructor that takes no parameters
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static bool HasParameterlessConstructor(this Type type)
        {
            return (type.GetConstructor(Type.EmptyTypes) != null);
        }
    }
}