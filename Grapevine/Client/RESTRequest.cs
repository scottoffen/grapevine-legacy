using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Grapevine.Client
{
    /// <summary>
    /// Contains all data required to send a request to a RESTClient
    /// </summary>
    public class RESTRequest
    {
        public static int GlobalTimeout = 2000;

        protected NameValueCollection _parameters;
        protected NameValueCollection _querystring;
        private string _resource;

        #region Constructors

        public RESTRequest(string resource = "", HttpMethod method = HttpMethod.GET, ContentType type = ContentType.TXT, int timeout = -1, Encoding encoding = null)
        {
            this._parameters = new NameValueCollection();
            this._querystring = new NameValueCollection();
            this.Headers = new WebHeaderCollection();

            this.Method = method;
            this.Resource = resource;
            this.ContentType = type;
            this.Timeout = (timeout > -1) ? timeout : RESTRequest.GlobalTimeout;
            this.Encoding = (!object.ReferenceEquals(encoding, null)) ? encoding : Encoding.UTF8;

            this.Headers.Add(HttpRequestHeader.CacheControl, "no-store, must-revalidate");
            this.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Get or set the ContentType
        /// </summary>
        public ContentType ContentType { get; set; }

        /// <summary>
        /// Get or set the Encoding
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Get and interact with the WebHeaderCollection
        /// </summary>
        public WebHeaderCollection Headers { get; private set; }

        /// <summary>
        /// Get or set the HttpMethod
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Gets the parsed PathInfo
        /// </summary>
        public string PathInfo
        {
            get
            {
                string pathinfo = this.Resource;

                foreach (var key in this._parameters.AllKeys)
                {
                    var value = this._parameters.Get(key);
                    pathinfo = Regex.Replace(pathinfo, "{" + key + "}", value);
                }

                if (pathinfo.Matches("{") || pathinfo.Matches("}"))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("Not all parameters were replaced in your request resource:");
                    sb.AppendLine("");
                    sb.AppendLine(pathinfo);

                    EventLogger.Log(sb.ToString());
                    throw new ClientStateException("Not all parameters were replaced in your request resource");
                }

                return pathinfo;
            }
        }

        /// <summary>
        /// Get or set the data payload
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        /// Gets the parsed QueryString
        /// </summary>
        public string QueryString
        {
            get
            {
                if (this._querystring.Count > 0)
                {
                    List<string> querystring = new List<string>();

                    foreach (var key in this._querystring.AllKeys)
                    {
                        var value = this._querystring.Get(key);
                        querystring.Add(HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(value));
                    }

                    return string.Join("&", querystring.ToArray());
                }

                return null;
            }
        }

        /// <summary>
        /// Get or set the optionally-tokenized string that represents the path to the desired resource; when changed, all parameters are cleared
        /// </summary>
        public string Resource
        {
            get
            {
                return this._resource;
            }
            set
            {
                value = Regex.Replace(value, "^/", "");
                if (!value.Equals(this._resource))
                {
                    this._resource = value;
                    this._parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Get or set the request wait time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Provide a name and value for tokens included in the Resource string
        /// </summary>
        public void AddParameter(string name, string value)
        {
            this._parameters.Add(name, value);
        }

        /// <summary>
        /// Provide a name and value for parameters to be sent in the query string of the request
        /// </summary>
        public void AddQuery(string name, string value)
        {
            this._querystring.Add(name, value);
        }

        /// <summary>
        /// Clears the payload, parameters and query string of the request; automatically called when the request is passed as an argument to RESTClient.Execute
        /// </summary>
        public void Reset()
        {
            this.Payload = null;
            this._parameters.Clear();
            this._querystring.Clear();
        }

        #endregion
    }
}
