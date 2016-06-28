using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace Grapevine.Client
{
    /// <summary>
    /// Returned by RESTClient.Execute()
    /// </summary>
    public class RESTResponse
    {
        #region Constructor

        internal RESTResponse(HttpWebResponse response, long elapsedTime = 0, string error = null, WebExceptionStatus errorStatus = WebExceptionStatus.Success)
        {
            if (!object.ReferenceEquals(response, null))
            {
                this.Content = this.GetContent(response);
                this.ContentEncoding = response.ContentEncoding;
                this.ContentLength = response.ContentLength;
                this.ContentType = response.ContentType;
                this.Cookies = response.Cookies;
                this.Headers = response.Headers;
                this.ResponseStatus = response.StatusCode.ToString();
                this.ResponseUri = response.ResponseUri;
                this.ReturnedError = (this.ErrorStatus.Equals(WebExceptionStatus.Success)) ? false : true;
                this.Server = response.Server;
                this.StatusCode = response.StatusCode;
                this.StatusDescription = response.StatusDescription;
            }

            this.ElapsedTime = elapsedTime;
            this.Error = error;
            this.ErrorStatus = errorStatus;
        }

        private string GetContent(HttpWebResponse response)
        {
            var stream = response.GetResponseStream();
            if (response.Headers.AllKeys.Contains("Content-Encoding") && response.Headers["Content-Encoding"].Contains("gzip"))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }

            StreamReader reader = new StreamReader(stream);
            var content = reader.ReadToEnd();

            stream.Close();
            reader.Close();

            return content;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The body of the response
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// The method used to ecode the body of the response
        /// </summary>
        public string ContentEncoding { get; private set; }

        /// <summary>
        /// The length of the body of the response
        /// </summary>
        public long ContentLength { get; private set; }

        /// <summary>
        /// The content type of the response
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// The cookies associated with this response
        /// </summary>
        public CookieCollection Cookies { get; private set; }

        /// <summary>
        /// The time in milliseconds between sending the request receiving the response
        /// </summary>
        public long ElapsedTime { get; private set; }

        /// <summary>
        /// The error message if an error occured when executing the request
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// The WebExecptionStatus of the request, returns WebExceptionStatus.Success if there was no error
        /// </summary>
        public WebExceptionStatus ErrorStatus { get; private set; }

        /// <summary>
        /// The WebHeaderCollection for this response
        /// </summary>
        public WebHeaderCollection Headers { get; private set; }

        /// <summary>
        /// The response status
        /// </summary>
        public string ResponseStatus { get; private set; }

        /// <summary>
        /// The URI of the internet resource that responded to the request
        /// </summary>
        public Uri ResponseUri { get; private set; }

        /// <summary>
        /// Returns true if an error occurred in the execution of the request
        /// </summary>
        public bool ReturnedError { get; private set; }

        /// <summary>
        /// The name of the server that sent the response
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// The HttpStatusCode returned with the response
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// The status description returned with the response
        /// </summary>
        public string StatusDescription { get; private set; }

        #endregion
    }
}
