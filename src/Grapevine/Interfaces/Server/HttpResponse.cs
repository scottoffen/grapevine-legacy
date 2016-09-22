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
        /// Sends the specified response to the client and closes the response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isFilePath"></param>
        void SendResponse(string response, bool isFilePath = false);

        /// <summary>
        /// Sends the specified string response to the client using the specified encoding and closes the response
        /// </summary>
        /// <param name="response"></param>
        /// <param name="encoding"></param>
        void SendResponse(string response, Encoding encoding);

        /// <summary>
        /// Sends the specified binary response with the specifed content type to the client and closes the response; sets the attachment header on the response with the provided filename if asAttachment is true
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="type"></param>
        /// <param name="filename"></param>
        /// <param name="asAttachment"></param>
        void SendResponse(FileStream stream, ContentType type, string filename, bool asAttachment = false);

        /// <summary>
        /// Sends the specified status code and exception as a response to the client and closes the response
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="exception"></param>
        void SendResponse(HttpStatusCode statusCode, Exception exception);

        /// <summary>
        /// Sends the specified response to the client with the given response and closes the response
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="response"></param>
        void SendResponse(HttpStatusCode statusCode, string response = null);

        /// <summary>
        /// Adds or updates a Cookie in the collection of cookies sent with this response
        /// </summary>
        /// <param name="cookie"></param>
        void SetCookie(Cookie cookie);
    }

    public class HttpResponse : IHttpResponse
    {
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
            get { return Enum.GetValues(typeof (ContentType)).Cast<ContentType>().First(t => t.ToValue().Equals(Response.ContentType)); }
            set { Response.ContentType = value.ToValue(); }
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

        public void SendResponse(string response, bool isFilePath = false)
        {
            if (isFilePath)
            {
                if (NotModified(response)) return;
                var type = GetFileType(response);
                SendResponse(new FileStream(response, FileMode.Open, FileAccess.Read), type, response);
            }
            else
            {
                SendResponse(response, Response.ContentEncoding);
            }
        }

        public void SendResponse(string response, Encoding encoding)
        {
            Response.ContentEncoding = encoding;
            FlushResponse(encoding.GetBytes(response));
        }

        public void SendResponse(FileStream stream, ContentType type, string filename, bool asAttachment = false)
        {
            ContentType = type;
            if (!Response.Headers.AllKeys.Contains("Expires")) Response.AddHeader("Expires", DateTime.Now.AddHours(HoursToExpire).ToString("R"));
            if (asAttachment) Response.AddHeader("Content-Disposition", $"attachment; filename=\"{filename}\"");

            var buffer = GetFileBytes(stream, type.IsText());
            FlushResponse(buffer);
        }

        public void SendResponse(HttpStatusCode statusCode, Exception exception)
        {
            SendResponse(statusCode, $"{exception.Message}{Environment.NewLine}<br>{Environment.NewLine}{exception.StackTrace}");
        }

        public void SendResponse(HttpStatusCode statusCode, string response = null)
        {
            StatusDescription = statusCode.ToString().ConvertCamelCase();
            StatusCode = statusCode;
            byte[] buffer;

            if (string.IsNullOrWhiteSpace(response))
            {
                ContentType = ContentType.HTML;
                buffer = Encoding.ASCII.GetBytes($"<h1>{StatusDescription}</h1>");
            }
            else
            {
                buffer = ContentEncoding.GetBytes(response);
            }

            FlushResponse(buffer);
        }

        public void SetCookie(Cookie cookie)
        {
            Response.SetCookie(cookie);
        }

        protected bool NotModified(string filepath)
        {
            var lastModified = File.GetLastWriteTimeUtc(filepath).ToString("R");
            Response.AddHeader("Last-Modified", lastModified);

            if (!RequestHeaders.AllKeys.Contains("If-Modified-Since")) return false;
            if (!RequestHeaders["If-Modified-Since"].Equals(lastModified)) return false;

            SendResponse(HttpStatusCode.NotModified);
            return true;
        }

        protected ContentType GetFileType(string filepath)
        {
            var ext = Path.GetExtension(filepath)?.ToUpper().TrimStart('.');
            return !string.IsNullOrWhiteSpace(ext) && Enum.IsDefined(typeof(ContentType), ext)
                ? (ContentType)Enum.Parse(typeof(ContentType), ext)
                : ContentType.DEFAULT;
        }

        /// <summary>
        /// Write the content to and closes the OutputStream, then closes the response
        /// </summary>
        /// <param name="buffer"></param>
        protected void FlushResponse(byte[] buffer)
        {
            if (RequestHeaders.AllKeys.Contains("Accept-Encoding") && RequestHeaders["Accept-Encoding"].Contains("gzip") && buffer.Length > 1024)
            {
                using (var ms = new MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionMode.Compress))
                    {
                        zip.Write(buffer, 0, buffer.Length);
                    }
                    buffer = ms.ToArray();
                }
                Response.Headers["Content-Encoding"] = "gzip";
            }

            Response.ContentLength64 = buffer.Length;
            Response.OutputStream.Write(buffer, 0, buffer.Length);
            Response.OutputStream.Close();
            Advanced.Close();
        }

        /// <summary>
        /// Returns a byte array representation of the data in the file referenced by the stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="istext"></param>
        /// <returns></returns>
        protected byte[] GetFileBytes(FileStream stream, bool istext)
        {
            byte[] buffer;

            if (istext)
            {
                using (var reader = new StreamReader(stream))
                {
                    buffer = ContentEncoding.GetBytes(reader.ReadToEnd());
                }
            }
            else
            {
                using (var reader = new BinaryReader(stream))
                {
                    buffer = reader.ReadBytes((int)stream.Length);
                }
            }

            return buffer;
        }
    }

    /// <summary>
    /// Provides direct access to selected methods and properties on the internal HttpListenerResponse instance. This class cannot be inherited.
    /// </summary>
    public sealed class AdvancedHttpResponse
    {
        private readonly HttpResponse _response;

        internal AdvancedHttpResponse(HttpResponse response)
        {
            _response = response;
        }

        /// <summary>
        /// Gets a Stream object to which a response can be written
        /// </summary>
        public Stream OutputStream => _response.Response.OutputStream;

        /// <summary>
        /// Sends the response to the client and releases the resources held by this HttpListenerResponse instance
        /// </summary>
        public void Close()
        {
            _response.Response.Close();
            _response.ResponseSent = true;
        }

        /// <summary>
        /// Returns the specified byte array to the client and releases the resources held by this HttpListenerResponse instance
        /// </summary>
        /// <param name="responseEntity"></param>
        /// <param name="willBlock"></param>
        public void Close(byte[] responseEntity, bool willBlock)
        {
            _response.Response.Close(responseEntity, willBlock);
            _response.ResponseSent = true;
        }
    }
}
