using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Grapevine.Util;

namespace Grapevine.Client
{
    //Decorates HttpWebRequest?

    /// <summary>
    /// Contains all data required to send a request to a RESTClient
    /// </summary>
    public interface IRestRequest
    {
        /// <summary>
        /// Get or set the ContentType
        /// </summary>
        ContentType ContentType { get; set; }

        /// <summary>
        /// Get or set the Encoding
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Get and interact with the WebHeaderCollection
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// Get or set the HttpMethod
        /// </summary>
        HttpMethod Method { get; set; }

        /// <summary>
        /// Gets the parsed PathInfo
        /// </summary>
        string PathInfo { get; }

        /// <summary>
        /// Get or set the data payload
        /// </summary>
        string Payload { get; set; }

        /// <summary>
        /// Gets the parsed QueryString
        /// </summary>
        string QueryString { get; }

        /// <summary>
        /// Get or set the optionally-tokenized string that represents the path to the desired resource; when changed, all parameters are cleared
        /// </summary>
        string Resource { get; set; }

        /// <summary>
        /// Get or set the request wait time in milliseconds
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Provide a name and value for tokens included in the Resource string
        /// </summary>
        void AddParameter(string name, string value);

        /// <summary>
        /// Provide a name and value for parameters to be sent in the query string of the request
        /// </summary>
        void AddQuery(string name, string value);

        /// <summary>
        /// Clears the payload, parameters and query string of the request; automatically called when the request is passed as an argument to RESTClient.Execute
        /// </summary>
        void Reset();
    }

    public class RestRequest : IRestRequest
    {
        public static int GlobalTimeout = 2000;

        public ContentType ContentType { get; set; }
        public Encoding Encoding { get; set; }
        public WebHeaderCollection Headers { get; }
        public HttpMethod Method { get; set; }
        public string Payload { get; set; }
        public int Timeout { get; set; }

        protected NameValueCollection ParametersCollection;
        protected NameValueCollection QueryStringCollection;

        private string _resource;

        public RestRequest()
        {
            ParametersCollection = new NameValueCollection();
            QueryStringCollection = new NameValueCollection();

            Resource = string.Empty;
            Method = HttpMethod.GET;
            ContentType = ContentType.TXT;
            Timeout = GlobalTimeout;
            Encoding = Encoding.UTF8;

            Headers = new WebHeaderCollection
            {
                {HttpRequestHeader.CacheControl, "no-store, must-revalidate"},
                {HttpRequestHeader.AcceptEncoding, "gzip"}
            };
        }

        public string PathInfo
        {
            get
            {
                var pathinfo = Resource;

                foreach (var key in ParametersCollection.AllKeys)
                {
                    var value = ParametersCollection.Get(key);
                    pathinfo = Regex.Replace(pathinfo, "{" + key + "}", value);
                }

                if (!pathinfo.Matches("{") && !pathinfo.Matches("}")) return pathinfo;
                throw new ClientStateException($"Not all parameters were replaced in requested resource: {pathinfo}");
            }
        }

        public string QueryString => QueryStringCollection.Count <= 0 ? null : string.Join("&", (from key in QueryStringCollection.AllKeys let value = QueryStringCollection.Get(key) select HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value)).ToArray());

        public string Resource
        {
            get
            {
                return _resource;
            }
            set
            {
                value = Regex.Replace(value, "^/", "");
                if (value.Equals(_resource)) return;
                _resource = value;
                ParametersCollection.Clear();
            }
        }

        public void AddParameter(string name, string value)
        {
            ParametersCollection.Add(name, value);
        }

        public void AddQuery(string name, string value)
        {
            QueryStringCollection.Add(name, value);
        }

        public void Reset()
        {
            Payload = null;
            ParametersCollection.Clear();
            QueryStringCollection.Clear();
        }
    }
}
