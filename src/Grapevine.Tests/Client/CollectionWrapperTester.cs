using Grapevine.Client;
using Grapevine.Client.Exceptions;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Client
{
    public class CollectionWrapperTester
    {
        [Fact]
        public void query_string_generates_query_string()
        {
            var querystring = new QueryString();
            querystring.Add("search", "things");

            var qs = querystring.ToString();

            qs.ShouldBe("search=things");
        }

        [Fact]
        public void query_string_generates_query_string_with_ampersand()
        {
            var querystring = new QueryString();
            querystring.Add("search", "things");
            querystring.Add("more", "stuff");

            var qs = querystring.ToString();

            qs.Contains("search=things").ShouldBeTrue();
            qs.Contains("more=stuff").ShouldBeTrue();
            qs.Contains("&").ShouldBeTrue();
            qs.Contains("thingsmore").ShouldBeFalse();
            qs.Contains("stuffsearch").ShouldBeFalse();
        }

        [Fact]
        public void query_string_generates_uri_encoded_query_string()
        {
            var querystring = new QueryString();
            querystring.Add("My Long Key", "My Long Value");

            var qs = querystring.ToString();

            qs.ShouldBe("My%20Long%20Key=My%20Long%20Value");
        }

        [Fact]
        public void query_string_returns_empty_string_when_collection_is_empty()
        {
            var querystring = new QueryString();
            querystring.Add("key", "value");
            querystring.Remove("key");

            var qs = querystring.ToString();

            qs.ShouldBe(string.Empty);
        }

        [Fact]
        public void path_params_parses_resource_string()
        {
            var resource = "user/[lastname]/[firstname]";
            var pathparams = new PathParams();
            pathparams.Add("firstname", "John");
            pathparams.Add("lastname", "Doe");

            var path = pathparams.ParseResource(resource);

            path.ShouldBe("user/Doe/John");
        }

        [Fact]
        public void path_params_does_not_modify_resource_when_no_params_exist_in_resource()
        {
            var resource = "users/Doe/John";
            var pathparams = new PathParams();
            pathparams.Add("key", "value");

            var path = pathparams.ParseResource(resource);

            path.ShouldBe(resource);
        }

        [Fact]
        public void path_params_throws_exception_when_not_all_placeholders_are_parses()
        {
            var resource = "user/[lastname]/[firstname]";
            var pathparams = new PathParams();
            pathparams.Add("firstname", "John");

            var exception = Record.Exception(() => pathparams.ParseResource(resource));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<ClientStateException>();
            exception.Message.ShouldBe("Not all parameters were replaced in request resource: user/[lastname]/John");
        }
    }
}
