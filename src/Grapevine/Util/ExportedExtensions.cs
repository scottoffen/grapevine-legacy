using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Grapevine.Util
{
    public static class ExportedExtensions
    {
        public static T GetValue<T>(this NameValueCollection collection, string key)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection), "Missing collection");
            if (collection[key] == null) throw new ArgumentOutOfRangeException(nameof(key), "Missing key");

            var value = collection[key];
            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (!converter.CanConvertFrom(typeof(string))) throw new ArgumentException($"Cannot convert '{value}' to {typeof(T)}");
            return (T) converter.ConvertFrom(value);
        }

        public static T GetValue<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection), "Missing collection");
            if (collection[key] == null) return defaultValue;

            var value = collection[key];
            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (!converter.CanConvertFrom(typeof(string))) throw new ArgumentException($"Cannot convert '{value}' to {typeof(T)}");

            return (T) converter.ConvertFrom(value);
        }
    }
}
