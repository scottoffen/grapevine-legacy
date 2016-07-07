using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Principal;
using Grapevine.Client;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Test.Client
{
    public class RestRequestTester
    {
        [Fact]
        public void rest_request_ctor_sets_resource_to_empty_string()
        {
            var request = new RestRequest();
            request.Resource.ShouldBe(string.Empty);
        }

        [Fact]
        public void rest_request_ctor_strips_leading_slash_from_resource()
        {
            const string resource1 = "/user/Doe/John";
            const string resource2 = "user/Doe/John";

            var request = new RestRequest(resource1);

            request.Resource.ShouldBe(resource2);
        }

        [Fact]
        public void rest_request_reset_clears_query_string_and_path_params()
        {
            var request = new RestRequest();

            request.PathParams.Add("key1", "value1");
            request.PathParams.Add("key2", "value2");
            request.PathParams.Count.ShouldBe(2);

            request.QueryString.Add("param1", "value1");
            request.QueryString.Add("param2", "value2");
            request.QueryString.Count.ShouldBe(2);

            request.Reset();

            request.PathParams.Count.ShouldBe(0);
            request.QueryString.Count.ShouldBe(0);
        }

        [Fact]
        public void rest_request_default_can_convert_to_httpwebrequest()
        {
            var rest_request = new RestRequest();
            var http_request = rest_request.ToHttpWebRequest("http://localhost:1234");

            http_request.Accept.ShouldBeNull();
            http_request.AllowAutoRedirect.ShouldBeTrue();
            http_request.AllowWriteStreamBuffering.ShouldBeTrue();
            http_request.AuthenticationLevel.ShouldBe(AuthenticationLevel.MutualAuthRequested);
            http_request.AutomaticDecompression.ShouldBe(DecompressionMethods.None);
            http_request.CachePolicy.Level.ShouldBe(RequestCacheLevel.BypassCache);
            http_request.ClientCertificates.Count.ShouldBe(0);
            http_request.Connection.ShouldBeNull();
            http_request.ConnectionGroupName.ShouldBeNull();
            http_request.ContentLength.ShouldBe(0);
            http_request.ContentType.ShouldBe(ContentType.TXT.ToValue());
            http_request.ContinueDelegate.ShouldBeNull();
            http_request.Expect.ShouldBeNull();
            http_request.Headers.Count.ShouldBe(3);
            http_request.ImpersonationLevel.ShouldBe(TokenImpersonationLevel.Delegation);
            http_request.KeepAlive.ShouldBeTrue();
            http_request.MaximumAutomaticRedirections.ShouldBe(50);
            http_request.MaximumResponseHeadersLength.ShouldBe(64);
            http_request.MediaType.ShouldBeNull();
            http_request.Method.ShouldBe(HttpMethod.GET.ToString());
            http_request.Pipelined.ShouldBeTrue();
            http_request.PreAuthenticate.ShouldBeFalse();
            http_request.ProtocolVersion.Major.ShouldBe(1);
            http_request.ProtocolVersion.Minor.ShouldBe(1);
            http_request.ReadWriteTimeout.ShouldBe(300000);
            http_request.Referer.ShouldBeNull();
            http_request.SendChunked.ShouldBeFalse();
            http_request.Timeout.ShouldBe(100000);
            http_request.TransferEncoding.ShouldBeNull();
            http_request.UnsafeAuthenticatedConnectionSharing.ShouldBeFalse();
            http_request.UseDefaultCredentials.ShouldBeFalse();
            http_request.UserAgent.ShouldBeNull();
        }
    }
}
