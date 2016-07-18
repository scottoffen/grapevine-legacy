using System;
using System.Collections.Generic;
using System.Dynamic;
using Grapevine.Util;

namespace Grapevine.Server
{
    /// <summary>
    /// Adds an ExpandoObject called Dynamic to a class
    /// </summary>
    public abstract class DynamicAspect
    {
        /// <summary>
        /// Dynamic object for run-time extension
        /// </summary>
        public dynamic Dynamic { get; } = new ExpandoObject();
    }

    public static class ExpandoObjectExtensions
    {
        /// <summary>
        /// Gets the value of the specified key name in the ExpandoObject and casts it to the type specifed by <c>T</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expando"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static T GetValueAs<T>(this ExpandoObject expando, string keyName)
        {
            if (string.IsNullOrWhiteSpace(keyName)) throw new ArgumentNullException(nameof(keyName));

            var properties = expando as IDictionary<string, object>;
            if (properties == null || !properties.ContainsKey(keyName)) throw new DynamicValueNotFoundException(keyName);

            var property = properties[keyName];
            if (property is T) return (T)property;

            throw new DynamicPropertyTypeMismatch(keyName, property.GetType().Name, typeof(T).Name);
        }

        /// <summary>
        /// Gets a value indicating whether or not the ExpandoObject has a property with the given key name.
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool HasKey(this ExpandoObject expando, string keyName)
        {
            var dictionary = expando as IDictionary<string, object>;
            return dictionary != null && dictionary.ContainsKey(keyName);
        }
    }
}
