using System;
using System.Collections.Generic;
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
                    ContentType.DEFAULT.FromString(null).Equals(ContentType.DEFAULT).ShouldBeTrue();
                    ContentType.DEFAULT.FromString(string.Empty).Equals(ContentType.DEFAULT).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsDefaultWhenParameterIsNotInEnum()
                {
                    ContentType.DEFAULT.FromString("test").Equals(ContentType.DEFAULT).ShouldBeTrue();
                }

                [Fact]
                public void ReturnsContentTypeFromString()
                {
                    ContentType.DEFAULT.FromString("application/json").Equals(ContentType.JSON).ShouldBeTrue();
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
