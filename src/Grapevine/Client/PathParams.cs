using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Grapevine.Exceptions.Client;

namespace Grapevine.Client
{
    /// <summary>
    /// Provides simplified assignment of values to placeholders in the PathInfo of the request
    /// </summary>
    public class PathParams : NameValueCollection
    {
        public string Stringify(string resource)
        {
            var pathinfo = AllKeys.Aggregate(resource, (current, key) => Regex.Replace(current, $@"\[{key}\]", Get(key)));
            if (pathinfo.Contains("[") || pathinfo.Contains("]")) throw new ClientStateException($"Not all parameters were replaced in request resource: {pathinfo}");
            return pathinfo;
        }
    }
}