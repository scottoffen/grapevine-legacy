using System;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Grapevine.Util;
using System.IO;

namespace Grapevine.Server
{
    public interface IHttpRequest
    {
        string[] AcceptTypes { get; }

        int ClientCertificateError { get; }

        Encoding ContentEncoding { get; }

        long ContentLength64 { get; }

        string ContentType { get; }

        CookieCollection Cookies { get; }

        dynamic Dynamic { get; }

        bool HasEntityBody { get; }

        NameValueCollection Headers { get; }

        HttpMethod HttpMethod { get; }

        bool IsAuthenticated { get; }

        bool IsLocal { get; }

        bool IsSecureConnection { get; }

        bool KeepAlive { get; }

        IPEndPoint LocalEndPoint { get; }

        string PathInfo { get; }

        ReadOnlyDictionary<string, string> PathParameters { get; set; }

        string Payload { get; }

        Version ProtocolVersion { get; }

        NameValueCollection QueryString { get; }

        string RawUrl { get; }

        IPEndPoint RemoteEndPoint { get; }

        Guid RequestTraceIdentifier { get; }

        string ServiceName { get; }

        TransportContext TransportContext { get; }

        Uri Url { get; }

        Uri UrlReferrer { get; }

        string UserAgent { get; }

        string UserHostAddress { get; }

        string UserHostname { get; }

        string[] UserLanguages { get; }

        IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state);

        X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult);

        X509Certificate2 GetClientCertificate();
    }

    public class HttpRequest : DynamicAspect, IHttpRequest
    {
        private readonly HttpListenerRequest _request;

        internal HttpRequest(HttpListenerRequest request)
        {
            _request = request;
            HttpMethod = (HttpMethod) Enum.Parse(typeof (HttpMethod), _request.HttpMethod);
            PathInfo = (RawUrl.Split(new[] { '?' }, 2))[0];

            using (var reader = new StreamReader(_request.InputStream, _request.ContentEncoding))
            {
                Payload = reader.ReadToEnd();
            }
        }

        public string[] AcceptTypes => _request.AcceptTypes;

        public int ClientCertificateError => _request.ClientCertificateError;

        public Encoding ContentEncoding => _request.ContentEncoding;

        public long ContentLength64 => _request.ContentLength64;

        public string ContentType => _request.ContentType;

        public CookieCollection Cookies => _request.Cookies;

        public bool HasEntityBody => _request.HasEntityBody;

        public NameValueCollection Headers => _request.Headers;

        public HttpMethod HttpMethod { get; }

        public bool IsAuthenticated => _request.IsAuthenticated;

        public bool IsLocal => _request.IsLocal;

        public bool IsSecureConnection => _request.IsSecureConnection;

        public bool KeepAlive => _request.KeepAlive;

        public IPEndPoint LocalEndPoint => _request.LocalEndPoint;

        public string PathInfo { get; }

        public ReadOnlyDictionary<string, string> PathParameters { get; set; }

        public string Payload { get; }

        public Version ProtocolVersion => _request.ProtocolVersion;

        public NameValueCollection QueryString => _request.QueryString;

        public string RawUrl => _request.RawUrl;

        public IPEndPoint RemoteEndPoint => _request.RemoteEndPoint;

        public Guid RequestTraceIdentifier => _request.RequestTraceIdentifier;

        public string ServiceName => _request.ServiceName;

        public TransportContext TransportContext => _request.TransportContext;

        public Uri Url => _request.Url;

        public Uri UrlReferrer => _request.UrlReferrer;

        public string UserAgent => _request.UserAgent;

        public string UserHostAddress => _request.UserHostAddress;

        public string UserHostname => _request.UserHostName;

        public string[] UserLanguages => _request.UserLanguages;

        public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state)
        {
            return _request.BeginGetClientCertificate(requestCallback, state);
        }

        public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult)
        {
            return _request.EndGetClientCertificate(asyncResult);
        }

        public X509Certificate2 GetClientCertificate()
        {
            return _request.GetClientCertificate();
        }
    }
}
