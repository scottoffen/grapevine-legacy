using System;
using System.Net;
using System.IO;

namespace Grapevine
{
    public class RestResponse
    {
        protected HttpWebResponse _response;
        protected string _content;

        public RestResponse(HttpWebResponse response, long elapsedTime)
        {
            this._response = response;
            this.ElapsedTime = elapsedTime;
        }

        public string Content
        {
            get
            {
                if (Object.ReferenceEquals(_content, null))
                {
                    StreamReader reader = new StreamReader(this._response.GetResponseStream());
                    this._content = reader.ReadToEnd();
                }

                return this._content;
            }
        }

        public string ContentEncoding
        {
            get
            {
                return this._response.ContentEncoding;
            }
        }

        public long ContentLength
        {
            get
            {
                return this._response.ContentLength;
            }
        }

        public string ContentType
        {
            get
            {
                return this._response.ContentType;
            }
        }

        public CookieCollection Cookies
        {
            get
            {
                return this._response.Cookies;
            }
        }

        public long ElapsedTime { get; private set; }

        public WebHeaderCollection Headers
        {
            get
            {
                return this._response.Headers;
            }
        }

        public string ResponseStatus
        {
            get
            {
                return this._response.StatusCode.ToString();
            }
        }
        
        public Uri ResponseUri
        {
	        get
	        {
		        return this._response.ResponseUri;
	        }
        }

        public string Server
        {
	        get
	        {
                return this._response.Server;
	        }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return this._response.StatusCode;
            }
        }

        public string StatusDescription
        {
            get
            {
                return this._response.StatusDescription;
            }
        }
    }
}
