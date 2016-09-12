using System;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util
{
    public class ContentTypeExtensionsTester
    {
        [Fact]
        public void content_type_extensions_is_text()
        {
            const ContentType ct = ContentType.TXT;
            ct.ToValue().ShouldBe("text/plain");
            ct.IsText().ShouldBeTrue();
            ct.IsBinary().ShouldBeFalse();
        }

        [Fact]
        public void content_type_extensions_is_binary()
        {
            const ContentType ct = ContentType.JPG;
            ct.ToValue().ShouldBe("image/jpeg");
            ct.IsText().ShouldBeFalse();
            ct.IsBinary().ShouldBeTrue();
        }

        [Fact]
        public void content_type_extensions_from_string_returns_default_if_string_value_is_null_or_empty()
        {
            ContentType.DEFAULT.FromString(null).Equals(ContentType.DEFAULT).ShouldBeTrue();
            ContentType.DEFAULT.FromString(string.Empty).Equals(ContentType.DEFAULT).ShouldBeTrue();
        }

        [Fact]
        public void content_type_extensions_from_string_returns_content_type_from_string()
        {
            ContentType.DEFAULT.FromString("application/json").Equals(ContentType.JSON).ShouldBeTrue();
        }

        [Fact]
        public void content_type_extensions_from_string_returns_default_if_string_value_is_not_in_enum()
        {
            ContentType.DEFAULT.FromString("test").Equals(ContentType.DEFAULT).ShouldBeTrue();
        }
    }
}
