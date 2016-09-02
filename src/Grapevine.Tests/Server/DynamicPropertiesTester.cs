using System;
using System.Reflection;
using Grapevine.Server;
using Grapevine.Server.Exceptions;
using Shouldly;
using Xunit;
using Microsoft.CSharp.RuntimeBinder;

namespace Grapevine.Tests.Server
{
    public class DynamicPropertiesTester
    {
        [Fact]
        public void dynamic_property_not_initialized_until_first_access()
        {
            var props = new DynaProps();
            props.IsInitialized.ShouldBeFalse();

            props.Properties.Key = "value";

            props.IsInitialized.ShouldBeTrue();
        }

        [Fact]
        public void dynamic_returns_stored_values()
        {
            var props = new DynaProps();
            props.Properties.Key = "value";
            var val = props.Properties.Key;
            Assert.True(val.Equals("value"));
        }

        [Fact]
        public void dynamic_throws_exception_for_missing_values()
        {
            var props = new DynaProps();
            Should.Throw<RuntimeBinderException>(() => props.Properties.Key);
        }

        [Fact]
        public void dynamic_get_property_value_returns_value_as_specified_type()
        {
            var props = new DynaProps();
            props.Properties.Key = "value";
            var val = props.GetPropertyValueAs<string>("Key");
            val.Equals("value").ShouldBeTrue();
            val.GetType().ShouldBe(typeof(string));
        }

        [Fact]
        public void dynamic_get_property_value_throws_exception_if_key_not_provided()
        {
            var props = new DynaProps();
            props.Properties.Key = "value";
            Should.Throw<ArgumentNullException>(() => props.GetPropertyValueAs<string>(null));
            Should.Throw<ArgumentNullException>(() => props.GetPropertyValueAs<string>(string.Empty));
            Should.Throw<ArgumentNullException>(() => props.GetPropertyValueAs<string>(""));
        }

        [Fact]
        public void dynamic_get_property_value_throws_exception_if_key_not_found()
        {
            var props = new DynaProps();
            props.Properties.Key = "value";
            Should.Throw<DynamicValueNotFoundException>(() => props.GetPropertyValueAs<string>("AnotheKey"));
        }

        [Fact]
        public void dynamic_get_property_value_can_covert_between_types()
        {
            var props = new DynaProps();
            props.Properties.MyNumber = 50;
            var val = props.GetPropertyValueAs<string>("MyNumber");
            val.GetType().ShouldBe(typeof(string));
            val.Equals("50").ShouldBeTrue();
        }

        [Fact]
        public void dynamic_get_property_value_throws_excecption_if_can_not_convert()
        {
            var props = new DynaProps();
            props.Properties.Key = "value";
            Should.Throw<InvalidCastException>(() => props.GetPropertyValueAs<FakeType>("Key"));
        }

        [Fact]
        public void dynamic_contains_property_throws_exception_if_key_not_provided()
        {
            var props = new DynaProps();
            Should.Throw<ArgumentNullException>(() => props.ContainsProperty(null));
            Should.Throw<ArgumentNullException>(() => props.ContainsProperty(string.Empty));
            Should.Throw<ArgumentNullException>(() => props.ContainsProperty(""));
        }

        [Fact]
        public void dynamic_contains_property_returns_true_if_property_exists()
        {
            var props = new DynaProps();
            props.Properties.Key = "value";
            props.ContainsProperty("Key").ShouldBeTrue();
        }

        [Fact]
        public void dynamic_contains_property_returns_false_if_property_does_not_exist()
        {
            var props = new DynaProps();
            props.ContainsProperty("Key1").ShouldBeFalse();
        }
    }

    public class DynaProps : DynamicProperties
    {
        public bool IsInitialized
        {
            get
            {
                var memberInfo = GetType().BaseType;
                var field = memberInfo?.GetField("_properties", BindingFlags.Instance | BindingFlags.NonPublic);
                return field?.GetValue(this) != null;
            }
        }
    }

    public class FakeType{ }
}
