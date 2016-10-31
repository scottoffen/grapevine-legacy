using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Grapevine.Shared;
using System.IO;

namespace Grapevine.Interfaces.Server
{
    /// <summary>
    /// Describes an incoming HTTP request to an HttpListener object
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Gets the MIME types accepted by the client
        /// </summary>
        string[] AcceptTypes { get; }

        /// <summary>
        /// Gets an error code that identifies a problem with the X509Certificate provided by the client
        /// </summary>
        int ClientCertificateError { get; }

        /// <summary>
        /// Gets the content encoding that can be used with data sent with the request
        /// </summary>
        Encoding ContentEncoding { get; }

        /// <summary>
        /// Gets the length of the body data included in the request
        /// </summary>
        long ContentLength64 { get; }

        /// <summary>
        /// Gets the MIME type of the body data included in the request
        /// </summary>
        ContentType ContentType { get; }

        /// <summary>
        /// Gets the cookies sent with the request
        /// </summary>
        CookieCollection Cookies { get; }

        /// <summary>
        /// Gets a Boolean value that indicates whether the request has associated body data
        /// </summary>
        bool HasEntityBody { get; }

        /// <summary>
        /// Gets the collection of header name/value pairs sent in the request
        /// </summary>
        NameValueCollection Headers { get; }

        /// <summary>
        /// Gets the HTTPMethod specified by the client
        /// </summary>
        HttpMethod HttpMethod { get; }

        /// <summary>
        /// A value that represents a unique identifier for this request
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets a Boolean value that indicates whether the client sending this request is authenticated
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Gets a Boolean value that indicates whether the request is sent from the local computer
        /// </summary>
        bool IsLocal { get; }

        /// <summary>
        /// Gets a Boolean value that indicates whether the TCP connection used to send the request is using the Secure Sockets Layer (SSL) protocol
        /// </summary>
        bool IsSecureConnection { get; }

        /// <summary>
        /// Gets a Boolean value that indicates whether the client requests a persistent connection
        /// </summary>
        bool KeepAlive { get; }

        /// <summary>
        /// Get the server IP address and port number to which the request is directed
        /// </summary>
        IPEndPoint LocalEndPoint { get; }

        /// <summary>
        /// Gets a representation of the HttpMethod and PathInfo of the request
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the URL information (without the host, port or query string) requested by the client
        /// </summary>
        string PathInfo { get; }

        /// <summary>
        /// Gets or sets a dictionary of parameters provided in the PathInfo as identified by the processing route
        /// </summary>
        Dictionary<string, string> PathParameters { get; set; }

        /// <summary>
        /// Reads the HttpListenerRequest InputStream into a string and returns it
        /// </summary>
        string Payload { get; }

        /// <summary>
        /// Gets the HTTP version used by the requesting client
        /// </summary>
        Version ProtocolVersion { get; }

        /// <summary>
        /// Gets the query string included in the request
        /// </summary>
        NameValueCollection QueryString { get; }

        /// <summary>
        /// Gets the URL information (without the host and port) requested by the client.
        /// </summary>
        string RawUrl { get; }

        /// <summary>
        /// Gets the client IP address and port number from which the request originated
        /// </summary>
        IPEndPoint RemoteEndPoint { get; }

        /// <summary>
        /// Gets the request identifier of the incoming HTTP request
        /// </summary>
        Guid RequestTraceIdentifier { get; }

        /// <summary>
        /// Gets the Service Provider Name (SPN) that the client sent on the request
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// Gets the TransportContext for the client request
        /// </summary>
        TransportContext TransportContext { get; }

        /// <summary>
        /// Gets the Uri object requested by the client
        /// </summary>
        Uri Url { get; }

        /// <summary>
        /// Gets the Uniform Resource Identifier (URI) of the resource that referred the client to the server
        /// </summary>
        Uri UrlReferrer { get; }

        /// <summary>
        /// Gets the user agent presented by the client
        /// </summary>
        string UserAgent { get; }

        /// <summary>
        /// Gets the server IP address and port number to which the request is directed
        /// </summary>
        string UserHostAddress { get; }

        /// <summary>
        /// Gets the DNS name and, if provided, the port number specified by the client
        /// </summary>
        string UserHostname { get; }

        /// <summary>
        /// Gets the natural languages that are preferred for the response
        /// </summary>
        string[] UserLanguages { get; }

        /// <summary>
        /// Begins an asynchronous request for the client's X.509 v.3 certificate
        /// </summary>
        /// <param name="requestCallback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state);

