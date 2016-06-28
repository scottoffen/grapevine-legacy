using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Test.Util
{
    public class CamelCaseTester
    {
        [Fact]
        public void converts_httpstatuscode_description_from_camel_case()
        {
            var description = HttpStatusCode.EnhanceYourCalm.ConvertToString();
            description.ShouldBe("Enhance Your Calm");
        }
    }
}
