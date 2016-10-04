using System;
using System.Reflection;
using Grapevine.Exceptions.Server;
using Grapevine.Shared;
using Microsoft.CSharp.RuntimeBinder;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class DynamicPropertiesFact
    {
        public class ConstructorMethod
        {
            [Fact]
            public void DynamicPropertyNotInitializedUntilFirstAccess()
            {
                var props = new ConcreteDynmProp();
                props.IsInitialized.ShouldBeFalse();

                props.Properties.Key = "value";

                props.IsInitialized.ShouldBeTrue();
            }
        }

        public class IndexAccessorMethods
        {
            [Fact]
            public void StoresAndReturnsValue()
            {
                var props = new ConcreteDynmProp();
                props.Properties.Key = "value";
                var val = props.Properties.Key;
                Assert.True(val.Equals("value"));
            }

            [Fact]
            public void ThrowsExceptionWhenIndexDoesNotExists()
            {
                var props = new ConcreteDynmProp();
                Should.Throw<RuntimeBinderException>(() => props.Properties.Key);
            }
        }

        public class ExtensionMethods
        {
            public class GetPropertyValueAsMethod
            {
                [Fact]
                public void ReturnsValueAsSpecifiedType()
                {
                    var props = new ConcreteDynmProp();
                    props.Properties.Key = "value";

                    var val = props.GetPropertyValueAs<string>("Key");

                    val.Equals("value").ShouldBeTrue();
                    val.GetType().ShouldBe(typeof(string));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsNull()
                {
                    var props = new ConcreteDynmProp();
                    props.Properties.Key = "value";

                    Should.Throw<ArgumentNullException>(() => props.GetPropertyValueAs<string>(null));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsEmptyString()
                {
                    var props = new ConcreteDynmProp();
                    props.Properties.Key = "value";

                    Should.Throw<ArgumentException>(() => props.GetPropertyValueAs<string>(string.Empty));
                    Should.Throw<ArgumentException>(() => props.GetPropertyValueAs<string>(""));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsNotFound()
                {
                    var props = new ConcreteDynmProp();
                    props.Properties.Key = "value";

                    Should.Throw<DynamicValueNotFoundException>(() => props.GetPropertyValueAs<string>("AnotheKey"));
                }

                [Fact]
                public void CovertsBetweenTypes()
                {
                    var props = new ConcreteDynmProp();
                    props.Properties.MyNumber = 50;

                    var val = props.GetPropertyValueAs<string>("MyNumber");

                    val.GetType().ShouldBe(typeof(string));
                    val.Equals("50").ShouldBeTrue();
                }

                [Fact]
                public void ThrowsExcecptionWhenCanNotConvert()
                {
                    var props = new ConcreteDynmProp();
                    props.Properties.Key = "value";
                    Should.Throw<InvalidCastException>(() => props.GetPropertyValueAs<FakeType>("Key"));
                }

            }

            public class ContainsPropertyMethod
            {
                [Fact]
                public void ThrowsExceptionWhenKeyIsNull()
                {
                    var props = new ConcreteDynmProp();
                    Should.Throw<ArgumentNullException>(() => props.ContainsProperty(null));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsEmptyString()
                {
                    var props = new ConcreteDynmProp();
                    Should.Throw<ArgumentException>(() => props.ContainsProperty(string.Empty));
                    Should.Throw<ArgumentException>(() => props.ContainsProperty(""));
                }

                [Fact]
                public void ReturnsTrueWhenPropertyExists()
                {
                    var props = new ConcreteDynmProp();
                    props.Properties.Key = "value";
                    props.ContainsProperty("Key").ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseWhenPropertyDoesNotExist()
                {
                    var props = new ConcreteDynmProp();
                    props.ContainsProperty("Key1").ShouldBeFalse();
                }
            }
        }
    }

    public class ConcreteDynmProp : DynamicProperties
    {
        private static FieldInfo _fieldInfo;

        public ConcreteDynmProp()
        {
            if (_fieldInfo != null) return;
            var memberInfo = GetType().BaseType;
            _fieldInfo = memberInfo?.GetField("_properties", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public bool IsInitialized => _fieldInfo?.GetValue(this) != null;
    }

    public class FakeType { }
}
