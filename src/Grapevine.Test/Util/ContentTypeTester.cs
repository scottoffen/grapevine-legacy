using Grapevine.Util;
using Xunit;
using Shouldly;

namespace Grapevine.Test.Util
{
    public class ContentTypeTester
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
    }
}