        /// <summary>
        /// Ends an asynchronous request for the client's X.509 v.3 certificate
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult);

        /// <summary>
        /// Retrieves the client's X.509 v.3 certificate
        /// </summary>
        /// <returns></returns>
        X509Certificate2 GetClientCertificate();
    }

    public class HttpRequest : IHttpRequest
    {
        private string _payload;
        private ContentType _contentType;
        private HttpMethod _httpMethod;
        private bool _parsedContentType;
        private bool _parsedHttpMethod;

        /// <summary>
        /// The underlying HttpListenerRequest for this instance
        /// </summary>
        protected internal readonly HttpListenerRequest Request;

        /// <summary>
        /// Provides direct access to selected methods and properties on the internal HttpListenerRequest instance in use; do not used unless you are fully aware of what you are doing and the consequences involved.
        /// </summary>
        public AdvancedHttpRequest Advanced { get; }

        protected internal HttpRequest(HttpListenerRequest request)
        {
            Request = request;
            PathInfo = RawUrl.Split(new[] { '?' }, 2)[0];
            Name = $"{HttpMethod} {PathInfo}";
            Id = Guid.NewGuid().Truncate();
            Advanced = new AdvancedHttpRequest(request);
        }

        public string[] AcceptTypes => Request.AcceptTypes;

        public int ClientCertificateError => Request.ClientCertificateError;

        public Encoding ContentEncoding => Request.ContentEncoding;

        public long ContentLength64 => Request.ContentLength64;

        public ContentType ContentType
        {
            get
            {
                if (_parsedContentType) return _contentType;
                _contentType = _contentType.FromString(Request.ContentType);
                _parsedContentType = true;
                return _contentType;
            }
        }

        public CookieCollection Cookies => Request.Cookies;

        public bool HasEntityBody => Request.HasEntityBody;

        public NameValueCollection Headers => Request.Headers;

        public HttpMethod HttpMethod
        {
            get
            {
                if (_parsedHttpMethod) return _httpMethod;
                _httpMethod = HttpMethod.ALL.FromString(Request.HttpMethod);
                _parsedHttpMethod = true;
                return _httpMethod;
            }
        }

        public string Id { get; }

        public bool IsAuthenticated => Request.IsAuthenticated;

        public bool IsLocal => Request.IsLocal;

        public bool IsSecureConnection => Request.IsSecureConnection;

        public bool KeepAlive => Request.KeepAlive;

        public IPEndPoint LocalEndPoint => Request.LocalEndPoint;

        public string Name { get; }

        public string PathInfo { get; }

        public Dictionary<string, string> PathParameters { get; set; }

        public string Payload
        {
            get
            {
                if (_payload != null) return _payload;
                using (var reader = new StreamReader(Request.InputStream, Request.ContentEncoding))
                {
                    _payload = reader.ReadToEnd();
                }
                return _payload;
            }
        }

        public Version ProtocolVersion => Request.ProtocolVersion;

        public NameValueCollection QueryString => Request.QueryString;

        public string RawUrl => Request.RawUrl;

        public IPEndPoint RemoteEndPoint => Request.RemoteEndPoint;

        public Guid RequestTraceIdentifier => Request.RequestTraceIdentifier;

        public string ServiceName => Request.ServiceName;

        public TransportContext TransportContext => Request.TransportContext;

        public Uri Url => Request.Url;

        public Uri UrlReferrer => Request.UrlReferrer;

        public string UserAgent => Request.UserAgent;

        public string UserHostAddress => Request.UserHostAddress;

        public string UserHostname => Request.UserHostName;

        public string[] UserLanguages => Request.UserLanguages;

        public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
        {
            return Request.BeginGetClientCertificate(requestCallback, state);
        }

        public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
        {
            return Request.EndGetClientCertificate(asyncResult);
        }

        public X509Certificate2 GetClientCertificate()
        {
            return Request.GetClientCertificate();
        }
    }

    /// <summary>
    /// Provides direct access to selected methods and properties on the internal HttpListenerRequest instance. This class cannot be inherited.
    /// </summary>
    public sealed class AdvancedHttpRequest
    {
        private readonly HttpListenerRequest _request;

        internal AdvancedHttpRequest(HttpListenerRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Gets a stream that contains the body data sent by the client; recommended to use Payload property instead
        /// </summary>
        public Stream InputStream => _request.InputStream;

        /// <summary>
        /// Get the original ContentType string from the request
        /// </summary>
        public string ContentType => _request.ContentType;

        /// <summary>
        /// Gets the original HttpMethod string from the request
        /// </summary>
        public string HttpMethod => _request.HttpMethod;
    }
}
