using System;
using Grapevine.Server;
using Xunit;
using Grapevine.Shared;
using Shouldly;

namespace Grapevine.Tests.Shared
{
    public class InternalExtensionsFacts
    {
        public class GuidExtensions
        {
            public class TruncateMethod
            {
                [Fact]
                public void ReturnsLastPartOfGuid()
                {
                    var guid = Guid.NewGuid();
                    var trunc = guid.Truncate();
                    guid.ToString().EndsWith($"-{trunc}").ShouldBeTrue();
                }
            }
        }

        public class StringExtensions
        {
            public class ConvertCamelCaseMethod
            {
                [Fact]
                public void ConvertsToTitleCase()
                {
                    const string original = "EnhanceYourCalm";
                    const string expected = "Enhance Your Calm";
                    original.ConvertCamelCase().ShouldBe(expected);
                }
            }
        }

        public class TypeExtensions
        {
            public class ImplementsMethod
            {
                [Fact]
                public void ReturnsTrueIfTypeImplementsInterface()
                {
                    typeof(Router).Implements<IRouter>().ShouldBeTrue();
                }

                [Fact]
                public void ReturnsFalseIfTypeDoesNotImplementInterface()
                {
                    typeof(RestServer).Implements<IRouter>().ShouldBeFalse();
                }
            }
        }
    }
}
