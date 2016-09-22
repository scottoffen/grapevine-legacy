using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class UriSchemeFacts
    {
        public class ExtensionMethods
        {
            public class ToSchemeMethod
            {
                [Fact]
                public void ReturnsSchemeAsLowercaseString()
                {
                    UriScheme.Https.ToScheme().ShouldBe("https");
                }
            }
        }
    }
}
