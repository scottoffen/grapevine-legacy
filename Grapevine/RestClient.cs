using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Grapevine
{
    public class RestClient
    {
        protected string _baseUrl;

        public RestClient (string baseUrl)
        {
            this._baseUrl = Regex.Replace(baseUrl, "/$", "");
            this.Cookies = new CookieContainer();
        }

        public RestResponse Execute(RestRequest request)
        {
            var querystr = request.QueryString;
            var url      = (object.ReferenceEquals(querystr, null)) ? this._baseUrl + "/" + request.PathInfo : this._baseUrl + "/" + request.PathInfo + "?" + querystr;
            var client   = CreateRequest(url);

            var stopwatch = new Stopwatch();
            client.Timeout = request.Timeout;

            client.CookieContainer = this.Cookies;
            client.Method = request.Method.ToString();
            client.ContentType = request.ContentType.ToValue();

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
            string error       = "";
            string errorStatus = "";

            stopwatch.Start();
            try
            {
                httpresponse = (HttpWebResponse)client.GetResponse();
            }
            catch (WebException e)
            {
                httpresponse = (HttpWebResponse)e.Response;
            }
            stopwatch.Stop();
            request.Reset();

            var response = new RestResponse(httpresponse, stopwatch.ElapsedMilliseconds, error, errorStatus);
            this.Cookies.Add(response.Cookies);

            return response;
        }

        private HttpWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            if (!Object.ReferenceEquals(this.Credentials, null))
            {
                request.Credentials = this.Credentials;
            }

            return request;
        }

        #region Public Properties

        public string BaseUrl
        {
            get
            {
                return this._baseUrl;
            }
        }

        public CookieContainer Cookies { get; private set; }

        public NetworkCredential Credentials { get; set; }

        #endregion
    }
}
