using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Grapevine.Util;

namespace Grapevine.Client
{
    public abstract class CollectionWrapper
    {
        protected NameValueCollection Collection = new NameValueCollection();

        public void Add(string key, string value)
        {
            Collection.Add(key, value);
        }

        public int Count => Collection.Count;

        public string Get(string key)
        {
            return Collection[key];
        }

        public void Remove(string key)
        {
            Collection.Remove(key);
        }

        public void Clear()
        {
            Collection.Clear();
        }
    }

    public class QueryString : CollectionWrapper
    {
        public override string ToString()
        {
            return Collection.Count <= 0 ? string.Empty : string.Join("&", (from key in Collection.AllKeys let value = Collection.Get(key) select HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value)).ToArray());
        }
    }

    public class PathParams : CollectionWrapper
    {
        public string ParseResource(string resource)
        {
            var pathinfo = Collection.AllKeys.Aggregate(resource, (current, key) => Regex.Replace(current, $@"\[{key}\]", Collection.Get(key)));
            if (pathinfo.Contains("[") || pathinfo.Contains("]")) throw new ClientStateException($"Not all parameters were replaced in request resource: {pathinfo}");
            return pathinfo;
        }
    }

}
