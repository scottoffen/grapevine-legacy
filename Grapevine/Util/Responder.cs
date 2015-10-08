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
            this.InternalServerError (context, e.ToString());
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

            var lastWriteTime = File.GetLastWriteTimeUtc(path);
            var lastModified = lastWriteTime.ToString("R");
            var maxAge = (long)((DateTime.UtcNow - lastWriteTime).TotalSeconds + 86400);

            context.Response.AddHeader("Last-Modified", lastModified);
            context.Response.AddHeader("max-age", maxAge.ToString());
            context.Response.ContentType = type.ToValue();

            var ifModified = context.Request.Headers["If-Modified-Since"];
            if (null != ifModified && ifModified == lastModified)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotModified;
                context.Response.Close();
            }
            else
            {
                if (type.IsText())
                {
                    // Don't get too excited; this only detects UTF-8 and UTF-16.
                    byte [] buffer;
                    using (var reader = new StreamReader(path))
                    {
                        buffer = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                    }
                    FlushResponse(context, buffer, buffer.Length);
                }
                else
                {
                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        FlushResponse(context, stream);
                    }
                }
            }
        }

        /// <summary>
        /// Flushes the remaining contents from the buffer, sets the content length and closes the response output stream
        /// </summary>
        protected void FlushResponse(HttpListenerContext context, byte[] buffer, int length)
        {
            if (length > 1024
                && context.Request.Headers.AllKeys.Contains("Accept-Encoding") 
                && context.Request.Headers["Accept-Encoding"].Contains("gzip"))
            {
                using (var ms = new MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
                    {
                        zip.Write(buffer, 0, length);
                    }

                    ms.Position = 0;

                    context.Response.AddHeader("Content-Encoding", "gzip");
                    context.Response.ContentLength64 = ms.Length;
                    ms.WriteTo( context.Response.OutputStream );
                 }
            }
            else
            {
                context.Response.ContentLength64 = length;
                context.Response.OutputStream.Write(buffer, 0, length);
            }

            context.Response.OutputStream.Close();
            context.Response.Close();
        }

        protected void FlushResponse(HttpListenerContext context, FileStream stream)
        {
            if (stream.Length > 1024
                && context.Request.Headers.AllKeys.Contains("Accept-Encoding") 
                && context.Request.Headers["Accept-Encoding"].Contains("gzip"))
            {
                using (var ms = new MemoryStream())
                {
                    using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
                    {
                        stream.CopyTo( zip );
                    }
                    ms.Position = 0;

                    context.Response.AddHeader("Content-Encoding", "gzip");
                    context.Response.ContentLength64 = ms.Length;
                    ms.WriteTo( context.Response.OutputStream );
                 }
            }
            else
            {
                context.Response.ContentLength64 = stream.Length;
                stream.CopyTo( context.Response.OutputStream );
            }

            context.Response.OutputStream.Close();
            context.Response.Close();
        }
    }
}
