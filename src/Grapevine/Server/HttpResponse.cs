using System;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using Grapevine.Util;
using HttpStatusCode = Grapevine.Util.HttpStatusCode;

namespace Grapevine.Server
{
    public interface IHttpResponse
    {
        Encoding ContentEncoding { get; set; }

        long ContentLength64 { get; set; }

        string ContentType { get; set; }

        CookieCollection Cookies { get; set; }

        dynamic Dynamic { get; }

        WebHeaderCollection Headers { get; set; }

        bool KeepAlive { get; set; }

        Stream OutputStream { get; }

        Version ProtocolVersion { get; set; }

        string RedirectLocation { get; set; }

        bool ResponseSent { get; }

        bool SendChunked { get; set; }

        int StatusCode { get; set; }

        string StatusDescription { get; set; }

        void Abort();

        void AddHeader(string name, string value);

        void AppendCookie(Cookie cookie);

        void AppendHeader(string name, string value);

        void Close();

        void Close(byte[] responseEntity, bool willBlock);

        void CopyFrom(HttpResponse templateResponse);

        void Redirect(string Url);

        void SendResponse(string response, bool isFilePath = false);

        void SendResponse(string response, Encoding encoding);

        void SendResponse(FileStream stream, string filepath);

        void SendResponse(FileStream stream, ContentType type, string filename, bool asAttachment);

        void SendResponse(HttpStatusCode statusCode, Exception exception);

        void SendResponse(HttpStatusCode statusCode, string response = null);

        void SetCookie(Cookie cookie);
    }

    public class HttpResponse : DynamicAspect, IHttpResponse
    {
        protected const int HoursToExpire = 23;

        internal readonly HttpListenerResponse Response;
        internal readonly NameValueCollection RequestHeaders;

        internal HttpResponse(HttpListenerResponse response, NameValueCollection requestHeaders)
        {
            Response = response;
            RequestHeaders = requestHeaders;
            ResponseSent = false;
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

        public string ContentType
        {
            get { return Response.ContentType; }
            set { Response.ContentType = value; }
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

        public Stream OutputStream => Response.OutputStream;

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

        public bool ResponseSent { get; protected set; }

        public bool SendChunked
        {
            get { return Response.SendChunked; }
            set { Response.SendChunked = value; }
        }

        public int StatusCode
        {
            get { return Response.StatusCode; }
            set { Response.StatusCode = value; }
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

        public void Close()
        {
            Response.Close();
            ResponseSent = true;
        }

        public void Close(byte[] responseEntity, bool willBlock)
        {
            Response.Close(responseEntity, willBlock);
            ResponseSent = true;
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
                SendResponse(new FileStream(response, FileMode.Open, FileAccess.Read), response);
            }
            else
            {
                SendResponse(response, Encoding.UTF8);
            }
        }

        public void SendResponse(string response, Encoding encoding)
        {
            Response.ContentEncoding = encoding;
            if (string.IsNullOrWhiteSpace(ContentType)) Response.ContentType = Util.ContentType.TEXT.ToValue();
            FlushResponse(encoding.GetBytes(response));
        }

        public void SendResponse(FileStream stream, string filepath)
        {
            var lastModified = File.GetLastWriteTimeUtc(filepath).ToString("R");
            Response.AddHeader("Last-Modified", lastModified);

            if (RequestHeaders.AllKeys.Contains("If-Modified-Since"))
            {
                if (RequestHeaders["If-Modified-Since"].Equals(lastModified))
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                    Close();
                }
            }

            var ext = Path.GetExtension(filepath)?.ToUpper().TrimStart('.');
            var type = ext != null && Enum.IsDefined(typeof(ContentType), ext)
                ? (ContentType)Enum.Parse(typeof(ContentType), ext)
                : Util.ContentType.DEFAULT;

            SendResponse(stream, type, Path.GetFileName(filepath));
        }

        public void SendResponse(FileStream stream, ContentType type, string filename, bool asAttachment = false)
        {
            if (!Response.Headers.AllKeys.Contains("Expires")) Response.AddHeader("Expires", DateTime.Now.AddHours(23).ToString("R"));
            if (asAttachment) Response.AddHeader("Content-Disposition", $"attachment; filename=\"{filename}\"");
            Response.ContentType = type.ToValue();

            var buffer = GetFileBytes(stream, type.IsText());
            FlushResponse(buffer);
        }

        public void SendResponse(HttpStatusCode statusCode, Exception exception)
        {
            SendResponse(statusCode, $"{exception.Message}{Environment.NewLine}<br>{Environment.NewLine}{exception.StackTrace}");
        }

        public void SendResponse(HttpStatusCode statusCode, string response = null)
        {
            var description = statusCode.ToString().ConvertCamelCase();
            var body = (!string.IsNullOrWhiteSpace(response)) ? response : $"<h1>{description}</h1>";
            var buffer = Encoding.UTF8.GetBytes(body);

            StatusDescription = description;
            StatusCode = (int)statusCode;
            ContentType = Util.ContentType.HTML.ToValue();

            FlushResponse(buffer);
        }

        public void SetCookie(Cookie cookie)
        {
            Response.SetCookie(cookie);
        }

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
            Close();
        }

        protected byte[] GetFileBytes(FileStream stream, bool istext)
        {
            byte[] buffer;

            if (istext)
            {
                using (var reader = new StreamReader(stream))
                {
                    buffer = Encoding.UTF8.GetBytes(reader.ReadToEnd());
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
}
