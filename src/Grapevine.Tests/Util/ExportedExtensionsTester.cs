using System;
using System.Collections.Specialized;
using Grapevine.Util;
using Xunit;
using Shouldly;

namespace Grapevine.Tests.Util
{
    public class ExportedExtensionsTester
    {
        [Fact]
        public void get_value_throws_exception_when_collection_is_null()
        {
            Action<NameValueCollection> func = valueCollection => valueCollection.GetValue<string>("key");
            Should.Throw<ArgumentNullException>(() => func(null));
        }

        [Fact]
        public void get_value_throws_exception_when_key_is_null()
        {
            var collection = new NameValueCollection();
            Should.Throw<ArgumentNullException>(() => collection.GetValue<string>(null));
        }

        [Fact]
        public void get_value_throws_exception_when_key_is_not_found()
        {
            var collection = new NameValueCollection();
            Should.Throw<ArgumentOutOfRangeException>(() => collection.GetValue<string>("key"));
        }

        [Fact]
        public void get_value_throws_exception_when_value_can_not_convert()
        {
            var collection = new NameValueCollection();
            collection.Add("key", "value");
            collection.GetValue<string>("key").ShouldBe("value");
            Should.Throw<ArgumentException>(() => collection.GetValue<NoCoversion>("key"));
        }

        [Fact]
        public void get_value_converts_to_bool()
        {
            var collection = new NameValueCollection { { "IsTrue", "true" }, { "IsFalse", "FALSE" } };

            collection.GetValue<bool>("IsTrue").ShouldBeTrue();
            collection.GetValue<bool>("IsFalse").ShouldBeFalse();
        }

        [Fact]
        public void get_value_or_default_returns_default()
        {
            var collection = new NameValueCollection { { "IsTrue", "true" } };

            collection.GetValue<bool>("IsTrue", false).ShouldBeTrue();
            collection.GetValue<bool>("IsFalse", true).ShouldBeTrue();
        }
    }

    public class NoCoversion {}
}
