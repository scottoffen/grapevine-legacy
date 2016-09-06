using System;
using System.Linq;
using System.Reflection;

namespace Grapevine.Server.Attributes
{
    /// <summary>
    /// Provides extension methods for various classes related to Rest attributes
    /// </summary>
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
        /// Returns true if the type is a valid GetRestResource
        /// </summary>
        internal static bool IsRestResource(this Type type)
        {
            return !type.IsAbstract && type.IsClass && type.GetCustomAttributes(true).Any(a => a is RestResource);
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