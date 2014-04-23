using System.Diagnostics;
using System.Net;
using System.Text;
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
            var url = this._baseUrl + "/" + request.PathInfo;
            var client = CreateRequest(url);
            var stopwatch = new Stopwatch();

            client.Timeout = request.Timeout;
            client.CookieContainer = this.Cookies;
            client.Method = request.Method.ToString();
            client.ContentType = request.ContentType.ToValue();

            if (request.Payload != null)
            {
                var content = Encoding.ASCII.GetBytes(request.Payload);
                client.ContentLength = content.Length;
                client.GetRequestStream().Write(content, 0, content.Length);
                client.GetRequestStream().Close();
            }            

            stopwatch.Start();
            var httpresponse = (HttpWebResponse)client.GetResponse();
            stopwatch.Stop();

            this.Cookies.Add(httpresponse.Cookies);

            var response = new RestResponse(httpresponse, stopwatch.ElapsedMilliseconds);
            request.Reset();
            return response;
        }

        public string BaseUrl
        {
            get
            {
                return this._baseUrl;
            }
        }

        public CookieContainer Cookies { get; private set; }

        public NetworkCredential Credentials { get; set; }

        private HttpWebRequest CreateRequest(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);

            if (this.Credentials != null)
            {
                request.Credentials = this.Credentials;
            }

            return request;
        }
    }
}
