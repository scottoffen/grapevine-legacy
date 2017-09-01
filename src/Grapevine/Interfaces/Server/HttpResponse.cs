using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using Grapevine.Shared;
using HttpStatusCode = Grapevine.Shared.HttpStatusCode;

namespace Grapevine.Interfaces.Server
{
    /// <summary>
    /// Represents a response to a request being handled by an HttpListener object
    /// </summary>
    public interface IHttpResponse
    {
        AdvancedHttpResponse Advanced { get; }

        /// <summary>
        /// Gets or sets the Encoding for this response's OutputStream
        /// </summary>
        Encoding ContentEncoding { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes in the body data included in the response
        /// </summary>
        long ContentLength64 { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the content returned
        /// </summary>
        ContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the collection of cookies returned with the response
        /// </summary>
        CookieCollection Cookies { get; set; }

        /// <summary>
        /// Gets or sets the collection of header name/value pairs returned by the server
        /// </summary>
        WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the server requests a persistent connection
        /// </summary>
        bool KeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the HTTP version used for the response
        /// </summary>
        Version ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets the value of the HTTP Location header in this response
        /// </summary>
        string RedirectLocation { get; set; }

        /// <summary>
        /// Gets a value indicating whether a response has been sent to this request
        /// </summary>
        bool ResponseSent { get; }

        /// <summary>
        /// Gets or sets whether the response uses chunked transfer encoding
        /// </summary>
        bool SendChunked { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code to be returned to the client
        /// </summary>
        HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets a text description of the HTTP status code returned to the client
        /// </summary>
        string StatusDescription { get; set; }

        /// <summary>
        /// Closes the connection to the client without sending a response.
        /// </summary>
        void Abort();

        /// <summary>
        /// Adds the specified header and value to the HTTP headers for this response
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void AddHeader(string name, string value);

        /// <summary>
        /// Adds the specified Cookie to the collection of cookies for this response
        /// </summary>
        /// <param name="cookie"></param>
        void AppendCookie(Cookie cookie);

        /// <summary>
        /// Appends a value to the specified HTTP header to be sent with this response
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void AppendHeader(string name, string value);

        /// <summary>
        /// Copies properties from the specified HttpListenerResponse to this response
        /// </summary>
        /// <param name="templateResponse"></param>
        void CopyFrom(HttpResponse templateResponse);

        /// <summary>
        /// Configures the response to redirect the client to the specified URL
        /// </summary>
        /// <param name="url"></param>
        void Redirect(string url);

        /// <summary>
        /// Write the contents of the buffer to and then closes the OutputStream, followed by closing the Response
        /// </summary>
        /// <param name="contents"></param>
        void SendResponse(byte[] contents);

        /// <summary>
        /// Adds or updates a Cookie in the collection of cookies sent with this response
        /// </summary>
        /// <param name="cookie"></param>
        void SetCookie(Cookie cookie);
    }

    public class HttpResponse : IHttpResponse
    {
        private static readonly string Server;
        public static bool SuppressServerHeader { get; set; }

        static HttpResponse()
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.GetName().Name == "Grapevine");
            if (assembly != null) Server = $"{assembly.GetName().Name}/{assembly.GetName().Version}";
        }

        private ContentType _contentType;
        private bool _parsedContentType;

        /// <summary>
        /// Used to set the umber of hours before sent content expires on the Expires response header
        /// </summary>
        protected int HoursToExpire = 23;

        /// <summary>
        /// The underlying HttpListenerResponse for this instance
        /// </summary>
        protected internal readonly HttpListenerResponse Response;

        /// <summary>
        /// The Request headers corresponding to this response
        /// </summary>
        protected internal readonly NameValueCollection RequestHeaders;

        /// <summary>
        /// Provides direct access to selected methods and properties on the internal HttpListenerResponse instance in use; do not used unless you are fully aware of what you are doing and the consequences involved.
        /// </summary>
        public AdvancedHttpResponse Advanced { get; }

        internal HttpResponse(HttpListenerResponse response, NameValueCollection requestHeaders)
        {
            Response = response;
            Response.ContentEncoding = Encoding.ASCII;
            RequestHeaders = requestHeaders;
            
            ResponseSent = false;
            Advanced = new AdvancedHttpResponse(this);
        }

        public Encoding ContentEncoding
        {
            get { return Response.ContentEncoding; }
            set { Response.ContentEncoding = value; }
        }

        public long ContentLength64
        {
            get { return Response.ContentLength64; }
            set { Response.ContentLength64 = value; }
        }

        public ContentType ContentType
        {
            get
            {
                if (_parsedContentType) return _contentType;
                _contentType = _contentType.FromString(Response.ContentType);
                _parsedContentType = true;
                return _contentType;
            }
            set
            {
                _contentType = value;
                Response.ContentType = _contentType.ToValue();
                _parsedContentType = true;
            }
        }

        public CookieCollection Cookies
        {
            get { return Response.Cookies; }
            set { Response.Cookies = value; }
        }

        public WebHeaderCollection Headers
        {
            get { return Response.Headers; }
            set { Response.Headers = value; }
        }

        public bool KeepAlive
        {
            get { return Response.KeepAlive; }
            set { Response.KeepAlive = value; }
        }

        public Version ProtocolVersion
        {
            get { return Response.ProtocolVersion; }
            set { Response.ProtocolVersion = value; }
        }

        public string RedirectLocation
        {
            get { return Response.RedirectLocation; }
            set { Response.RedirectLocation = value; }
        }

        public bool ResponseSent { get; protected internal set; }

        public bool SendChunked
        {
            get { return Response.SendChunked; }
            set { Response.SendChunked = value; }
        }

        public HttpStatusCode StatusCode
        {
            get { return (HttpStatusCode)Response.StatusCode; }
            set
            {
                Response.StatusCode = (int)value;
                StatusDescription = value.ConvertToString();
            }
        }

        public string StatusDescription
        {
            get { return Response.StatusDescription; }
            set { Response.StatusDescription = value; }
        }

        public void Abort()
        {
            Response.Abort();
        }

        public void AddHeader(string name, string value)
        {
            Response.AddHeader(name, value);
        }

        public void AppendCookie(Cookie cookie)
        {
            Response.AppendCookie(cookie);
        }

        public void AppendHeader(string name, string value)
        {
            Response.AppendHeader(name, value);
        }

        public void CopyFrom(HttpResponse templateResponse)
        {
            Response.CopyFrom(templateResponse.Response);
        }

        public void Redirect(string url)
        {
            Response.Redirect(url);
        }

        public void SetCookie(Cookie cookie)
        {
            Response.SetCookie(cookie);
        }

        public void SendResponse(byte[] contents)
        {
            if (Server != null && !SuppressServerHeader) Headers["Server"] = Server;

            if (RequestHeaders.AllKeys.Contains("Accept-Encoding") && RequestHeaders["Accept-Encoding"].Contains("gzip") && contents.Length > 1024)
            {
                using (var ms = new MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionMode.Compress))
                    {
                        zip.Write(contents, 0, contents.Length);
                    }
                    contents = ms.ToArray();
                }
                Response.Headers["Content-Encoding"] = "gzip";
            }

