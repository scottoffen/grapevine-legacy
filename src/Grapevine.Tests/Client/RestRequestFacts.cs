using System;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Principal;
using Grapevine.Client;
using Grapevine.Shared;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Client
{
    public class RestRequestFacts
    {
        public class Constructors
        {
            [Fact]
            public void DefaultsResourceToEmptyString()
            {
                var request = new RestRequest();
                request.Resource.ShouldBe(string.Empty);
            }

            [Fact]
            public void StripsLeadingSlashFromResource()
            {
                const string resource1 = "/user/Doe/John";
                const string resource2 = "user/Doe/John";

                var request = new RestRequest(resource1);

                request.Resource.ShouldBe(resource2);
            }
        }

        public class ResetMethod
        {
            [Fact]
            public void ClearsQueryStringAndPathParams()
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
        }

        public class ToHttpWebRequestMethod
        {
            [Fact]
            public void ConvertsToHttpWebRequest()
            {
                var restRequest = new RestRequest();
                var httpRequest =
                    restRequest.ToHttpWebRequest(new UriBuilder
                    {
                        Scheme = UriScheme.Http.ToScheme(),
                        Host = "localhost",
                        Port = 1234
                    }, new CookieContainer());

                httpRequest.Accept.ShouldBeNull();
                httpRequest.AllowAutoRedirect.ShouldBeTrue();
                httpRequest.AllowWriteStreamBuffering.ShouldBeTrue();
                httpRequest.AuthenticationLevel.ShouldBe(AuthenticationLevel.MutualAuthRequested);
                httpRequest.AutomaticDecompression.ShouldBe(DecompressionMethods.None);
                httpRequest.CachePolicy.Level.ShouldBe(RequestCacheLevel.BypassCache);
                httpRequest.ClientCertificates.Count.ShouldBe(0);
                httpRequest.Connection.ShouldBeNull();
                httpRequest.ConnectionGroupName.ShouldBeNull();
                httpRequest.ContentLength.ShouldBe(0);
                httpRequest.ContentType.ShouldBe(ContentType.TXT.ToValue());
                httpRequest.ContinueDelegate.ShouldBeNull();
                httpRequest.CookieContainer.ShouldNotBeNull();
                httpRequest.Expect.ShouldBeNull();
                httpRequest.Headers.Count.ShouldBe(3);
                httpRequest.ImpersonationLevel.ShouldBe(TokenImpersonationLevel.Delegation);
                httpRequest.KeepAlive.ShouldBeTrue();
                httpRequest.MaximumAutomaticRedirections.ShouldBe(50);
                httpRequest.MaximumResponseHeadersLength.ShouldBe(64);
                httpRequest.MediaType.ShouldBeNull();
                httpRequest.Method.ShouldBe(HttpMethod.GET.ToString());
                httpRequest.Pipelined.ShouldBeTrue();
                httpRequest.PreAuthenticate.ShouldBeFalse();
                httpRequest.ProtocolVersion.Major.ShouldBe(1);
                httpRequest.ProtocolVersion.Minor.ShouldBe(1);
                httpRequest.ReadWriteTimeout.ShouldBe(300000);
                httpRequest.Referer.ShouldBeNull();
                httpRequest.SendChunked.ShouldBeFalse();
                httpRequest.Timeout.ShouldBe(100000);
                httpRequest.TransferEncoding.ShouldBeNull();
                httpRequest.UnsafeAuthenticatedConnectionSharing.ShouldBeFalse();
                httpRequest.UseDefaultCredentials.ShouldBeFalse();
                httpRequest.UserAgent.ShouldBeNull();
            }
        }

        public class ResourceProperty
        {
            [Fact]
            public void DoesNotClearPathParamsWhenResourceDoesNotChange()
            {
                const string resource = "/request/path";
                var pathparams = Substitute.For<PathParams>();
                var request = new RestRequest(resource) {PathParams = pathparams};

                request.Resource = resource;

                pathparams.DidNotReceive().Clear();
            }
        }
    }
}
