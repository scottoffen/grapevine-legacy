using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using Grapevine.Shared;
using HttpStatusCode = Grapevine.Shared.HttpStatusCode;

namespace Grapevine.Client
{
    /// <summary>
    /// Provides a custom wrapper to HttpWebResponse, an HTTP-specific implementation of the WebResponse class
    /// </summary>
    public interface IRestResponse
    {
        /// <summary>
        /// Gets the character set of the response
        /// </summary>
        string CharacterSet { get; }

        /// <summary>
        /// Gets the method that is used to encode the body of the response
        /// </summary>
        string ContentEncoding { get; }

        /// <summary>
        /// Gets the length of the content returned by the request
        /// </summary>
        long ContentLength { get; }

        /// <summary>
        /// Gets the content type of the response
        /// </summary>
        ContentType ContentType { get; }

        /// <summary>
        /// Gets or sets the cookies that are associated with this response
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// Gets a value that represents the amount of time in milliseconds it took to receive this response
        /// </summary>
        long ElapsedTime { get; }

        /// <summary>
        /// The error message (if any) error occured when executing the request
        /// </summary>
        string Error { get; }

        /// <summary>
        /// The WebExceptionStatus of the request, returns WebExceptionStatus.Success if there was no error
        /// </summary>
        WebExceptionStatus ErrorStatus { get; }

        /// <summary>
        /// Gets the headers that are associated with this response from the server
        /// </summary>
        WebHeaderCollection Headers { get; }

        /// <summary>
        /// Gets a value that indicates whether this response was obtained from the cache
        /// </summary>
        bool IsFromCache { get; }

        /// <summary>
        /// Gets a value that indicates whether both client and server were authenticated
        /// </summary>
        bool IsMutuallyAuthenticated { get; }

        /// <summary>
        /// Gets the last date and time that the contents of the response were modified
        /// </summary>
        DateTime LastModified { get; }

        /// <summary>
        /// Gets the method that is used to return the response
        /// </summary>
        HttpMethod HttpMethod { get; }

        /// <summary>
        /// Gets the version of the HTTP protocol that is used in the response
        /// </summary>
        Version ProtocolVersion { get; }

        /// <summary>
        /// Gets the URI of the Internet resource that responded to the request
        /// </summary>
        Uri ResponseUri { get; }

        /// <summary>
        /// Gets a value indicating whether an error occurred during the execution of the request
        /// </summary>
        bool ReturnedError { get; }

        /// <summary>
        /// Gets the name of the server that sent the response
        /// </summary>
        string Server { get; }

        /// <summary>
        /// Gets the status of the response
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the status description returned with the response
        /// </summary>
        string StatusDescription { get; }

        /// <summary>
        /// Returns a string representation of the data in the response stream and closes the response stream
        /// </summary>
        /// <returns>string</returns>
        string GetContent();

        /// <summary>
        /// Gets the contents of a header that was returned with the response
        /// </summary>
        /// <returns>string</returns>
        string GetResponseHeader(string headerName);
    }

    public class RestResponse : IRestResponse
    {
        private readonly HttpWebResponse _response;
        private string _content;
        private ContentType _contentType;
        private bool _parsedContentType;

        public string CharacterSet => _response.CharacterSet;
        public string ContentEncoding => _response.ContentEncoding;
        public long ContentLength => _response.ContentLength;
        public CookieCollection Cookies => _response.Cookies;
        public long ElapsedTime { get; protected internal set; }
        public string Error { get; protected internal set; }
        public WebExceptionStatus ErrorStatus { get; protected internal set; }
        public WebHeaderCollection Headers => _response.Headers;
        public HttpMethod HttpMethod { get; }
        public bool IsFromCache => _response.IsFromCache;
        public bool IsMutuallyAuthenticated => _response.IsMutuallyAuthenticated;
        public DateTime LastModified => _response.LastModified;
        public Version ProtocolVersion => _response.ProtocolVersion;
        public Uri ResponseUri => _response.ResponseUri;
        public bool ReturnedError { get; protected internal set; }
        public string Server => _response.Server;
        public HttpStatusCode StatusCode { get; }
        public string StatusDescription => _response.StatusDescription;

        /// <summary>
        /// Provides direct access to selected methods and properties on the internal HttpWebResponse instance in use; do not used unless you are fully aware of what you are doing and the consequences involved.
        /// </summary>
        public AdvancedRestResponse Advanced { get; }

        internal RestResponse(HttpWebResponse response)
        {
            _response = response;
            Advanced = new AdvancedRestResponse(_response);
            Error = string.Empty;
            ErrorStatus = WebExceptionStatus.Success;
            HttpMethod = HttpMethod.ALL.FromString(_response.Method);
            StatusCode = (HttpStatusCode) (int) _response.StatusCode;
        }

        public ContentType ContentType
        {
            get
            {
                if (_parsedContentType) return _contentType;
                _contentType = _contentType.FromString(_response.ContentType);
                _parsedContentType = true;
                return _contentType;
            }
        }

        public string GetContent()
        {
            if (_content != null) return _content;

            var stream = Advanced.GetResponseStream();
            if (Headers.AllKeys.Contains("Content-Encoding") && Headers["Content-Encoding"].Contains("gzip"))
            {
                stream = new GZipStream(stream, CompressionMode.Decompress);
            }

            var reader = new StreamReader(stream);
            _content = reader.ReadToEnd();
            reader.Close();
            stream.Close();

            return _content;
        }

        public string GetResponseHeader(string headerName)
        {
            return _response.GetResponseHeader(headerName);
        }
    }

    /// <summary>
    /// Provides direct access to selected methods and properties on the internal HttpWebResponse instance. This class cannot be inherited.
    /// </summary>
    public sealed class AdvancedRestResponse
    {
        private readonly HttpWebResponse _response;

        internal AdvancedRestResponse(HttpWebResponse response)
        {
            _response = response;
        }

        /// <summary>
        /// Closes the response stream
        /// </summary>
        public void Close()
        {
            _response.Close();
        }

        /// <summary>
        /// Gets the stream that is used to read the body of the response from the server
        /// </summary>
        /// <returns>Stream</returns>
        public Stream GetResponseStream()
        {
            return _response.GetResponseStream();
        }
    }
}
