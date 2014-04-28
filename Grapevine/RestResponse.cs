using System;
using System.IO;
using System.Net;

namespace Grapevine
{
    public class RestResponse
    {
        public RestResponse(HttpWebResponse response, long elapsedTime) : this(response, elapsedTime, "", "") { }

        public RestResponse(HttpWebResponse response, long elapsedTime, string error, string errorStatus)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            this.Content = reader.ReadToEnd();

            this.ContentEncoding = response.ContentEncoding;
            this.ContentLength = response.ContentLength;
            this.ContentType = response.ContentType;
            this.Cookies = response.Cookies;
            this.ElapsedTime = elapsedTime;
            this.Error = error;
            this.ErrorStatus = errorStatus;
            this.Headers = response.Headers;
            this.ResponseStatus = response.StatusCode.ToString();
            this.ResponseUri = response.ResponseUri;
            this.ReturnedError = (this.Error.Length > 0) ? true : false;
            this.Server = response.Server;
            this.StatusCode = response.StatusCode;
            this.StatusDescription = response.StatusDescription;
        }

        #region Public Properties

        public string Content { get; private set; }

        public string ContentEncoding { get; private set; }

        public long ContentLength { get; private set; }

        public string ContentType { get; private set; }

        public CookieCollection Cookies { get; private set; }

        public long ElapsedTime { get; private set; }

        public string Error { get; private set; }

        public string ErrorStatus { get; private set; }

        public WebHeaderCollection Headers { get; private set; }

        public string ResponseStatus { get; private set; }
        
        public Uri ResponseUri { get; private set; }

        public bool ReturnedError { get; private set; }

        public string Server { get; private set; }

        public HttpStatusCode StatusCode { get; private set; }

        public string StatusDescription { get; private set; }

        #endregion
    }
}
