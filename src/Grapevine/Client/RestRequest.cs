using System;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using Grapevine.Util;

namespace Grapevine.Client
{
    /// <summary>
    /// Reuseable collection of all data required to send a request to a RestClient
    /// </summary>
    public interface IRestRequest
    {
        /// <summary>
        /// Gets or sets the value of the Accept HTTP header
        /// </summary>
        string Accept { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the request should follow redirection responses
        /// </summary>
        bool AllowAutoRedirect { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to buffer the data sent to the Internet resource
        /// </summary>
        bool AllowWriteStreamBuffering { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to buffer the data sent to the Internet resource
        /// </summary>
        AuthenticationLevel AuthenticationLevel { get; set; }

        /// <summary>
        /// Gets or sets the type of decompression that is used
        /// </summary>
        DecompressionMethods AutomaticDecompression { get; set; }

        /// <summary>
        /// Gets or sets the cache policy for this request
        /// </summary>
        RequestCachePolicy CachePolicy { get; set; }

        /// <summary>
        /// Gets or sets the collection of security certificates that are associated with this request
        /// </summary>
        X509Certificate2Collection ClientCertificates { get; set; }

        /// <summary>
        /// Gets or sets the value of the Connection HTTP header
        /// </summary>
        string Connection { get; set; }

        /// <summary>
        /// Gets or sets the name of the connection group for the request
        /// </summary>
        string ConnectionGroupName { get; set; }

        /// <summary>
        /// Gets or sets the Content-length HTTP header
        /// </summary>
        long ContentLength { get; set; }

        /// <summary>
        /// Gets or sets the value of the Content-type HTTP header
        /// </summary>
        ContentType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the delegate method called when an HTTP 100-continue response is received from the Internet resource
        /// </summary>
        HttpContinueDelegate ContinueDelegate { get; set; }

        /// <summary>
        /// Gets or sets authentication information for the request
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary>
        /// Get or set the Date HTTP header value to use in an HTTP request
        /// </summary>
        DateTime Date { get; set; }

        /// <summary>
        /// Get or set the Encoding for the current request
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the value of the Expect HTTP header; defaults to UTF-8
        /// </summary>
        string Expect { get; set; }

        /// <summary>
        /// Specifies a collection of the name/value pairs that make up the HTTP headers
        /// </summary>
        WebHeaderCollection Headers { get; set; }

        /// <summary>
        /// Get or set the Host header value to use in an HTTP request independent from the request URI
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Gets or sets the value of the If-Modified-Since HTTP header
        /// </summary>
        DateTime IfModifiedSince { get; set; }

        /// <summary>
        /// Gets or sets the impersonation level for the current request
        /// </summary>
        TokenImpersonationLevel ImpersonationLevel { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to make a persistent connection to the Internet resource
        /// </summary>
        bool KeepAlive { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of redirects that the request follows
        /// </summary>
        int MaximumAutomaticRedirections { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed length of the response headers
        /// </summary>
        int MaximumResponseHeadersLength { get; set; }

        /// <summary>
        /// Gets or sets the media type of the request
        /// </summary>
        string MediaType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method for the request
        /// </summary>
        HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// Gets the parsed PathInfo
        /// </summary>
        string PathInfo { get; }

        /// <summary>
        /// Specifies a collection of the name/value pairs to replace placeholders in the resource path
        /// </summary>
        PathParams PathParams { get; }

        /// <summary>
        /// Get or set the data payload
        /// </summary>
        string Payload { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to pipeline the request to the Internet resource
        /// </summary>
        bool Pipelined { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to send an Authorization header with the request
        /// </summary>
        bool PreAuthenticate { get; set; }

        /// <summary>
        /// Gets or sets the version of HTTP to use for the request
        /// </summary>
        Version ProtocolVersion { get; set; }

        /// <summary>
        /// Gets or sets proxy information for the request
        /// </summary>
        IWebProxy Proxy { get; set; }

        /// <summary>
        /// Specifies a collection of the name/value pairs that make up the request query string
        /// </summary>
        QueryString QueryString { get; }

        /// <summary>
        /// Gets or sets a time-out in milliseconds when writing to or reading from a stream
        /// </summary>
        int ReadWriteTimeout { get; set; }

        /// <summary>
        /// Gets or sets the value of the Referer HTTP header
        /// </summary>
        string Referer { get; set; }

        /// <summary>
        /// Get or set the optionally-tokenized string that represents the path to the desired resource; when changed, all parameters are cleared
        /// </summary>
        string Resource { get; set; }

        /// <summary>
        /// Gets the original Uniform Resource Identifier (URI) of the request
        /// </summary>
        Uri RequestUri { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to send data in segments to the Internet resource
        /// </summary>
        bool SendChunked { get; set; }

        /// <summary>
        /// Gets the service point to use for the request
        /// </summary>
        ServicePoint ServicePoint { get; set; }

        /// <summary>
        /// Gets or sets the time-out value in milliseconds for the GetResponse and GetRequestStream methods
        /// </summary>
        int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the value of the Transfer-encoding HTTP header
        /// </summary>
        string TransferEncoding { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to allow high-speed NTLM-authenticated connection sharing
        /// </summary>
        bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>
        /// Gets or sets a Boolean value that controls whether default credentials are sent with requests
        /// </summary>
        bool UseDefaultCredentials { get; set; }

        /// <summary>
        /// Gets or sets the value of the User-agent HTTP header
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>
        /// Reset the parameterized request values
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets the current request as an HttpWebRequest
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns>HttpWebRequest</returns>
        HttpWebRequest ToHttpWebRequest(string baseUrl);
    }

    public class RestRequest : IRestRequest
    {
        public static int GlobalTimeout = 2000;

        private string _resource;

        public string Accept { get; set; }
        public bool AllowAutoRedirect { get; set; }
        public bool AllowWriteStreamBuffering { get; set; }
        public AuthenticationLevel AuthenticationLevel { get; set; }
        public DecompressionMethods AutomaticDecompression { get; set; }
        public RequestCachePolicy CachePolicy { get; set; }
        public X509Certificate2Collection ClientCertificates { get; set; }
        public string Connection { get; set; }
        public string ConnectionGroupName { get; set; }
        public long ContentLength { get; set; }
        public ContentType ContentType { get; set; }
        public HttpContinueDelegate ContinueDelegate { get; set; }
        public ICredentials Credentials { get; set; }
        public DateTime Date { get; set; }
        public Encoding Encoding { get; set; }
        public string Expect { get; set; }
        public WebHeaderCollection Headers { get; set; }
        public string Host { get; set; }
        public DateTime IfModifiedSince { get; set; }
        public TokenImpersonationLevel ImpersonationLevel { get; set; }
        public bool KeepAlive { get; set; }
        public int MaximumAutomaticRedirections { get; set; }
        public int MaximumResponseHeadersLength { get; set; }
        public string MediaType { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public string Payload { get; set; }
        public string PathInfo => PathParams.ParseResource(_resource);
        public PathParams PathParams { get; }
        public bool Pipelined { get; set; }
        public bool PreAuthenticate { get; set; }
        public Version ProtocolVersion { get; set; }
        public IWebProxy Proxy { get; set; }
        public QueryString QueryString { get; }
        public int ReadWriteTimeout { get; set; }
        public string Referer { get; set; }
        public Uri RequestUri { get; set; }
        public bool SendChunked { get; set; }
        public ServicePoint ServicePoint { get; set; }
        public int Timeout { get; set; }
        public string TransferEncoding { get; set; }
        public bool UnsafeAuthenticatedConnectionSharing { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string UserAgent { get; set; }

        /// <summary>
        /// Provides direct access to selected methods and properties on the internal HttpWebResponse instance in use; do not used unless you are fully aware of what you are doing and the consequences involved.
        /// </summary>
        public AdvancedRestRequest Advanced { get; }

        public RestRequest()
        {
            Advanced = new AdvancedRestRequest(this);
            ContentType = ContentType.TXT;
            Encoding = Encoding.UTF8;
            Headers = new WebHeaderCollection
            {
                {HttpRequestHeader.CacheControl, "no-store, must-revalidate"},
                {HttpRequestHeader.AcceptEncoding, "gzip"}
            };
            HttpMethod = HttpMethod.GET;
            PathParams = new PathParams();
            QueryString = new QueryString();
            Timeout = GlobalTimeout;
        }

        public string Resource
        {
            get
            {
                return _resource;
            }
            set
            {
                var r = Regex.Replace(value, "^/", "");
                if (value.Equals(r)) return;
                _resource = r;
                PathParams.Clear();
            }
        }

        public void Reset()
        {
            Payload = null;
            PathParams.Clear();
            QueryString.Clear();
        }

        public HttpWebRequest ToHttpWebRequest(string baseUrl)
        {
            return Advanced.ToHttpWebRequest(baseUrl);
        }
    }

    /// <summary>
    /// Provides direct access to selected methods and properties on the internal HttpWebRequest instance. This class cannot be inherited.
    /// </summary>
    public sealed class AdvancedRestRequest
    {
        private readonly RestRequest _request;

        internal AdvancedRestRequest(RestRequest request)
        {
            _request = request;
        }

        /// <summary>
        /// Gets the current request as an HttpWebRequest
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <returns>HttpWebRequest</returns>
        public HttpWebRequest ToHttpWebRequest(string baseUrl)
        {
            var url = $"{baseUrl}/{_request.PathInfo}{(_request.QueryString.Count > 0 ? $"?{_request.QueryString}" : "")}";
            var request = (HttpWebRequest) WebRequest.Create(url);

            request.Accept = _request.Accept;
            request.AllowAutoRedirect = _request.AllowAutoRedirect;
            request.AllowWriteStreamBuffering = _request.AllowWriteStreamBuffering;
            request.AuthenticationLevel = _request.AuthenticationLevel;
            request.AutomaticDecompression = _request.AutomaticDecompression;
            request.CachePolicy = _request.CachePolicy;
            request.ClientCertificates = _request.ClientCertificates;
            request.Connection = _request.Connection;
            request.ConnectionGroupName = _request.ConnectionGroupName;
            request.ContentLength = _request.ContentLength;
            request.ContentType = _request.ContentType.ToValue();
            request.ContinueDelegate = _request.ContinueDelegate;
            request.Credentials = _request.Credentials;
            request.Date = _request.Date;
            request.Expect = _request.Expect;
            request.Headers = _request.Headers;
            request.Host = _request.Host;
            request.IfModifiedSince = _request.IfModifiedSince;
            request.ImpersonationLevel = _request.ImpersonationLevel;
            request.KeepAlive = _request.KeepAlive;
            request.MaximumAutomaticRedirections = _request.MaximumAutomaticRedirections;
            request.MaximumResponseHeadersLength = _request.MaximumResponseHeadersLength;
            request.MediaType = _request.MediaType;
            request.Method = _request.HttpMethod.ToString();
            request.Pipelined = _request.Pipelined;
            request.PreAuthenticate = _request.PreAuthenticate;
            request.ProtocolVersion = _request.ProtocolVersion;
            request.Proxy = _request.Proxy;
            request.ReadWriteTimeout = _request.ReadWriteTimeout;
            request.Referer = _request.Referer;
            request.SendChunked = _request.SendChunked;
            request.Timeout = _request.Timeout;
            request.TransferEncoding = _request.TransferEncoding;
            request.UnsafeAuthenticatedConnectionSharing = _request.UnsafeAuthenticatedConnectionSharing;
            request.UseDefaultCredentials = _request.UseDefaultCredentials;
            request.UserAgent = _request.UserAgent;

            if (_request.Payload == null) return request;

            var content = _request.Encoding.GetBytes(_request.Payload);
            request.ContentLength = content.Length;
            request.GetRequestStream().Write(content, 0, content.Length);
            request.GetRequestStream().Close();

            _request.Reset();
            return request;
        }
    }
}
