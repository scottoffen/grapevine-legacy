using System;
using System.Collections.Specialized;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class NameValueCollectionFacts
    {
        public class ExtensionMethods
        {
            public class GetValueMethod
            {
                [Fact]
                public void ThrowsExceptionWhenCollectionIsNull()
                {
                    Action<NameValueCollection> func = valueCollection => valueCollection.GetValue<string>("key");
                    Should.Throw<ArgumentNullException>(() => func(null));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsNull()
                {
                    var collection = new NameValueCollection();
                    Should.Throw<ArgumentNullException>(() => collection.GetValue<string>(null));
                }

                [Fact]
                public void ThrowsExceptionWhenKeyIsNotFound()
                {
                    var collection = new NameValueCollection();
                    Should.Throw<ArgumentOutOfRangeException>(() => collection.GetValue<string>("key"));
                }

                [Fact]
                public void ThrowsExceptionWhenValueCanNotConvert()
                {
                    var collection = new NameValueCollection();
                    collection.Add("key", "value");
                    collection.GetValue<string>("key").ShouldBe("value");
                    Should.Throw<ArgumentException>(() => collection.GetValue<NoCoversion>("key"));
                }

                [Fact]
                public void ConvertsToType()
                {
                    var collection = new NameValueCollection { { "IsTrue", "true" }, { "IsFalse", "FALSE" } };

                    collection.GetValue<bool>("IsTrue").ShouldBeTrue();
                    collection.GetValue<bool>("IsFalse").ShouldBeFalse();
                }
            }

            public class GetValueOrDefaultMethod
            {
                [Fact]
                public void ReturnsDefault()
                {
                    var collection = new NameValueCollection { { "IsTrue", "true" } };

                    collection.GetValue<bool>("IsTrue", false).ShouldBeTrue();
                    collection.GetValue<bool>("IsFalse", true).ShouldBeTrue();
                }
            }
        }

        public class NoCoversion { }
    }
}
