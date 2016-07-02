using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Grapevine.Util
{
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Gets the value for the specified key cast the type specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <returns>object of type &lt;T&gt;</T></returns>
        public static T GetValue<T>(this NameValueCollection collection, string key)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection), "Missing collection");
            if (collection[key] == null) throw new ArgumentOutOfRangeException(nameof(key), "Missing key");

            var value = collection[key];
            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (!converter.CanConvertFrom(typeof(string))) throw new ArgumentException($"Cannot convert '{value}' to {typeof(T)}");
            return (T) converter.ConvertFrom(value);
        }

        /// <summary>
        /// Gets the value for the specified key cast the type specified or the default value if the key does not exist in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns>object of type &lt;T&gt;</T></returns>
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
