using System.Linq;
using System.Reflection;
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

        [Fact]
        public void reflected_and_declaring_types_for_methods_on_interface()
        {
            /* Interface Reflected and Declaring Types */
            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").ReflectedType.ShouldBe(typeof(TypicalInterface));
            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").ReflectedType.IsInterface.ShouldBeTrue();
            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").ReflectedType.IsClass.ShouldBeFalse();

            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").DeclaringType.ShouldBe(typeof(TypicalInterface));
            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").DeclaringType.IsInterface.ShouldBeTrue();
            typeof(TypicalInterface).GetMethod("TypicalInterfaceMethod").DeclaringType.IsClass.ShouldBeFalse();
        }

        [Fact]
        public void reflected_and_declaring_types_for_methods_on_abstract_class()
        {
            /* Abstract Method in Abstract Class Reflected and Declaring Types */
            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.ShouldBe(typeof(TypicalAbstractClass));
            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.ShouldBe(typeof(TypicalAbstractClass));
            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.IsClass.ShouldBeTrue();

            /* Non-Abstract Method in Abstract Class Reflected and Declaring Types */
            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.ShouldBe(typeof(TypicalAbstractClass));
            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.ShouldBe(typeof(TypicalAbstractClass));
            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.IsClass.ShouldBeTrue();
        }

        [Fact]
        public void reflected_and_declaring_types_for_methods_on_nested_abstract_class()
        {
            /* Abstract Method in Abstract Class Reflected and Declaring Types */
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.ShouldBe(typeof(TypicalAbstractClass.TypicalNestedAbstractClass));
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.ShouldBe(typeof(TypicalAbstractClass.TypicalNestedAbstractClass));
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalAbstractMethod").DeclaringType.IsClass.ShouldBeTrue();

            /* Non-Abstract Method in Abstract Class Reflected and Declaring Types */
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.ShouldBe(typeof(TypicalAbstractClass.TypicalNestedAbstractClass));
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.ShouldBe(typeof(TypicalAbstractClass.TypicalNestedAbstractClass));
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractClass.TypicalNestedAbstractClass).GetMethod("TypicalNonAbstractMethod").DeclaringType.IsClass.ShouldBeTrue();
        }

        [Fact]
        public void reflected_and_declaring_types_for_methods_on_abstract_concretion_class()
        {
            /* Abstract Method in Abstract Class Reflected and Declaring Types */
            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").ReflectedType.ShouldBe(typeof(TypicalAbstractConcretion));
            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").ReflectedType.IsAbstract.ShouldBeFalse();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").DeclaringType.ShouldBe(typeof(TypicalAbstractConcretion));
            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").DeclaringType.IsAbstract.ShouldBeFalse();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalMethod").DeclaringType.IsClass.ShouldBeTrue();

            /* Abstract Method in Abstract Class Reflected and Declaring Types */
            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").ReflectedType.ShouldBe(typeof(TypicalAbstractConcretion));
            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").ReflectedType.IsAbstract.ShouldBeFalse();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").DeclaringType.ShouldBe(typeof(TypicalAbstractConcretion));
            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").DeclaringType.IsAbstract.ShouldBeFalse();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalAbstractMethod").DeclaringType.IsClass.ShouldBeTrue();

            /* Non-Abstract Method in Abstract Class Reflected and Declaring Types */
            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").ReflectedType.ShouldBe(typeof(TypicalAbstractConcretion));
            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").ReflectedType.IsAbstract.ShouldBeFalse();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").DeclaringType.ShouldBe(typeof(TypicalAbstractClass));
            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").DeclaringType.IsAbstract.ShouldBeTrue();
            typeof(TypicalAbstractConcretion).GetMethod("TypicalNonAbstractMethod").DeclaringType.IsClass.ShouldBeTrue();
        }

        [Fact]
        public void reflected_and_declaring_types_for_methods_on_class()
        {
            /* Method in Class Reflected and Declaring Types */
            typeof(TypicalClass).GetMethod("TypicalMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalClass).GetMethod("TypicalMethod").ReflectedType.ShouldBe(typeof(TypicalClass));
            typeof(TypicalClass).GetMethod("TypicalMethod").ReflectedType.IsInterface.ShouldBeFalse();
            typeof(TypicalClass).GetMethod("TypicalMethod").ReflectedType.IsAbstract.ShouldBeFalse();
            typeof(TypicalClass).GetMethod("TypicalMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalClass).GetMethod("TypicalMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalClass).GetMethod("TypicalMethod").DeclaringType.ShouldBe(typeof(TypicalClass));
            typeof(TypicalClass).GetMethod("TypicalMethod").DeclaringType.IsInterface.ShouldBeFalse();
            typeof(TypicalClass).GetMethod("TypicalMethod").DeclaringType.IsAbstract.ShouldBeFalse();
            typeof(TypicalClass).GetMethod("TypicalMethod").DeclaringType.IsClass.ShouldBeTrue();
        }

        [Fact]
        public void reflected_and_declaring_types_for_methods_on_class_implementing_interface()
        {
            /* Method in Class Reflected and Declaring Types */
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").ReflectedType.ShouldBe(typeof(TypicalInterfaceConcretion));
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").ReflectedType.IsInterface.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").ReflectedType.IsAbstract.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").DeclaringType.ShouldBe(typeof(TypicalInterfaceConcretion));
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").DeclaringType.IsInterface.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").DeclaringType.IsAbstract.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalMethod").DeclaringType.IsClass.ShouldBeTrue();

            /* Method in Class Reflected and Declaring Types */
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").ReflectedType.ShouldNotBeNull();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").ReflectedType.ShouldBe(typeof(TypicalInterfaceConcretion));
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").ReflectedType.IsInterface.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").ReflectedType.IsAbstract.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").ReflectedType.IsClass.ShouldBeTrue();

            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").DeclaringType.ShouldNotBeNull();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").DeclaringType.ShouldBe(typeof(TypicalInterfaceConcretion));
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").DeclaringType.IsInterface.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").DeclaringType.IsAbstract.ShouldBeFalse();
            typeof(TypicalInterfaceConcretion).GetMethod("TypicalInterfaceMethod").DeclaringType.IsClass.ShouldBeTrue();
        }


        [Fact]
        public void can_invoke_returns_false_when_class_is_abstract()
        {
            //typeof(AbstractRoutes).ReflectedType.ShouldBeNull();
            //typeof(AbstractRoutes).GetMethod("NonAbstractMethod").CanInvoke().ShouldBeFalse();
        }

        //[Fact]
        //public void rest_route_ignores_abstract_methods()
        //{
        //    typeof(AbstractRoutes).GetMethod("AbstractMethod").IsRestRoute().ShouldBeFalse();
        //}

        //[Fact]
        //public void rest_route_ignores_methods_with_special_names()
        //{
        //    typeof(OneValidRoute).GetMethod("get_Stuff").IsRestRoute().ShouldBeFalse();
        //    typeof(OneValidRoute).GetMethod("set_Stuff").IsRestRoute().ShouldBeFalse();
        //}

        //[Fact]
        //public void rest_route_ignores_non_final_methods()
        //{
        //    typeof(AbstractRoutes).GetMethod("InheritedMethod").IsRestRoute().ShouldBeFalse();
        //    //typeof(OneValidRoute).GetMethod("InheritedMethod").IsRestRoute().ShouldBeTrue();
        //}

        //[Fact]
        //public void rest_route_is_rest_route_returns_true_on_valid_route()
        //{
        //    typeof(OneValidRoute).GetMethod("TheValidRoute").IsRestRoute().ShouldBeTrue();
        //}
    }
}
