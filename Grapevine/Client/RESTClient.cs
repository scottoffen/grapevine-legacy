using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Grapevine.Client
{
    /// <summary>
    /// Represents a server exposing resources to interact with
    /// </summary>
    public class RESTClient
    {
        public RESTClient(string baseUrl)
        {
            this.BaseUrl = Regex.Replace(baseUrl, "/$", "");
            this.Cookies = new CookieContainer();
        }

        /// <summary>
        /// Sends RESTRequest to server represented by RESTClient and returns the RESTResponse received
        /// </summary>
        public RESTResponse Execute(RESTRequest request)
        {
            var querystr = request.QueryString;
            var url = (object.ReferenceEquals(querystr, null)) ? this.BaseUrl + "/" + request.PathInfo : this.BaseUrl + "/" + request.PathInfo + "?" + querystr;
            var client = CreateRequest(url);

            var stopwatch = new Stopwatch();
            client.Timeout = request.Timeout;

            client.CookieContainer = this.Cookies;
            client.Method = request.Method.ToString();
            client.ContentType = request.ContentType.ToValue();
            client.Headers = request.Headers;

            if (!Object.ReferenceEquals(request.Payload, null))
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
                EventLogger.Log(e);

                httpresponse = (HttpWebResponse)e.Response;
                error = e.Message;
                errorStatus = e.Status;
            }
            stopwatch.Stop();
            request.Reset();

            var response = new RESTResponse(httpresponse, stopwatch.ElapsedMilliseconds, error, errorStatus);
            this.Cookies.Add(response.Cookies);

            return response;
        }

        private HttpWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            if (!Object.ReferenceEquals(this.NetworkCredentials, null))
            {
                request.Credentials = this.NetworkCredentials;
            }

            return request;
        }

        #region Public Properties

        /// <summary>
        /// The base URL of the server exposing resources
        /// </summary>
        public string BaseUrl { get; private set; }

        /// <summary>
        /// The cookies sent with and updated by each request
        /// </summary>
        public CookieContainer Cookies { get; private set; }

        /// <summary>
        /// Optional credentials needed to interact with server resources
        /// </summary>
        public NetworkCredential NetworkCredentials { get; set; }

        #endregion
    }
}
