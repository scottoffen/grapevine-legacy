using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util
{
    public class UriSchemeExtensionsTester
    {
        [Fact]
        public void uri_schema_extension_returns_uri_as_lowercase_string()
        {
            UriScheme.Https.ToScheme().ShouldBe("https");
        }
    }
}
