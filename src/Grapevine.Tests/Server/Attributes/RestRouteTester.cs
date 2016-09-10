using System;
using System.Linq;
using System.Reflection;
using Grapevine.Server.Attributes;
using Grapevine.Server.Exceptions;
using Grapevine.Tests.Server.Attributes.Helpers;
using Grapevine.Util;
using Rhino.Mocks;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server.Attributes
{
    public class RestRouteTester
    {
        public RestRouteTester()
        {
            Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add(
                typeof(System.Security.Permissions.ReflectionPermissionAttribute));
            Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add(
                typeof(System.Security.Permissions.PermissionSetAttribute));
        }

        [Fact]
        public void rest_route_returns_empty_list_when_attribute_not_present()
        {
            var attributes = typeof(TestClass).GetMethod("IsEligibleButNoAttribute").GetRouteAttributes();
            attributes.ShouldNotBeNull();
            attributes.Count().ShouldBe(0);
        }

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
        public void rest_route_pathinfo_arg_only()
        {
            var method = typeof(RestRouteTesterHelper).GetMethod("RouteHasPathInfoOnly");
            var attrs = method.GetRouteAttributes().ToList();

            attrs.Count.ShouldBe(1);
            attrs[0].HttpMethod.ShouldBe(HttpMethod.ALL);
            attrs[0].PathInfo.Equals("/some/path").ShouldBeTrue();
        }

        [Fact]
        public void rest_route_both_args()
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

        [Fact]
        public void has_parameterless_constructor_returns_correct_value()
        {
            typeof(ImplicitConstructor).HasParameterlessConstructor().ShouldBeTrue();
            typeof(ExplicitConstructor).HasParameterlessConstructor().ShouldBeTrue();
            typeof(MultipleConstructor).HasParameterlessConstructor().ShouldBeTrue();
            typeof(NoParameterlessConstructor).HasParameterlessConstructor().ShouldBeFalse();
        }

        [Fact]
        public void can_invoke_returns_true_for_static_methods()
        {
            typeof(TestClass).GetMethod("TestStaticMethod").CanInvoke().ShouldBeTrue();
        }

        [Fact]
        public void can_invoke_returns_false_when_method_is_abstract()
        {
            typeof(TestAbstract).GetMethod("TestAbstractMethod").CanInvoke().ShouldBeFalse();
        }

        [Fact]
        public void can_invoke_returns_false_when_reflectedtype_is_null()
        {
            var method = MockRepository.Mock<MethodInfo>();
            method.Stub(m => m.ReflectedType).Return(null);

            method.IsStatic.ShouldBeFalse();
            method.IsAbstract.ShouldBeFalse();
            method.CanInvoke().ShouldBeFalse();
        }

        [Fact]
        public void can_invoke_returns_false_when_reflected_type_is_not_a_class()
        {
            typeof(TestInterface).GetMethod("TestInterfaceMethod").CanInvoke().ShouldBeFalse();
            typeof(TestStruct).GetMethod("TestStructMethod").CanInvoke().ShouldBeFalse();
        }

        [Fact]
        public void can_invoke_returns_false_when_reflected_type_is_abstract()
        {
            typeof(TestAbstract).GetMethod("TestVirtualMethod").CanInvoke().ShouldBeFalse();
        }

        [Fact]
        public void can_invoke_returns_true_on_invokable_method()
        {
            typeof(TestClass).GetMethod("TestMethod").CanInvoke().ShouldBeTrue();
        }

        [Fact]
        public void is_rr_eligible_returns_false_when_methodinfo_is_null()
        {
            MethodInfo method = null;
            Action<MethodInfo> action = info => info.IsRestRouteEligible();
            Should.Throw<ArgumentNullException>(() => action(method));
        }

        [Fact]
        public void is_rr_eligible_returns_false_when_methodinfo_is_not_invokable()
        {
            typeof(TestAbstract).GetMethod("TestAbstractMethod").IsRestRouteEligible().ShouldBeFalse();
        }

        [Fact]
        public void is_rr_eligible_returns_false_when_method_reflectedtype_has_no_parameterless_constructor()
        {
            typeof(NoParameterlessConstructor).GetMethod("TestMethod").IsRestRouteEligible().ShouldBeFalse();
        }

        [Fact]
        public void is_rr_eligible_returns_false_when_method_is_special_name()
        {
            typeof(TestClass).GetMethod("get_TestProperty").IsRestRouteEligible().ShouldBeFalse();
        }

        [Fact]
        public void is_rr_eligible_returns_false_when_return_type_is_not_ihttpcontext()
        {
            typeof(TestClass).GetMethod("HasNoReturnValue").IsRestRouteEligible().ShouldBeFalse();
            typeof(TestClass).GetMethod("ReturnValueIsWrongType").IsRestRouteEligible().ShouldBeFalse();
        }

        [Fact]
        public void is_rr_eligible_returns_false_when_method_accepts_more_or_less_than_one_argument()
        {
            typeof(TestClass).GetMethod("TakesZeroArgs").IsRestRouteEligible().ShouldBeFalse();
            typeof(TestClass).GetMethod("TakesTwoArgs").IsRestRouteEligible().ShouldBeFalse();
        }

        [Fact]
        public void is_rr_eligible_returns_false_when_first_argument_is_not_ihttpcontext()
        {
            typeof(TestClass).GetMethod("TakesWrongArgs").IsRestRouteEligible().ShouldBeFalse();
        }

        [Fact]
        public void is_rr_eligible_throws_aggregate_exception_when_method_is_not_eligible_and_throw_exceptions_is_true()
        {
            Should.Throw<InvalidRouteMethodExceptions>(
                () => typeof(TestClass).GetMethod("TakesWrongArgs").IsRestRouteEligible(true));
        }

        [Fact]
        public void is_rr_eligible_returns_true_when_method_is_eligible()
        {
            typeof(TestClass).GetMethod("ValidRoute").IsRestRouteEligible().ShouldBeTrue();
        }

        [Fact]
        public void is_rest_route_returns_false_when_method_is_not_eligible()
        {
            typeof(TestClass).GetMethod("HasAttributeButIsNotEligible").IsRestRoute().ShouldBeFalse();
        }

        [Fact]
        public void is_rest_route_returns_false_when_restroute_attribute_is_not_present()
        {
            typeof(TestClass).GetMethod("IsEligibleButNoAttribute").IsRestRoute().ShouldBeFalse();
        }

        [Fact]
        public void is_rest_route_throws_exception_when_false_and_throw_exceptions_is_true()
        {
            Should.Throw<InvalidRouteMethodExceptions>(() => typeof(TestClass).GetMethod("HasAttributeButIsNotEligible").IsRestRoute(true));
            Should.Throw<InvalidRouteMethodExceptions>(() => typeof(TestClass).GetMethod("IsEligibleButNoAttribute").IsRestRoute(true));
        }

        [Fact]
        public void is_rest_route_returns_true_when_attribute_is_present_and_method_is_eligible()
        {

        }
    }
}
