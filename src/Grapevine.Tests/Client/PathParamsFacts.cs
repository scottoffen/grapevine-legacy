using Grapevine.Client;
using Grapevine.Exceptions.Client;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Client
{
    public class PathParamsFacts
    {
        public class StringifyMethod
        {
            [Fact]
            public void ParsesResourceString()
            {
                const string resource = "user/[lastname]/[firstname]";
                var pathparams = new PathParams {{"firstname", "John"}, {"lastname", "Doe"}};

                var path = pathparams.Stringify(resource);

                path.ShouldBe("user/Doe/John");
            }

            [Fact]
            public void DoesNotModifyResourceWhenNoParamsExistInResource()
            {
                const string resource = "users/Doe/John";
                var pathparams = new PathParams {{"key", "value"}};

                var path = pathparams.Stringify(resource);

                path.ShouldBe(resource);
            }

            [Fact]
            public void ThrowsExceptionWhenNotAllPlaceholdersAreParses()
            {
                const string resource = "user/[lastname]/[firstname]";
                var pathparams = new PathParams {{"firstname", "John"}};

                var exception = Record.Exception(() => pathparams.Stringify(resource));

                exception.ShouldNotBeNull();
                exception.ShouldBeOfType<ClientStateException>();
                exception.Message.ShouldNotBeNull();
                exception.Message.ShouldBe("Not all parameters were replaced in request resource: user/[lastname]/John");
            }
        }
    }
}
