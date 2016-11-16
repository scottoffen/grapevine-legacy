using System;
using System.Collections.Generic;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class DynamicPropertiesFact
    {
        public class Constructors
        {
            [Fact]
            public void PropertyNotInitializedUntilFirstAccess()
            {
                var props = new ObjectWithProperties();
                props.IsInitialized.ShouldBeFalse();

                props.Properties["Key"] = "value";

                props.IsInitialized.ShouldBeTrue();
            }
        }

        public class IndexAccessorMethods
        {
            [Fact]
            public void StoresAndReturnsValue()
            {
                var props = new ObjectWithProperties();
                props.Properties["Key"] = "value";
                var val = props.Properties["Key"];
                Assert.True(val.Equals("value"));
            }

            [Fact]
            public void ThrowsExceptionWhenIndexDoesNotExists()
            {
                var props = new ObjectWithProperties();
                Should.Throw<KeyNotFoundException>(() => { var x = props.Properties["Key"]; });
            }
        }

        public class ExtensionMethods
        {
            public class GetPropertyValueAsMethod
            {
                [Fact]
                public void ReturnsValueAsSpecifiedType()
                {
                    var props = new ObjectWithProperties();
                    props.Properties["Key"] = "value";

                    var val = props.GetPropertyValueAs<string>("Key");

                    val.Equals("value").ShouldBeTrue();
                    val.GetType().ShouldBe(typeof(string));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsNull()
                {
                    var props = new ObjectWithProperties();
                    props.Properties["Key"] = "value";

                    Should.Throw<ArgumentNullException>(() => props.GetPropertyValueAs<string>(null));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsEmptyString()
                {
                    var props = new ObjectWithProperties();
                    props.Properties["Key"] = "value";

                    Should.Throw<ArgumentException>(() => props.GetPropertyValueAs<string>(string.Empty));
                    Should.Throw<ArgumentException>(() => props.GetPropertyValueAs<string>(""));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsNotFound()
                {
                    var props = new ObjectWithProperties();
                    props.Properties["Key"] = "value";

                    Should.Throw<DynamicValueNotFoundException>(() => props.GetPropertyValueAs<string>("AnotheKey"));
                }

                [Fact]
                public void CovertsBetweenTypes()
                {
                    var props = new ObjectWithProperties();
                    props.Properties["MyNumber"] = 50;

                    var val = props.GetPropertyValueAs<string>("MyNumber");

                    val.GetType().ShouldBe(typeof(string));
                    val.Equals("50").ShouldBeTrue();
                }

                [Fact]
                public void ThrowsExcecptionWhenCanNotConvert()
                {
                    var props = new ObjectWithProperties();
                    props.Properties["Key"] = "value";
                    Should.Throw<InvalidCastException>(() => props.GetPropertyValueAs<FakeType>("Key"));
                }

            }

            public class ContainsPropertyMethod
            {
                [Fact]
                public void ThrowsExceptionWhenKeyIsNull()
                {
                    var props = new ObjectWithProperties();
                    Should.Throw<ArgumentNullException>(() => props.ContainsProperty(null));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsEmptyString()
                {
                    var props = new ObjectWithProperties();
                    Should.Throw<ArgumentException>(() => props.ContainsProperty(string.Empty));
                    Should.Throw<ArgumentException>(() => props.ContainsProperty(""));
                }

                [Fact]
                public void ReturnsTrueWhenPropertyExists()
                {
                    var props = new ObjectWithProperties();
                    props.Properties["Key"] = "value";
                    props.ContainsProperty("Key").ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenPropertyDoesNotExist()
                {
                    var props = new ObjectWithProperties();
                    props.ContainsProperty("Key1").ShouldBeFalse();
                }
            }
        }
    }

    public class ObjectWithProperties : DynamicProperties
    {
        private static FieldInfo _fieldInfo;

        public ObjectWithProperties()
        {
            if (_fieldInfo != null) return;
            var memberInfo = GetType().BaseType;
            _fieldInfo = memberInfo?.GetField("_properties", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public bool IsInitialized => _fieldInfo?.GetValue(this) != null;
    }

    public class FakeType { }
}
