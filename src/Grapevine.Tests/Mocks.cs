using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;
using NSubstitute;

namespace Grapevine.Tests
{
    public static class Mocks
    {
        private static readonly Dictionary<string, object> Defaults;

        static Mocks()
        {
            Defaults = new Dictionary<string, object>
            {
                {"HttpMethod", HttpMethod.GET},
                {"PathInfo", "/"},
                {"Name", "mocked"},
                {"Id", Guid.NewGuid().Truncate()},
                {"RequestHeaders", new WebHeaderCollection() },
                {"ResponseHeaders", new WebHeaderCollection() },
                {"ContentEncoding", Encoding.ASCII}
            };
        }

        public static IHttpContext HttpContext()
        {
            return HttpContext(Defaults);
        }

        public static IHttpContext HttpContext(Dictionary<string, object> properties)
        {
            return HttpContext(HttpRequest(properties), HttpResponse(properties));
        }

        public static IHttpContext HttpContext(IHttpRequest request)
        {
            return HttpContext(request, HttpResponse(Defaults));
        }

        public static IHttpContext HttpContext(IHttpResponse response)
        {
            return HttpContext(HttpRequest(Defaults), response);
        }

        public static IHttpContext HttpContext(IHttpRequest request, IHttpResponse response)
        {
            var context = Substitute.For<IHttpContext>();
            context.Request.Returns(request);
            context.Response.Returns(response);
            return context;
        }

        public static IHttpRequest HttpRequest()
        {
            return HttpRequest(Defaults);
        }

        public static IHttpRequest HttpRequest(Dictionary<string, object> properties)
        {
            return CopyProperties<IHttpRequest>(properties);
        }

        public static IHttpResponse HttpResponse()
        {
            return HttpResponse(Defaults);
        }

        public static IHttpResponse HttpResponse(Dictionary<string, object> properties)
        {
            return CopyProperties<IHttpResponse>(properties);
        }

        private static T CopyProperties<T>(Dictionary<string, object> props)
        {
            var properties = Merge(props);

            if (typeof(T) == typeof(IHttpRequest))
            {
                var target = Substitute.For<IHttpRequest>();

                target.Headers.Returns((WebHeaderCollection)properties["RequestHeaders"]);
                target.HttpMethod.Returns((HttpMethod)properties["HttpMethod"]);
                target.PathInfo.Returns((string)properties["PathInfo"]);
                target.Name.Returns((string)properties["Name"]);
                target.Id.Returns((string)properties["Id"]);

                return (T)target;
            }

            if (typeof(T) == typeof(IHttpResponse))
            {
                var target = Substitute.For<IHttpResponse>();

                target.Headers.Returns((WebHeaderCollection)properties["ResponseHeaders"]);
                target.Advanced.Returns(new AdvancedHttpResponse(target as HttpResponse));
                target.ContentEncoding.Returns((Encoding) properties["ContentEncoding"]);

                target.When(x => x.AddHeader(Arg.Any<string>(), Arg.Any<string>())).Do(info =>
                {
                    target.Headers.Add(info.ArgAt<string>(0), info.ArgAt<string>(1));
                });

                return (T)target;
            }

            throw new Exception($"Can not create substitute for {typeof(T)}");
        }

        private static Dictionary<string, object> Merge(Dictionary<string, object> source)
        {
            var target = new Dictionary<string, object>(Defaults);

            foreach (var key in source.Keys)
            {
                target[key] = source[key];
            }

            return target;
        }
    }
}
