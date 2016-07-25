using Grapevine.Client;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Client
{
    public class RestClientTester
    {
        [Fact]
        public void rest_client_ctor_strips_trailing_slash()
        {
            const string baseurl1 = "http://localhost:1234/";
            const string baseurl2 = "http://localhost:1234";

            var client = new RestClient(baseurl1);

            client.BaseUrl.ShouldBe(baseurl2);
            client.Cookies.ShouldNotBeNull();
            client.Credentials.ShouldBeNull();
        }
    }
}
