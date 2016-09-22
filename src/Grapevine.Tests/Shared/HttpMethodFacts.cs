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
        }
    }
}
