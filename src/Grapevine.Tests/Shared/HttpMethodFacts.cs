using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class HttpMethodFacts
    {
        public class ExtensionMethods
        {
            public class EquivalentMethod
            {
                [Fact]
                public void EquivalentIsFalseForDifferentHttpMethods()
                {
                    HttpMethod.POST.IsEquivalent(HttpMethod.GET).ShouldBeFalse();
                }

                [Fact]
                public void EquivalentIsTrueForSameHttpMethods()
                {
                    HttpMethod.POST.IsEquivalent(HttpMethod.POST).ShouldBeTrue();
                }

                [Fact]
                public void EquivalentIsAlwaysTrueForHttpMethod_ALL()
                {
                    HttpMethod.ALL.IsEquivalent(HttpMethod.GET).ShouldBeTrue();
                    HttpMethod.GET.IsEquivalent(HttpMethod.ALL).ShouldBeTrue();
                }
            }

            public class FromStringMethod
            {
                [Fact]
                public void ReturnsDefaultWhenNoMatchExists()
                {
                    HttpMethod.POST.FromString("NoMatch").ShouldBe(HttpMethod.ALL);
                }

                [Fact]
                public void ReturnsCorrectValue()
                {
                    HttpMethod.ALL.FromString("Get").ShouldBe(HttpMethod.GET);
                    HttpMethod.ALL.FromString("put").ShouldBe(HttpMethod.PUT);
                    HttpMethod.ALL.FromString("pOST").ShouldBe(HttpMethod.POST);
                    HttpMethod.ALL.FromString("DELETE").ShouldBe(HttpMethod.DELETE);
                }
            }
        }
    }
}
