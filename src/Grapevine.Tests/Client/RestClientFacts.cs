using System;
using System.Net;
using Grapevine.Client;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Client
{
    public class RestClientFacts
    {
        public class Constructors
        {
            [Fact]
            public void BaseInitialization()
            {
                const string baseurl = "http://localhost:1234/";

                var client = new RestClient { Host = "localhost", Port = 1234, Scheme = UriScheme.Http };

                client.BaseUrl.AbsoluteUri.ShouldBe(baseurl);
                client.Cookies.ShouldNotBeNull();
                client.Credentials.ShouldBeNull();
            }
        }

        public class Execute
        {
            [Fact]
            public void ClientRunsTimeoutDelegateOnRequestTimeoutIfSet()
            {
                var actionCalled = false;
                Action updateActionCalled = () => actionCalled = true;

                var client = new RestClient { RequestTimeoutAction = updateActionCalled, Host = "localhost", Port = 1234, Scheme = UriScheme.Http };

                var req = new RestRequest { Timeout = 1, ContentType = ContentType.MIME };

                client.Execute(req);

                actionCalled.ShouldBeTrue();
            }

            [Fact]
            public void ClientRethrowsExceptionIfNoTimeoutDelegateSetAndRequestTimesOut()
            {
                var client = new RestClient { Host = "localhost", Port = 1234, Scheme = UriScheme.Http };

                var req = new RestRequest { Timeout = 1, ContentType = ContentType.MIME };

                Should.Throw<WebException>(() => client.Execute(req));
            }
        }
    }
}
