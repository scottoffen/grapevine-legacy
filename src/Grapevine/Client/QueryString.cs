using System;
using System.Collections.Specialized;
using System.Linq;

namespace Grapevine.Client
{
    /// <summary>
    /// Provides access the key/value pairs of the query string of the request
    /// </summary>
    public class QueryString : NameValueCollection
    {
        public string Stringify()
        {
            return Count <= 0 ? string.Empty : string.Join("&", (from key in AllKeys let value = Get(key) select Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(value)).ToArray());
        }
    }
}