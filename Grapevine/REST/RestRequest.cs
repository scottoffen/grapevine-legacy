using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Grapevine.REST
{
    public class RestRequest
    {
        protected NameValueCollection _parameters;
        private string _resource;

        public RestRequest() : this(HttpMethod.GET, "", RequestContentType.DEFAULT) { }

        public RestRequest(HttpMethod method) : this(method, "", RequestContentType.DEFAULT) { }

        public RestRequest(string resource) : this(HttpMethod.GET, resource, RequestContentType.DEFAULT) { }

        public RestRequest(string resource, HttpMethod method) : this(method, resource, RequestContentType.DEFAULT) { }

        public RestRequest(HttpMethod method, string resource, RequestContentType type)
        {
            this._parameters  = new NameValueCollection();
            this.Method      = method;
            this.Resource    = resource;
            this.Timeout     = 500;
            this.ContentType = type.Value();
        }

        public void SetContentType(RequestContentType type)
        {
            this.ContentType = type.Value();
        }

        public HttpMethod Method { get; set; }

        public string Resource
        {
            get
            {
                return this._resource;
            }
            set
            {
                this._resource = Regex.Replace(value, "^/", "");
            }
        }

        public int Timeout { get; set; }

        public string Payload { get; set; }

        public string ContentType { get; set; }

        public void AddParameter(string name, string value)
        {
            _parameters.Add(name, value);
        }

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
                    throw new InvalidStateException("Not all parameters were replaced in your request resource");
                }
                return pathinfo;
            }
        }

        public void Reset()
        {
            this.Payload = null;
            this._parameters.Clear();
        }
    }
}
