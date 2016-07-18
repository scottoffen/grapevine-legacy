using System.Collections.Specialized;
using Grapevine.Util;
using Xunit;
using Shouldly;

namespace Grapevine.Test.Util
{
    public class ExternalExtensionsTester
    {
        [Fact]
        public void get_value_converts_to_bool()
        {
            var collection = new NameValueCollection {{"IsTrue", "true"}, {"IsFalse", "FALSE"}};

            collection.GetValue<bool>("IsTrue").ShouldBeTrue();
            collection.GetValue<bool>("IsFalse").ShouldBeFalse();
        }

        [Fact]
        public void get_value_or_default_returns_default()
        {
            var collection = new NameValueCollection {{"IsTrue", "true"}};

            collection.GetValue<bool>("IsTrue", false).ShouldBeTrue();
            collection.GetValue<bool>("IsFalse", true).ShouldBeTrue();
        }
    }
}
