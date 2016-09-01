using System;
using System.Collections.Generic;
using Grapevine.Server.Exceptions;

namespace Grapevine.Server
{
    public static class DynamicPropertiesExtensions
    {
        public static T GetPropertyValueAs<T>(this IDynamicProperties props, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var dictionary = props.Properties as IDictionary<string, object>;
            if (dictionary == null || !dictionary.ContainsKey(key)) throw new DynamicValueNotFoundException(key);

            var property = dictionary[key];
            if (property is T) return (T)property;

            var result = Convert.ChangeType(property, typeof(T));
            return (T)result;
        }

        public static bool ContainsProperty(this IDynamicProperties props, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var dictionary = props.Properties as IDictionary<string, object>;
            return dictionary != null && dictionary.ContainsKey(key);
        }
    }
}
