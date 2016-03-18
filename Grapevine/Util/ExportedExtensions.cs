using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Grapevine.Util
{
    public static class ExportedExtensions
    {
        public static T GetValue<T>(this NameValueCollection collection, string key)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Missing collection");
            }

            var value = collection[key];

            if (value == null)
            {
                throw new ArgumentOutOfRangeException("Missing key");
            }

            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (!converter.CanConvertFrom(typeof(string)))
            {
                throw new ArgumentException(String.Format("Cannot convert '{0}' to {1}", value, typeof(T)));
            }

            return (T) converter.ConvertFrom(value);
        }

        public static T GetValue<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("Missing collection");
            }

            var value = collection[key];

            if (value == null) return defaultValue;

            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (!converter.CanConvertFrom(typeof(string)))
            {
                throw new ArgumentException(String.Format("Cannot convert '{0}' to {1}", value, typeof(T)));
            }

            return (T) converter.ConvertFrom(value);
        }
    }
}
