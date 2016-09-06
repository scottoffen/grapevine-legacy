using System.Linq;
using Grapevine.Server.Attributes;
using Grapevine.Tests.Server.Attributes.Helpers;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server.Attributes
{
    public class RestRouteTester
    {
        [Fact]
        public void rest_route_no_args_gets_default_properties()
        {
            var method = typeof(RestRouteTesterHelper).GetMethod("RouteHasNoArgs");
            var attrs = method.GetRouteAttributes().ToList();

            attrs.Count.ShouldBe(1);
            attrs[0].HttpMethod.ShouldBe(HttpMethod.ALL);
            attrs[0].PathInfo.Equals(string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void rest_route_httpmethod_arg_only()
        {
            var method = typeof(RestRouteTesterHelper).GetMethod("RouteHasHttpMethodOnly");
            var attrs = method.GetRouteAttributes().ToList();

            attrs.Count.ShouldBe(1);
            attrs[0].HttpMethod.ShouldBe(HttpMethod.DELETE);
            attrs[0].PathInfo.Equals(string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void rest_route_pathinfo_parameter_only()
        {
            var method = typeof(RestRouteTesterHelper).GetMethod("RouteHasPathInfoOnly");
            var attrs = method.GetRouteAttributes().ToList();

            attrs.Count.ShouldBe(1);
            attrs[0].HttpMethod.ShouldBe(HttpMethod.ALL);
            attrs[0].PathInfo.Equals("/some/path").ShouldBeTrue();
        }

        [Fact]
        public void rest_route_both_parameters()
        {
            var method = typeof(RestRouteTesterHelper).GetMethod("RouteHasBothArgs");
            var attrs = method.GetRouteAttributes().ToList();

            attrs.Count.ShouldBe(1);
            attrs[0].HttpMethod.ShouldBe(HttpMethod.POST);
            attrs[0].PathInfo.Equals("/some/other/path").ShouldBeTrue();
        }

        [Fact]
        public void rest_route_multiple_attributes()
        {
            var method = typeof(RestRouteTesterHelper).GetMethod("RouteHasMultipleAttrs");
            var attrs = method.GetRouteAttributes().ToList();

            attrs.Count.ShouldBe(2);
            attrs[0].HttpMethod.ShouldBe(HttpMethod.GET);
            attrs[0].PathInfo.Equals("/index.html").ShouldBeTrue();
            attrs[1].HttpMethod.ShouldBe(HttpMethod.HEAD);
            attrs[1].PathInfo.Equals("/index").ShouldBeTrue();
        }

    }
}
