using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class HttpStatusCodeFacts
    {
        public class ExtensionMethods
        {
            public class ConvertToStringMethod
            {
                [Fact]
                public void ConvertsCamelCaseToString()
                {
                    HttpStatusCode.NonAuthoritativeInformation.ConvertToString().ShouldBe("Non Authoritative Information");
                }
            }

            public class ToIntegerMethod
            {
                [Fact]
                public void ConvertsToInt()
                {
                    HttpStatusCode.NonAuthoritativeInformation.ToInteger().ShouldBe(203);
                }
            }
        }
    }
}
