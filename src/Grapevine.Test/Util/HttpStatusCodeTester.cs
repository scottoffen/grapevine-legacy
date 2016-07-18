using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Test.Util
{
    public class HttpStatusCodeTester
    {
        [Fact]
        public void http_status_code_converts_case()
        {
            HttpStatusCode.NonAuthoritativeInformation.ConvertToString().ShouldBe("Non Authoritative Information");
        }

        [Fact]
        public void http_status_code_converts_to_int()
        {
            const int code = (int) HttpStatusCode.NonAuthoritativeInformation;
            code.ShouldBe(203);
        }
    }
}
