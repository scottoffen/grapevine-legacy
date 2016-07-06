using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace Grapevine.Client
{
    //Decorates HttpWebResponse

    /// <summary>
    /// Returned by RESTClient.Execute()
    /// </summary>
    public interface IRestResponse
    {
        /// <summary>
        /// The body of the response
        /// </summary>
        string Content { get; }

        /// <summary>
        /// The method used to ecode the body of the response
        /// </summary>
        string ContentEncoding { get; }

        /// <summary>
        /// The length of the body of the response
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// The content type of the response
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// The cookies associated with this response
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// The time in milliseconds between sending the request receiving the response
        /// </summary>
        long ElapsedTime { get; }

        /// <summary>
        /// The error message if an error occured when executing the request
        /// </summary>
        string Error { get; }

        /// <summary>
        /// The WebExecptionStatus of the request, returns WebExceptionStatus.Success if there was no error
        /// </summary>
        WebExceptionStatus ErrorStatus { get; }

        /// <summary>
        /// The WebHeaderCollection for this response
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// The response status
        /// </summary>
        string ResponseStatus { get; }

        /// <summary>
        /// The URI of the internet resource that responded to the request
        /// </summary>
        Uri ResponseUri { get; }

        /// <summary>
        /// Returns true if an error occurred in the execution of the request
        /// </summary>
        bool ReturnedError { get; }

        /// <summary>
        /// The name of the server that sent the response
        /// </summary>
        string Server { get; }

        /// <summary>
        /// The HttpStatusCode returned with the response
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// The status description returned with the response
        /// </summary>
        string StatusDescription { get; }
    }

    public class RestResponse : IRestResponse
    {
        private readonly HttpWebResponse _response;

        public string CharacterSet => _response.CharacterSet;
        public string Content { get; }
        public string ContentEncoding => _response.ContentEncoding;
        public long ContentLength => _response.ContentLength;
        public string ContentType => _response.ContentType;
        public CookieCollection Cookies => _response.Cookies;
        public long ElapsedTime { get; }
        public string Error { get; }
        public WebExceptionStatus ErrorStatus { get; }
        public WebHeaderCollection Headers { get; }
        public string ResponseStatus { get; }
        public Uri ResponseUri { get; }
        public bool ReturnedError { get; }
        public string Server { get; }
        public HttpStatusCode StatusCode { get; }
        public string StatusDescription { get; }

        internal RestResponse(HttpWebResponse response, long elapsedTime = 0, string error = null, WebExceptionStatus errorStatus = WebExceptionStatus.Success)
        {
            _response = response;
            if (quals(response, null))
            {
                this.Content = GetContent(response);
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

        private static string GetContent(HttpWebResponse response)
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
    }
}
