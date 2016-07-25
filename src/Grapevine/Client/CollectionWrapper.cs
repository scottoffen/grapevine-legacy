using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Grapevine.Client.Exceptions;

namespace Grapevine.Client
{
    public abstract class CollectionWrapper
    {
        protected NameValueCollection Collection = new NameValueCollection();

        /// <summary>
        /// Adds and entry with the specified name and value to the collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            Collection.Add(name, value);
        }

        /// <summary>
        /// Gets the number of key/value pairs in the collection
        /// </summary>
        public int Count => Collection.Count;

        /// <summary>
        /// Gets the value of the specified name from the collection
        /// </summary>
        /// <param name="name"></param>
        /// <returns>string</returns>
        public string Get(string name)
        {
            return Collection[name];
        }

        /// <summary>
        /// Removes the entries with the specified key from the collection
        /// </summary>
        /// <param name="name"></param>
        public void Remove(string name)
        {
            Collection.Remove(name);
        }

        /// <summary>
        /// Invalidates all cached arrays and removes all entries from the collection
        /// </summary>
        public void Clear()
        {
            Collection.Clear();
        }
    }

    /// <summary>
    /// Provides simplified access the key/value pairs of the query string of the request
    /// </summary>
    public class QueryString : CollectionWrapper
    {
        public override string ToString()
        {
            return Collection.Count <= 0 ? string.Empty : string.Join("&", (from key in Collection.AllKeys let value = Collection.Get(key) select Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value)).ToArray());
        }
    }

    /// <summary>
    /// Provides simplified assignment of values to placeholders in the PathInfo of the request
    /// </summary>
    public class PathParams : CollectionWrapper
    {
        /// <summary>
        /// Gets a copy of the resource with placeholders replaced by values in the collection
        /// </summary>
        public string ParseResource(string resource)
        {
            var pathinfo = Collection.AllKeys.Aggregate(resource, (current, key) => Regex.Replace(current, $@"\[{key}\]", Collection.Get(key)));
            if (pathinfo.Contains("[") || pathinfo.Contains("]")) throw new ClientStateException($"Not all parameters were replaced in request resource: {pathinfo}");
            return pathinfo;
        }
    }

}
