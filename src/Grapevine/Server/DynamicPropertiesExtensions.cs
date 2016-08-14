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

            var properties = props.Properties as IDictionary<string, object>;
            if (properties == null || !properties.ContainsKey(key)) throw new DynamicValueNotFoundException(key);

            var property = properties[key];
            if (property is T) return (T)property;

            var result = Convert.ChangeType(property, typeof(T));
            if (result != null) return (T)result;

            throw new DynamicPropertyTypeMismatchException(key, property.GetType().Name, typeof(T).Name);
        }

        public static bool ContainsProperty(this IDynamicProperties props, string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));

            var dictionary = props.Properties as IDictionary<string, object>;
            return dictionary != null && dictionary.ContainsKey(key);
        }
    }
}
