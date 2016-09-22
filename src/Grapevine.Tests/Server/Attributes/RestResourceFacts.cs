using Grapevine.Server.Attributes;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server.Attributes
{
    public class RestResourceFacts
    {
        public class ExtensionMethods
        {
            public class IsRestResourceMethod
            {
                [Fact]
                public void ReturnsFalseWhenTypeIsNotClass()
                {
                    typeof(RestResourceInterface).IsRestResource().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenTypeIsAbstract()
                {
                    typeof(AbstractRestResource).IsRestResource().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsFalseWhenAttributeIsNotPresent()
                {
                    typeof(NotARestResource).IsRestResource().ShouldBeFalse();
                }

                [Fact]
                public void ReturnsTrueWhenClassIsValidRestResource()
                {
                    typeof(RestResourceNoArgs).IsRestResource().ShouldBeTrue();
                }
            }

            public class GetRestResourceMethod
            {
                [Fact]
                public void ReturnsNullWhenAttributeIsNotPresent()
                {
                    var attributes = typeof(NotARestResource).GetRestResource();
                    attributes.ShouldBeNull();
                }

                [Fact]
                public void ReturnsDefaultProperties()
                {
                    var attributes = typeof(RestResourceNoArgs).GetRestResource();
                    attributes.ShouldNotBeNull();
                    attributes.BasePath.Equals(string.Empty).ShouldBeTrue();
                    attributes.Scope.Equals(string.Empty).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsSpecifiedBasePath()
                {
                    var attributes = typeof(RestResourceBasePathOnly).GetRestResource();
                    attributes.ShouldNotBeNull();
                    attributes.BasePath.Equals("/test/path").ShouldBeTrue();
                    attributes.Scope.Equals(string.Empty).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsSpecifiedScope()
                {
                    var attributes = typeof(RestResourceScopeOnly).GetRestResource();
                    attributes.ShouldNotBeNull();
                    attributes.BasePath.Equals(string.Empty).ShouldBeTrue();
                    attributes.Scope.Equals("TestScope").ShouldBeTrue();
                }

                [Fact]
                public void ReturnsSpecifiedBasePathAndScope()
                {
                    var attributes = typeof(RestResourceBothArgs).GetRestResource();
                    attributes.ShouldNotBeNull();
                    attributes.BasePath.Equals("/api").ShouldBeTrue();
                    attributes.Scope.Equals("ApiScope").ShouldBeTrue();
                }
            }
        }
    }

    /* Classes and methods used in testing */

    public class NotARestResource { /* class body intentionally left blank */ }

    [RestResource]
    public class RestResourceNoArgs { /* class body intentionally left blank */ }

    [RestResource(BasePath = "/test/path")]
    public class RestResourceBasePathOnly { /* class body intentionally left blank */ }

    [RestResource(Scope = "TestScope")]
    public class RestResourceScopeOnly { /* class body intentionally left blank */ }

    [RestResource(BasePath = "/api", Scope = "ApiScope")]
    public class RestResourceBothArgs { /* class body intentionally left blank */ }

    public interface RestResourceInterface { /* interface body intentionally left blank */ }

    public abstract class AbstractRestResource { /* class body intentionally left blank */ }
}
