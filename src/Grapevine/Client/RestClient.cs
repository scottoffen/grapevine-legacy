using System;
using System.Diagnostics;
using System.Net;
using System.Net.Cache;
using System.Text.RegularExpressions;

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
        string BaseUrl { get; }

        /// <summary>
        /// Gets the cookies sent with and updated by each request
        /// </summary>
        CookieContainer Cookies { get; }

        /// <summary>
        /// Gets or sets optional credentials needed to interact with server resources
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary>
        /// Gets an IRestResponse returned from sending an IRestRequest to the server represented by the IRestClient
        /// </summary>
        IRestResponse Execute(IRestRequest restRequest);
    }

    public class RestClient : IRestClient
    {
        public string BaseUrl { get; }
        public CookieContainer Cookies { get; }
        public ICredentials Credentials { get; set; }

        public RestClient(string baseUrl)
        {
            if (baseUrl == null) throw new ArgumentNullException(nameof(baseUrl));
            BaseUrl = Regex.Replace(baseUrl, "/$", "");
            Cookies = new CookieContainer();
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

        public IRestResponse Execute(IRestRequest restRequest)
        {
            var request = restRequest.ToHttpWebRequest(BaseUrl);
            if (request.Credentials == null) request.Credentials = Credentials;
            request.CookieContainer.Add(Cookies.GetCookies(request.RequestUri));

            RestResponse response;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var httpresponse = (HttpWebResponse) request.GetResponse();
                var elapsed = stopwatch.ElapsedMilliseconds;
                response = new RestResponse(httpresponse) {ElapsedTime = elapsed};
            }
            catch (WebException e)
            {
                var elapsed = stopwatch.ElapsedMilliseconds;
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
    }
}
