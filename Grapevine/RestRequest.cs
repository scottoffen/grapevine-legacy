using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Net;

namespace Grapevine
{
    public class RestRequest
    {
        protected NameValueCollection _parameters;
        protected NameValueCollection _querystring;
        private string _resource;

        #region Constructors

        public RestRequest() : this(HttpMethod.GET, "", ContentType.DEFAULT) { }

        public RestRequest(HttpMethod method) : this(method, "", ContentType.DEFAULT) { }

        public RestRequest(string resource) : this(HttpMethod.GET, resource, ContentType.DEFAULT) { }

        public RestRequest(ContentType type) : this(HttpMethod.GET, "", type) { }

        public RestRequest(string resource, HttpMethod method) : this(method, resource, ContentType.DEFAULT) { }

        public RestRequest(HttpMethod method, string resource) : this(method, resource, ContentType.DEFAULT) { }

        public RestRequest(string resource, ContentType type) : this(HttpMethod.GET, resource, type) { }

        public RestRequest(ContentType type, string resource) : this(HttpMethod.GET, resource, type) { }

        public RestRequest(ContentType type, HttpMethod method) : this(method, "", type) { }

        public RestRequest(HttpMethod method, ContentType type) : this(method, "", type) { }

        public RestRequest(ContentType type, HttpMethod method, string resource) : this(method, resource, type) { }

        public RestRequest(ContentType type, string resource, HttpMethod method) : this(method, resource, type) { }

        public RestRequest(HttpMethod method, ContentType type, string resource) : this(method, resource, type) { }

        public RestRequest(string resource, HttpMethod method, ContentType type) : this(method, resource, type) { }

        public RestRequest(string resource, ContentType type, HttpMethod method) : this(method, resource, type) { }

        public RestRequest(HttpMethod method, string resource, ContentType type)
        {
            this._parameters  = new NameValueCollection();
            this._querystring = new NameValueCollection();
            this.Headers      = new WebHeaderCollection();
            this.Method       = method;
            this.Resource     = resource;
            this.ContentType  = type;
            this.Timeout      = 500;
            this.Encoding     = Encoding.UTF8;
        }

        #endregion

        #region Public Properties

        public ContentType ContentType { get; set; }

        public Encoding Encoding { get; set; }

        public WebHeaderCollection Headers { get; private set; }

        public HttpMethod Method { get; set; }

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
                    throw new ServerStateException("Not all parameters were replaced in your request resource");
                }

                return pathinfo;
            }
        }

        public string Payload { get; set; }

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
                    this._resource = Regex.Replace(value, "^/", "");
                    this._parameters.Clear();
                }
            }
        }

        public int Timeout { get; set; }

        #endregion

        #region Public Methods

        public void AddParameter(string name, string value)
        {
            this._parameters.Add(name, value);
        }

        public void AddQuery(string name, string value)
        {
            this._querystring.Add(name, value);
        }

        public void Reset()
        {
            this.Payload = null;
            this._parameters.Clear();
            this._querystring.Clear();
        }

        #endregion
    }
}
