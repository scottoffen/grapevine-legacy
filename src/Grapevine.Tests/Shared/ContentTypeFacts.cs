using System;
using System.Collections.Generic;
using System.Diagnostics;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class ContentTypeFacts
    {
        public class ExtensionMethods
        {
            public class IsText
            {
                [Fact]
                public void ReturnsTrue()
                {
                    ContentType.TXT.IsText().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalse()
                {
                    ContentType.JPG.IsText().ShouldBeFalse();
                }
            }

            public class IsBinary
            {
                [Fact]
                public void ReturnsTrue()
                {
                    ContentType.JPG.IsBinary().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalse()
                {
                    ContentType.TXT.IsBinary().ShouldBeFalse();
                }
            }

            public class ToValue
            {
                [Fact]
                public void ReturnsMetadataValue()
                {
                    ContentType.JPG.ToValue().ShouldBe("image/jpeg");
                }
            }

            public class FromString
            {
                [Fact]
                public void ReturnsDefaultWhenParameterIsNullOrEmpty()
                {
                    ContentType.CUSTOM_TEXT.FromString(null).Equals(ContentType.CUSTOM_TEXT).ShouldBeTrue();
                    ContentType.CUSTOM_TEXT.FromString(string.Empty).Equals(ContentType.CUSTOM_TEXT).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsDefaultWhenParameterIsNotInEnum()
                {
                    ContentType.CUSTOM_TEXT.FromString("test").Equals(ContentType.CUSTOM_TEXT).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsContentTypeFromString()
                {
                    ContentType.CUSTOM_TEXT.FromString("application/json").Equals(ContentType.JSON).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsContentTypeFromStringWithParameters()
                {
                    ContentType.CUSTOM_TEXT.FromString("text/html; charset=UTF-8").Equals(ContentType.HTML).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsContentTypeFromStringWithMultipleValues()
                {
                    ContentType.CUSTOM_TEXT.FromString("text/html,text/plain").Equals(ContentType.HTML).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsContentTypeFromStringWithMultipleValuesWithParameters()
                {
                    ContentType.CUSTOM_TEXT.FromString("text/html,text/plain; charset=windows-1251").Equals(ContentType.HTML).ShouldBeTrue();
                    ContentType.CUSTOM_TEXT.FromString("text/html; charset=UTF-8,text/plain").Equals(ContentType.HTML).ShouldBeTrue();
                }

                [Fact(Skip="For local performance testing only")]
                public void MaintainsPerformance()
                {
                    const int cycles = 10000;
                    const ContentType ct = ContentType.DEFAULT;
                    const long maxThreshold = 50000;
                    const long minThreshold = 1000;

                    ct.FromString("application/json,text/json").ShouldBe(ContentType.JSON);
                    ct.FromString("chemical/x-xyz").ShouldBe(ContentType.XYZ);
                    ct.FromString("video/x-ms-wvx").ShouldBe(ContentType.WVX);
                    ct.FromString("text/x-vcalendar").ShouldBe(ContentType.VCS);
                    ct.FromString("application/jsonml+json").ShouldBe(ContentType.JSONML);
                    ct.FromString("application/pdf").ShouldBe(ContentType.PDF);
                    ct.FromString("image/x-pcx").ShouldBe(ContentType.PCX);
                    ct.FromString("application/vnd.ms-powerpoint.template.macroenabled.12").ShouldBe(ContentType.POTM);
                    ct.FromString("application/vnd.businessobjects").ShouldBe(ContentType.REP);
                    ct.FromString("image/x-mrsid-image").ShouldBe(ContentType.SID);

                    var sw = Stopwatch.StartNew();
                    for (var i = 0; i < cycles; i++)
                    {
                        var a = ct.FromString("application/json,text/json");
                        var b = ct.FromString("chemical/x-xyz");
                        var c = ct.FromString("video/x-ms-wvx");
                        var d = ct.FromString("text/x-vcalendar");
                        var e = ct.FromString("application/jsonml+json");
                        var f = ct.FromString("application/pdf");
                        var g = ct.FromString("image/x-pcx");
                        var h = ct.FromString("application/vnd.ms-powerpoint.template.macroenabled.12");
                        var j = ct.FromString("application/vnd.businessobjects");
                        var k = ct.FromString("image/x-mrsid-image");
                    }
                    sw.Stop();

                    //throw new Exception($"Ticks: {sw.ElapsedTicks}{Environment.NewLine}Milli: {sw.ElapsedMilliseconds}{Environment.NewLine}Per: {sw.ElapsedTicks / (cycles * 11)}");
                    sw.ElapsedTicks.ShouldBeInRange(minThreshold, maxThreshold);
                }
            }

            public class FromExtension
            {
                private const ContentType DefaultContentType = 0;

                [Fact]
                public void ReturnsDefaulttWhenArgumentIsNull()
                {
                    DefaultContentType.FromExtension(null).ShouldBe(DefaultContentType);
                }

                [Fact]
                public void ReturnsDefaulttWhenArgumentIsEmpty()
                {
                    DefaultContentType.FromExtension(string.Empty).ShouldBe(DefaultContentType);
                }

                [Fact]
                public void ReturnsDefaultWhenNoExtensionOnArgument()
                {
                    DefaultContentType.FromExtension("/ihavenoextension").ShouldBe(DefaultContentType);
                }

                [Fact]
                public void ReturnsDefaultWhenExtentionNotInEnum()
                {
                    DefaultContentType.FromExtension("/thisextention.doesnotexist").ShouldBe(DefaultContentType);
                }

                [Fact]
                public void ReturnsEnumFromExtension()
                {
                    DefaultContentType.FromExtension("/image.jpg").ShouldBe(ContentType.JPG);
                }

                [Fact]
                public void HtmlExtensionReturnsHtmlContentType()
                {
                    DefaultContentType.FromExtension("page.html").ShouldBe(ContentType.HTML);
                }
            }
        }

        public class MetadataAttribute
        {
            [Fact]
            public void MultipleAttributesNotAllowed()
            {
                var attributes = (IList<AttributeUsageAttribute>)typeof(ContentTypeMetadata).GetCustomAttributes(typeof(AttributeUsageAttribute), false);
                attributes.Count.ShouldBe(1);

                var attribute = attributes[0];
                attribute.AllowMultiple.ShouldBeFalse();
            }

            [Fact]
            public void AppliesDefaults()
            {
                const ContentType ct = ContentType.TEXT;
                ct.ToValue().ShouldBe("text/plain");
                ct.IsText().ShouldBeTrue();
                ct.IsBinary().ShouldBeFalse();
            }
        }
    }
}
