using Grapevine.Client;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Client
{
    public class QueryStringFacts
    {
        public class StringifyMethod
        {
            [Fact]
            public void GeneratesQueryString()
            {
                var querystring = new QueryString { { "search", "things" } };

                var qs = querystring.Stringify();

                qs.ShouldBe("search=things");
            }

            [Fact]
            public void GeneratesQueryStringWithAmpersand()
            {
                var querystring = new QueryString { { "search", "things" }, { "more", "stuff" } };

                var qs = querystring.Stringify();

                qs.Contains("search=things").ShouldBeTrue();
                qs.Contains("more=stuff").ShouldBeTrue();
                qs.Contains("&").ShouldBeTrue();
                qs.Contains("thingsmore").ShouldBeFalse();
                qs.Contains("stuffsearch").ShouldBeFalse();
            }

            [Fact]
            public void UriEncodesQueryString()
            {
                var querystring = new QueryString {{"My Long Key", "My Long Value"}};

                var qs = querystring.Stringify();

                qs.ShouldBe("My%20Long%20Key=My%20Long%20Value");
            }

            [Fact]
            public void ReturnsEmptyStringWhenCollectionIsEempty()
            {
                var querystring = new QueryString {{"key", "value"}};
                querystring.Remove("key");

                var qs = querystring.Stringify();

                qs.ShouldBe(string.Empty);
            }
        }
    }
}
