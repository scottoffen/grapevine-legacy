using System;
using System.Collections.Generic;
using System.Dynamic;
using Grapevine.Util;

namespace Grapevine.Server
{
    public interface IDynamicAspect
    {
        dynamic Dynamic { get; }

        T GetDynamicValue<T>(string propertyName);

        bool HasDynamicKey(string keyName);
    }

    public abstract class DynamicAspect : IDynamicAspect
    {
        public dynamic Dynamic { get; } = new ExpandoObject();

        public T GetDynamicValue<T>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException(nameof(propertyName));

            var properties = Dynamic as IDictionary<string, object>;
            if (properties == null || !properties.ContainsKey(propertyName)) throw new DynamicValueNotFoundException(propertyName);

            var property = properties[propertyName];
            if (property is T) return (T) property;

            throw new DynamicPropertyTypeMismatch(propertyName, property.GetType().Name, typeof(T).Name);
        }

        public bool HasDynamicKey(string keyName)
        {
            var dictionary = Dynamic as IDictionary<string, object>;
            return dictionary != null && dictionary.ContainsKey(keyName);
        }
    }
}
