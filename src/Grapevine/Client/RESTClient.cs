using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Grapevine.Util;

namespace Grapevine.Client
{
    /// <summary>
    /// Represents a server exposing resources to interact with
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Sends RESTRequest to server represented by RESTClient and returns the RESTResponse received
        /// </summary>
        IRestResponse Execute(IRestRequest request);

        /// <summary>
        /// The base URL of the server exposing resources
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        /// The cookies sent with and updated by each request
        /// </summary>
        CookieContainer Cookies { get; }

        /// <summary>
        /// Optional credentials needed to interact with server resources
        /// </summary>
        NetworkCredential NetworkCredentials { get; set; }
    }

    public class RestClient : IRestClient
    {
        public RestClient(string baseUrl)
        {
            BaseUrl = Regex.Replace(baseUrl, "/$", "");
            Cookies = new CookieContainer();
        }

        public IRestResponse Execute(IRestRequest request)
        {
            var querystr = request.QueryString;
            var url = (object.ReferenceEquals(querystr, null)) ? this.BaseUrl + "/" + request.PathInfo : this.BaseUrl + "/" + request.PathInfo + "?" + querystr;
            var client = CreateRequest(url);

            var stopwatch = new Stopwatch();
            client.Timeout = request.Timeout;

            client.CookieContainer = this.Cookies;
            client.Method = request.Method.ToString();
            client.Headers = request.Headers;

            client.ContentType = request.ContentType.ToValue();

            if (request.Payload != null)
            {
                var content = request.Encoding.GetBytes(request.Payload);
                client.ContentLength = content.Length;
                client.GetRequestStream().Write(content, 0, content.Length);
                client.GetRequestStream().Close();
            }
            else
            {
                client.ContentLength = 0;
            }
                
            HttpWebResponse httpresponse;
            string error = "";
            WebExceptionStatus errorStatus = WebExceptionStatus.Success;

            stopwatch.Start();
            try
            {
                httpresponse = (HttpWebResponse)client.GetResponse();
            }
            catch (WebException e)
            {
                httpresponse = (HttpWebResponse)e.Response;
                error = e.Message;
                errorStatus = e.Status;
            }
            stopwatch.Stop();
            request.Reset();

            var response = new RestResponse(httpresponse, stopwatch.ElapsedMilliseconds, error, errorStatus);
            if (response.Cookies != null)
                this.Cookies.Add(response.Cookies);

            return response;
        }

        public string BaseUrl { get; }

        public CookieContainer Cookies { get; }

        public NetworkCredential NetworkCredentials { get; set; }

        private HttpWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            if (NetworkCredentials != null) request.Credentials = NetworkCredentials;
            return request;
        }
    }
}
