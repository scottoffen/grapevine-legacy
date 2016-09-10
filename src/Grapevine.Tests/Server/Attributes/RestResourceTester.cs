using Grapevine.Server.Attributes;
using Grapevine.Tests.Server.Attributes.Helpers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server.Attributes
{
    public class RestResourceTester
    {
        [Fact]
        public void is_rest_resource_returns_false_when_type_is_not_class()
        {
            typeof(RestResourceInterface).IsRestResource().ShouldBeFalse();
        }

        [Fact]
        public void is_rest_resource_returns_false_when_type_is_abstract()
        {
            typeof(AbstractRestResource).IsRestResource().ShouldBeFalse();
        }

        [Fact]
        public void is_rest_resource_returns_false_when_attribute_is_not_present()
        {
            typeof(NotARestResource).IsRestResource().ShouldBeFalse();
        }

        [Fact]
        public void is_rest_resource_returns_true_class_is_valid_rest_resource()
        {
            typeof(RestResourceNoArgs).IsRestResource().ShouldBeTrue();
        }

        [Fact]
        public void rest_resource_returns_null_when_attribute_is_not_present()
        {
            var attributes = typeof(NotARestResource).GetRestResource();
            attributes.ShouldBeNull();
        }

        [Fact]
        public void rest_resource_no_args_gets_default_properties()
        {
            var attributes = typeof(RestResourceNoArgs).GetRestResource();
            attributes.ShouldNotBeNull();
            attributes.BasePath.Equals(string.Empty).ShouldBeTrue();
            attributes.Scope.Equals(string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void rest_resource_basepath_arg_only()
        {
            var attributes = typeof(RestResourceBasePathOnly).GetRestResource();
            attributes.ShouldNotBeNull();
            attributes.BasePath.Equals("/test/path").ShouldBeTrue();
            attributes.Scope.Equals(string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void rest_resource_scope_arg_only()
        {
            var attributes = typeof(RestResourceScopeOnly).GetRestResource();
            attributes.ShouldNotBeNull();
            attributes.BasePath.Equals(string.Empty).ShouldBeTrue();
            attributes.Scope.Equals("TestScope").ShouldBeTrue();
        }

        [Fact]
        public void rest_resource_both_args()
        {
            var attributes = typeof(RestResourceBothArgs).GetRestResource();
            attributes.ShouldNotBeNull();
            attributes.BasePath.Equals("/api").ShouldBeTrue();
            attributes.Scope.Equals("ApiScope").ShouldBeTrue();
        }
    }
}