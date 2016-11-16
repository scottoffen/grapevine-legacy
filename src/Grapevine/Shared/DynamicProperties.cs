using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Grapevine.Exceptions.Server;

namespace Grapevine.Shared
{
    public interface IDynamicProperties
    {
        /// <summary>
        /// Gets a concurrent dictionary object available for adding application-specific properties at run-time
        /// </summary>
        IDictionary<string, object> Properties { get; }
    }

    public abstract class DynamicProperties : IDynamicProperties
    {
        private ConcurrentDictionary<string, object> _properties;

        public IDictionary<string, object> Properties
        {
            get
            {
                if (_properties != null) return _properties;
                _properties = new ConcurrentDictionary<string, object>();
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

            if (!props.Properties.ContainsKey(key)) throw new DynamicValueNotFoundException(key);

            var property = props.Properties[key];
            if (property is T) return (T)property;

            var result = Convert.ChangeType(property, typeof(T));
            return (T)result;
        }

        public static bool ContainsProperty(this IDynamicProperties props, string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException(nameof(key));
            return props.Properties.ContainsKey(key);
        }
    }
}