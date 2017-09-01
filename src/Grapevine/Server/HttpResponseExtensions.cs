using System;
using System.IO;
using System.Linq;
using System.Text;
using Grapevine.Interfaces.Server;
using Grapevine.Interfaces.Shared;
using Grapevine.Shared;
using HttpStatusCode = Grapevine.Shared.HttpStatusCode;

namespace Grapevine.Server
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        ///Sends a response and closes the response output using the specified status code as the response content.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        public static void SendResponse(this IHttpResponse response, HttpStatusCode statusCode)
        {
            response.StatusCode = statusCode;
            response.ContentType = ContentType.HTML;
            response.ContentEncoding = Encoding.ASCII;
            response.SendResponse($"<h1>{response.StatusDescription}</h1>");
        }

        /// <summary>
        /// Sends a response and closes the response output using the specified status code and content.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        /// <param name="content"></param>
        public static void SendResponse(this IHttpResponse response, HttpStatusCode statusCode, string content)
        {
            response.StatusCode = statusCode;
            response.SendResponse(content);
        }

        /// <summary>
        /// Sends a response and closes the response output using the specified status code and the formatted exception as the response content.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statusCode"></param>
        /// <param name="exception"></param>
        public static void SendResponse(this IHttpResponse response, HttpStatusCode statusCode, Exception exception)
        {
            response.StatusCode = statusCode;
            response.SendResponse($"{exception.Message}{Environment.NewLine}<br>{Environment.NewLine}{exception.StackTrace}");
        }

        /// <summary>
        /// Sends a response and closes the response output using the content and encoding provided.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        public static void SendResponse(this IHttpResponse response, string content, Encoding encoding)
        {
            response.ContentEncoding = encoding;
            response.SendResponse(content);
        }

        /// <summary>
        /// Sends a response and closes the response output using the content provided.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="content"></param>
        public static void SendResponse(this IHttpResponse response, string content)
        {
            response.SendResponse(response.ContentEncoding.GetBytes(content));
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response, sets the Content-Disposition header to attachment using the filename provided and sets the content type and encoding using the provided values.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        /// <param name="filename"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream, ContentType contentType, Encoding encoding, string filename)
        {
            response.ContentType = contentType;
            response.ContentEncoding = encoding;
            response.SendResponse(stream, filename);
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response, sets the Content-Disposition header to attachment using the filename provided and sets the encoding using the provided value.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="filename"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream, Encoding encoding, string filename)
        {
            response.ContentEncoding = encoding;
            response.SendResponse(stream, filename);
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response, sets the Content-Disposition header to attachment using the filename provided and sets the content type using the provided value.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        /// <param name="filename"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream, ContentType contentType, string filename)
        {
            response.ContentType = contentType;
            response.SendResponse(stream, filename);
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response and sets the content type and encoding using the provided values.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        /// <param name="encoding"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream, ContentType contentType, Encoding encoding)
        {
            response.ContentType = contentType;
            response.ContentEncoding = encoding;
            response.SendResponse(stream);
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response and sets the content type using the provided value.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        /// <param name="contentType"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream, ContentType contentType)
        {
            response.ContentType = contentType;
            response.SendResponse(stream);
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response and sets the encoding using the provided value.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream, Encoding encoding)
        {
            response.ContentEncoding = encoding;
            response.SendResponse(stream);
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response, sets the Content-Disposition header to attachment using the filename provided.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        /// <param name="filename"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream, string filename)
        {
            response.AddHeader("Content-Disposition", $"attachment; filename=\"{filename}\"");
            response.SendResponse(stream);
        }

        /// <summary>
        /// Sends the provided Stream as the content of the response.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="stream"></param>
        public static void SendResponse(this IHttpResponse response, Stream stream)
        {
            if (!response.Headers.AllKeys.ToArray().Contains("Expires"))
                response.AddHeader("Expires",
                    DateTime.Now.AddHours(response.Advanced?.DefaultHoursToExpire ?? 23).ToString("R"));

            var buffer = response.ContentType.IsText()
                ? stream.GetTextBytes(response.ContentEncoding)
                : stream.GetBinaryBytes();

            response.SendResponse(buffer);
        }

        public static void TrySendResponse(this IHttpResponse response, IGrapevineLogger logger, HttpStatusCode status, Exception e = null)
        {
            try
            {
                response.SendResponse(status, e);
            }
            catch (Exception ex)
            {
                logger.Log(new LogEvent { Exception = ex, Level = LogLevel.Error, Message = "Failed to send response" });
            }
        }
    }
}