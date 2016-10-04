using System;
using System.Net;
using Grapevine.Client;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Client
{
    public class RestClientTester
    {
        //[Fact]
        //public void rest_client_ctor()
        //{
        //    const string baseurl2 = "http://localhost:1234/";

        //    var client = new RestClient { Host = "localhost", Port = 1234, Scheme = UriScheme.Http };

        //    client.BaseUrl.AbsoluteUri.ShouldBe(baseurl2);
        //    client.Cookies.ShouldNotBeNull();
        //    client.Credentials.ShouldBeNull();
        //}

        [Fact]
        public void client_runs_timeout_delegate_on_request_timeout_if_set()
        {
            var actionCalled = false;
            Action updateActionCalled = () => actionCalled = true;

            var client = new RestClient { RequestTimeoutAction = updateActionCalled, Host = "localhost", Port = 1234, Scheme = UriScheme.Http };

            var req = new RestRequest { Timeout = 1, ContentType = ContentType.MIME };

            client.Execute(req);

            actionCalled.ShouldBeTrue();
        }

        [Fact]
        public void client_rethrows_exception_if_no_timeout_delegate_set_and_request_times_out()
        {
            var client = new RestClient { Host = "localhost", Port = 1234, Scheme = UriScheme.Http };

            var req = new RestRequest { Timeout = 1, ContentType = ContentType.MIME };

            Should.Throw<WebException>(() => client.Execute(req));
        }
    }
}
