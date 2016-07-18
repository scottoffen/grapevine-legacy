using Grapevine.Util;
using Xunit;
using Shouldly;

namespace Grapevine.Test.Util
{
    public class HttpMethodTester
    {
        [Fact]
        public void http_method_different_methods_are_not_equivalent()
        {
            HttpMethod.POST.IsEquivalent(HttpMethod.GET).ShouldBeFalse();
        }

        [Fact]
        public void http_method_same_methods_are_equivalent()
        {
            HttpMethod.POST.IsEquivalent(HttpMethod.POST).ShouldBeTrue();
        }

        [Fact]
        public void http_method_all_is_equivalent()
        {
            HttpMethod.ALL.IsEquivalent(HttpMethod.GET).ShouldBeTrue();
            HttpMethod.GET.IsEquivalent(HttpMethod.ALL).ShouldBeTrue();
        }
    }
}
