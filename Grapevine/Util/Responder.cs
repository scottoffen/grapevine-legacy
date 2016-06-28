using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace Grapevine
{
    /// <summary>
    /// Provides consistent response handling helper methods.
    /// </summary>
    public abstract class Responder
    {
        /// <summary>
        /// Returns a string representation of the request body
        /// </summary>
        protected string GetPayload(HttpListenerRequest request)
        {
            try
            {
                string data;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    data = reader.ReadToEnd();
                }
                return data;
            }
            catch (Exception e)
            {
                EventLogger.Log(e);
            }

            return null;
        }

        /// <summary>
        /// Default response method to an Internal Server Error
        /// </summary>
        protected virtual void InternalServerError(HttpListenerContext context, Exception e)
        {
            this.InternalServerError(context, EventLogger.ExceptionToString(e), ContentType.HTML);
        }

        /// <summary>
        /// Default response method to an Internal Server Error
        /// </summary>
        protected virtual void InternalServerError(HttpListenerContext context, string payload = "<h1>Internal Server Error</h1>", ContentType contentType = ContentType.HTML)
        {
            var buffer = Encoding.UTF8.GetBytes(payload);
            var length = buffer.Length;

            context.Response.StatusCode = 500;
            context.Response.StatusDescription = "Internal Server Error";
            context.Response.ContentType = contentType.ToValue();
            FlushResponse(context, buffer, length);
        }

        /// <summary>
        /// Default response method when a route or file is not found
        /// </summary>
        protected virtual void NotFound(HttpListenerContext context, string payload = "<h1>Not Found</h1>", ContentType contentType = ContentType.HTML)
        {
            var buffer = Encoding.UTF8.GetBytes(payload);
            var length = buffer.Length;

            context.Response.StatusCode = 404;
            context.Response.StatusDescription = "Not Found";
            context.Response.ContentType = contentType.ToValue();
            FlushResponse(context, buffer, length);
        }

        /// <summary>
        /// Respond to a request using text
        /// </summary>
        protected void SendTextResponse(HttpListenerContext context, string payload, Encoding encoding = null)
        {
            encoding = (object.ReferenceEquals(encoding, null)) ? Encoding.UTF8 : encoding;

            var buffer = encoding.GetBytes(payload);
            var length = buffer.Length;

            context.Response.ContentEncoding = encoding;
            FlushResponse(context, buffer, length);
        }

        /// <summary>
        /// Respond to a request with the contents of a file
        /// </summary>
        protected void SendFileResponse(HttpListenerContext context, string path)
        {
            var ext = Path.GetExtension(path).ToUpper().TrimStart('.');
            var type = (Enum.IsDefined(typeof(ContentType), ext)) ? (ContentType)Enum.Parse(typeof(ContentType), ext) : ContentType.DEFAULT;

            var buffer = this.GetFileBytes(path, type.IsText());
            var length = buffer.Length;

            var lastModified = File.GetLastWriteTimeUtc(path).ToString("R");
            var expireDate = DateTime.Now.AddHours(23).ToString("R");

            context.Response.AddHeader("Last-Modified", lastModified);
            context.Response.AddHeader("Expires", expireDate);
            context.Response.ContentType = type.ToValue();

            if (context.Request.Headers.AllKeys.Contains("If-Modified-Since") && context.Request.Headers["If-Modified-Since"].Equals(lastModified))
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.Response.Close();
            }
            else
            {
                FlushResponse(context, buffer, length);
            }
        }

        /// <summary>
        /// Flushes the remaining contents from the buffer, sets the content length and closes the response output stream
        /// </summary>
        protected void FlushResponse(HttpListenerContext context, byte[] buffer, int length)
        {
            if (context.Request.Headers.AllKeys.Contains("Accept-Encoding") && context.Request.Headers["Accept-Encoding"].Contains("gzip") && length > 1024)
            {
                using (var ms = new MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionMode.Compress))
                    {
                        zip.Write(buffer, 0, length);
                    }
                    buffer = ms.ToArray();
                }
                length = buffer.Length;
                context.Response.AddHeader("Content-Encoding", "gzip");
            }

            context.Response.ContentLength64 = length;
            context.Response.OutputStream.Write(buffer, 0, length);
            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        private byte[] GetFileBytes(string path, bool istext)
        {
            byte[] buffer;

            if (istext)
            {
                var reader = new StreamReader(path);
                buffer = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                reader.Close();
            }
            else
            {
                var finfo = new FileInfo(path);
                var reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
                buffer = reader.ReadBytes((int)finfo.Length);
                reader.Close();
            }

            return buffer;
        }
    }
}
