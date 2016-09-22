using System;
using System.Collections.Generic;
using System.Dynamic;
using Grapevine.Exceptions.Server;

namespace Grapevine.Shared
{
    public interface IDynamicProperties
    {
        /// <summary>
        /// Gets a dynamic object available for adding dynamic properties at run-time
        /// </summary>
        dynamic Properties { get; }
    }

    public abstract class DynamicProperties : IDynamicProperties
    {
        private ExpandoObject _properties;

        public dynamic Properties
        {
            get
            {
                if (_properties != null) return _properties;
                _properties = new ExpandoObject();
                return _properties;
            }
        }
    }

    public static class DynamicPropertiesExtensions
    {
        public static T GetPropertyValueAs<T>(this IDynamicProperties props, string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(nameof(key));

            var dictionary = props.Properties as IDictionary<string, object>;
            if (dictionary == null || !dictionary.ContainsKey(key)) throw new DynamicValueNotFoundException(key);

            var property = dictionary[key];
            if (property is T) return (T)property;

            var result = Convert.ChangeType(property, typeof(T));
            return (T)result;
        }

        public static bool ContainsProperty(this IDynamicProperties props, string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(nameof(key));

            var dictionary = props.Properties as IDictionary<string, object>;
            return dictionary != null && dictionary.ContainsKey(key);
        }
    }
}