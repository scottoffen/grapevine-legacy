using Grapevine.Server;
using Grapevine.Util;
using Rhino.Mocks;

namespace Grapevine.Tests.Server.Helpers
{
    public class MockContext
    {
        public static IHttpContext GetMockContext()
        {
            return GetMockContext(new MockProperties());
        }

        public static IHttpContext GetMockContext(MockProperties properties)
        {
            var request = GetMockRequest(properties);
            var response = GetMockResponse(properties);
            return GetMockContext(request, response);
        }

        public static IHttpContext GetMockContext(IHttpRequest request)
        {
            var properties = new MockProperties();
            var response = GetMockResponse(properties);
            return GetMockContext(request, response);
        }

        public static IHttpContext GetMockContext(IHttpResponse response)
        {
            var properties = new MockProperties();
            var request = GetMockRequest(properties);
            return GetMockContext(request, response);
        }

        public static IHttpContext GetMockContext(IHttpRequest request, IHttpResponse response)
        {
            var context = MockRepository.Mock<IHttpContext>();
            context.Stub(c => c.Request).Return(request);
            context.Stub(c => c.Response).Return(response);
            return context;
        }

        public static IHttpRequest GetMockRequest(MockProperties properties)
        {
            var request = MockRepository.Mock<IHttpRequest>();

            request.Stub(r => r.PathInfo).Return(properties.PathInfo);
            request.Stub(r => r.HttpMethod).Return(properties.HttpMethod);
            request.Stub(r => r.Name).Return(properties.Name);
            request.Stub(r => r.Id).Return(properties.Id);

            return request;
        }

        public static IHttpResponse GetMockResponse(MockProperties properties = null)
        {
            var response = MockRepository.Mock<IHttpResponse>();
            return response;
        }
    }

    public class MockProperties
    {
        public string PathInfo { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }

        public MockProperties()
        {
            PathInfo = "/";
            HttpMethod = HttpMethod.GET;
            Id = "1234";
            Name = "mocked";
        }
    }
}
