using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Cache;
using Grapevine.Shared;

namespace Grapevine.Client
{
    /// <summary>
    /// Represents a server exposing resources to interact with
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Gets the base URL of the server exposing resources
        /// </summary>
        Uri BaseUrl { get; }

        /// <summary>
        /// Gets the cookies sent with and updated by each request
        /// </summary>
        CookieContainer Cookies { get; }

        /// <summary>
        /// Gets or sets optional credentials needed to interact with server resources
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets the Domain Name System (DNS) host name or IP address of a server
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// Gets or sets the password associated with the user that accesses the resource, will be included in the URI
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the port number of the URI
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the scheme name of the URI
        /// </summary>
        UriScheme Scheme { get; set; }

        /// <summary>
        /// The user name associated with the user that accesses the resource, will be included in the URI
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets an IRestResponse returned from sending an IRestRequest to the server represented by the IRestClient
        /// </summary>
        IRestResponse Execute(IRestRequest restRequest);
    }

    public delegate void WebExceptionHandler(IRestClient client, IRestRequest request, WebException exception);

    public class RestClient : IRestClient
    {
        protected UriBuilder Builder;

        public Uri BaseUrl => Builder.Uri;
        public CookieContainer Cookies { get; }
        public ICredentials Credentials { get; set; }

        private Action _requestTimeoutAction;

        public Dictionary<WebExceptionStatus, WebExceptionHandler> WebExceptionHandlers;

        public RestClient()
        {
            Builder = new UriBuilder();
            Scheme = UriScheme.Http;
            Cookies = new CookieContainer();
            WebExceptionHandlers = new Dictionary<WebExceptionStatus, WebExceptionHandler>();
        }

        [Obsolete("RequestTimeoutAction is deprecated, add an entry to WebExceptionHandlers instead.")]
        public Action RequestTimeoutAction
        {
            get { return _requestTimeoutAction; }
            set
            {
                _requestTimeoutAction = value;
                WebExceptionHandlers[WebExceptionStatus.Timeout] = (client, request, exception) => _requestTimeoutAction();
            }
        }

        public string Host
        {
            get { return Builder.Host;  }
            set { Builder.Host = value; }
        }

        public string Password
        {
            get { return Builder.Password;  }
            set { Builder.Password = value; }
        }

        public int Port
        {
            get { return Builder.Port;  }
            set { Builder.Port = value; }
        }

        public UriScheme Scheme
        {
            get { return Enum.GetValues(typeof (UriScheme)).Cast<UriScheme>().First(t => t.ToScheme().Equals(Builder.Scheme)); }
            set { Builder.Scheme = value.ToScheme(); }
        }

        public string UserName
        {
            get { return Builder.UserName;  }
            set { Builder.UserName = value; }
        }

        public IRestResponse Execute(IRestRequest restRequest)
        {
            var request = restRequest.ToHttpWebRequest(Builder, Cookies);

            if (request.Credentials == null) request.Credentials = Credentials;
            request.CookieContainer.Add(Cookies.GetCookies(request.RequestUri));

            RestResponse response;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var httpresponse = (HttpWebResponse)request.GetResponse();
                var elapsed = stopwatch.ElapsedMilliseconds;
                response = new RestResponse(httpresponse) { ElapsedTime = elapsed };
            }
            catch (WebException e)
            {
                var elapsed = stopwatch.ElapsedMilliseconds;

                if (WebExceptionHandlers.ContainsKey(e.Status))
                {
                    WebExceptionHandlers[e.Status].Invoke(this, restRequest, e);
                    return null;
                }
                if (e.Response == null) throw;

                var httpresponse = (HttpWebResponse) e.Response;
                response = new RestResponse(httpresponse)
                {
                    ElapsedTime = elapsed,
                    Error = e.Message,
                    ErrorStatus = e.Status
                };
            }

            if (response.Cookies != null) Cookies.Add(response.Cookies);
            return response;
        }

        /// <summary>
        /// Gets or sets the default cache policy for this request
        /// </summary>
        public static RequestCachePolicy DefaultCachePolicy
        {
            get { return HttpWebRequest.DefaultCachePolicy; }
            set { HttpWebRequest.DefaultCachePolicy = value; }
        }

        /// <summary>
        /// Gets or sets the default maximum length of an HTTP error response
        /// </summary>
        public static int DefaultMaximumErrorResponseLength
        {
            get { return HttpWebRequest.DefaultMaximumErrorResponseLength; }
            set { HttpWebRequest.DefaultMaximumErrorResponseLength = value; }
        }

        /// <summary>
        /// Gets or sets the default for the MaximumResponseHeadersLength property
        /// </summary>
        public static int DefaultMaximumResponseHeadersLength
        {
            get { return HttpWebRequest.DefaultMaximumResponseHeadersLength; }
            set { HttpWebRequest.DefaultMaximumResponseHeadersLength = value; }
        }

        /// <summary>
        /// Gets or sets the global HTTP proxy
        /// </summary>
        public static IWebProxy DefaultWebProxy
        {
            get { return WebRequest.DefaultWebProxy; }
            set { WebRequest.DefaultWebProxy = value; }
        }
    }
}