            try
            {
                Response.ContentLength64 = contents.Length;
                Response.OutputStream.Write(contents, 0, contents.Length);
                Response.OutputStream.Close();
            }
            catch
            {
                Response.OutputStream.Dispose();
                throw;
            }
            finally
            {
                Advanced.Close();
            }
        }
    }

    /// <summary>
    /// Provides direct access to selected methods and properties on the internal HttpListenerResponse instance. This class cannot be inherited.
    /// </summary>
    public class AdvancedHttpResponse
    {
        protected readonly HttpResponse Response;

        protected internal AdvancedHttpResponse(HttpResponse response)
        {
            Response = response;
        }

        public int DefaultHoursToExpire { get; set; } = 23;

        public string ContentType
        {
            get { return Response.Response.ContentType; }
            set { Response.Response.ContentType = value; }
        }

        /// <summary>
        /// Gets a Stream object to which a response can be written
        /// </summary>
        public Stream OutputStream => Response.Response.OutputStream;

        /// <summary>
        /// Sends the response to the client and releases the resources held by this HttpListenerResponse instance
        /// </summary>
        public void Close()
        {
            Response.Response.Close();
            Response.ResponseSent = true;
        }

        /// <summary>
        /// Returns the specified byte array to the client and releases the resources held by this HttpListenerResponse instance
        /// </summary>
        /// <param name="responseEntity"></param>
        /// <param name="willBlock"></param>
        public void Close(byte[] responseEntity, bool willBlock)
        {
            Response.Response.Close(responseEntity, willBlock);
            Response.ResponseSent = true;
        }
    }
}
