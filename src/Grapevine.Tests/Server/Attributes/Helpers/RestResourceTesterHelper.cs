using Grapevine.Server.Attributes;

namespace Grapevine.Tests.Server.Attributes.Helpers
{
    public class NotARestResource { /* class body intentionally left blank */ }

    [RestResource]
    public class RestResourceNoArgs { /* class body intentionally left blank */ }

    [RestResource(BasePath = "/test/path")]
    public class RestResourceBasePathOnly { /* class body intentionally left blank */ }

    [RestResource(Scope = "TestScope")]
    public class RestResourceScopeOnly { /* class body intentionally left blank */ }

    [RestResource(BasePath = "/api", Scope = "ApiScope")]
    public class RestResourceBothArgs { /* class body intentionally left blank */ }

    public interface RestResourceInterface { /* interface body intentionally left blank */ }

    public abstract class AbstractRestResource { /* class body intentionally left blank */ }
}
