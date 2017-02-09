using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Grapevine.Interfaces.Server;
using Grapevine.Server;
using Grapevine.Shared;
using NSubstitute;
using Shouldly;
using Xunit;
using HttpStatusCode = Grapevine.Shared.HttpStatusCode;

namespace Grapevine.Tests.Server
{
    public class HttpResponseExtensionsFacts
    {
        public class SendResponseMethod
        {
            private readonly IHttpResponse _response;

            private static readonly string Txtpath;
            private static readonly string Pngpath;

            private static readonly byte[] ExpectedTxtBytes;
            private static readonly byte[] ExpectedPngBytes;

            static SendResponseMethod()
            {
                var basepath = Path.Combine(Directory.GetCurrentDirectory(), "test-get-bytes.");
                Txtpath = basepath + "txt";
                Pngpath = basepath + "png";

                ExpectedTxtBytes = new byte[]
                {
                    84, 104, 105, 115, 32, 105, 115, 32, 115, 105, 109, 112, 108, 101, 32, 116, 101, 120, 116, 32, 102,
                    105, 108, 101, 32, 116, 111, 32, 116, 101, 115, 116, 32, 103, 101, 116, 116, 105, 110, 103, 32, 116,
                    104, 101, 32, 98, 121, 116, 101, 115, 32, 111, 102, 32, 97, 32, 116, 101, 120, 116, 32, 102, 105,
                    108, 101, 46
                };

                ExpectedPngBytes = new byte[]
                {
                    137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82, 0, 0, 0, 10, 0, 0, 0, 10, 8, 2, 0, 0,
                    0, 2, 80, 88, 234, 0, 0, 0, 1, 115, 82, 71, 66, 0, 174, 206, 28, 233, 0, 0, 0, 4, 103, 65, 77, 65, 0,
                    0, 177, 143, 11, 252, 97, 5, 0, 0, 0, 9, 112, 72, 89, 115, 0, 0, 14, 195, 0, 0, 14, 195, 1, 199, 111,
                    168, 100, 0, 0, 0, 24, 116, 69, 88, 116, 83, 111, 102, 116, 119, 97, 114, 101, 0, 112, 97, 105, 110,
                    116, 46, 110, 101, 116, 32, 52, 46, 48, 46, 54, 252, 140, 99, 223, 0, 0, 0, 41, 73, 68, 65, 84, 40,
                    83, 99, 248, 207, 128, 130, 208, 249, 104, 8, 157, 143, 134, 208, 249, 104, 8, 157, 143, 134, 208,
                    249, 29, 168, 0, 93, 30, 42, 12, 3, 244, 147, 102, 248, 15, 0, 74, 126, 114, 142, 14, 108, 150, 112,
                    0, 0, 0, 0, 73, 69, 78, 68, 174, 66, 96, 130
                };
            }

            public SendResponseMethod()
            {
                _response = Substitute.For<IHttpResponse>();
                _response.Headers = new WebHeaderCollection();

                _response.When(x => x.AddHeader(Arg.Any<string>(), Arg.Any<string>())).Do(info =>
                {
                    _response.Headers.Add(info.ArgAt<string>(0), info.ArgAt<string>(1));
                });
            }

            public static byte[] GetBytes(string content)
            {
                return GetBytes(Encoding.ASCII, content);
            }

            public static byte[] GetBytes(Encoding encoding, string content)
            {
                return encoding.GetBytes(content);
            }

            [Fact]
            public void SendsStatusCodeResponse()
            {
                const HttpStatusCode status = HttpStatusCode.EnhanceYourCalm;
                var bytes = GetBytes($"<h1>{status}</h1>");

                _response.StatusDescription.Returns(status.ToString());
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(bytes);
                });

                _response.SendResponse(status);

                _response.StatusCode.ShouldBe(status);
                _response.ContentType.ShouldBe(ContentType.HTML);
                _response.ContentEncoding.ShouldBe(Encoding.ASCII);
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsStatusCodeAndStringResponse()
            {
                const HttpStatusCode status = HttpStatusCode.EnhanceYourCalm;
                const string content = "This is the content";
                var bytes = GetBytes(content);

                _response.ContentEncoding = Encoding.ASCII;
                _response.StatusDescription.Returns(status.ToString());
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(bytes);
                });

                _response.SendResponse(status, content);

                _response.StatusCode.ShouldBe(status);
                _response.ContentType.ShouldBe(ContentType.CUSTOM_TEXT);
                _response.ContentEncoding.ShouldBe(Encoding.ASCII);
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsStatusCodeAndException()
            {
                const HttpStatusCode status = HttpStatusCode.EnhanceYourCalm;
                var exception = new Exception("This is the exception message");
                
                var bytes = GetBytes($"{exception.Message}{Environment.NewLine}<br>{Environment.NewLine}{exception.StackTrace}");

                _response.ContentEncoding = Encoding.ASCII;
                _response.StatusDescription.Returns(status.ToString());
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(bytes);
                });

                _response.SendResponse(status, exception);

                _response.StatusCode.ShouldBe(status);
                _response.ContentType.ShouldBe(ContentType.CUSTOM_TEXT);
                _response.ContentEncoding.ShouldBe(Encoding.ASCII);
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsStringWithEncoding()
            {
                const string content = "This is the content";
                var encoding = Encoding.UTF32;
                var bytes = GetBytes(encoding, content);

                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(bytes);
                });

                _response.SendResponse(content, encoding);

                _response.ContentEncoding.ShouldBe(encoding);
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsString()
            {
                const string content = "This is the content";
                var encoding = Encoding.UTF32;
                var bytes = GetBytes(encoding, content);

                _response.ContentEncoding = encoding;
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(bytes);
                });

                _response.SendResponse(content);

                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsTextFileStreamAsAttachmentWithContentTypeAndEncoding()
            {
                var encoding = Encoding.ASCII;
                const ContentType contentType = ContentType.TXT;
                const string filename = "test-file.txt";

                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedTxtBytes);
                });

                _response.SendResponse(new FileStream(Txtpath, FileMode.Open), contentType, encoding, filename);

                _response.ContentType.ShouldBe(contentType);
                _response.ContentEncoding.ShouldBe(encoding);
                _response.Headers.AllKeys.ShouldContain("Content-Disposition");
                _response.Headers["Content-Disposition"].ShouldBe($"attachment; filename=\"{filename}\"");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsTextFileStreamWithContentTypeAndEncoding()
            {
                var encoding = Encoding.ASCII;
                const ContentType contentType = ContentType.TXT;

                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedTxtBytes);
                });

                _response.SendResponse(new FileStream(Txtpath, FileMode.Open), contentType, encoding);

                _response.ContentType.ShouldBe(contentType);
                _response.ContentEncoding.ShouldBe(encoding);
                _response.Headers.AllKeys.ShouldNotContain("Content-Disposition");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsTextFileStreamAsAttachmentWithEncoding()
            {
                var encoding = Encoding.ASCII;
                const string filename = "test-file.txt";

                _response.ContentType = ContentType.TXT;
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedTxtBytes);
                });

                _response.SendResponse(new FileStream(Txtpath, FileMode.Open), encoding, filename);

                _response.ContentEncoding.ShouldBe(encoding);
                _response.Headers.AllKeys.ShouldContain("Content-Disposition");
                _response.Headers["Content-Disposition"].ShouldBe($"attachment; filename=\"{filename}\"");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsTextFileStreamAsAttachmentWithContentType()
            {
                const ContentType contentType = ContentType.TXT;
                const string filename = "test-file.txt";

                _response.ContentEncoding = Encoding.ASCII;
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedTxtBytes);
                });

                _response.SendResponse(new FileStream(Txtpath, FileMode.Open), contentType, filename);

                _response.ContentType.ShouldBe(contentType);
                _response.Headers.AllKeys.ShouldContain("Content-Disposition");
                _response.Headers["Content-Disposition"].ShouldBe($"attachment; filename=\"{filename}\"");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsFileStreamWithContentType()
            {
                const ContentType contentType = ContentType.PNG;

                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedPngBytes);
                });

                _response.SendResponse(new FileStream(Pngpath, FileMode.Open), contentType);

                _response.ContentType.ShouldBe(contentType);
                _response.Headers.AllKeys.ShouldNotContain("Content-Disposition");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsTextFileStreamWithEncoding()
            {
                var encoding = Encoding.ASCII;

                _response.ContentType = ContentType.TXT;
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedTxtBytes);
                });

                _response.SendResponse(new FileStream(Txtpath, FileMode.Open), encoding);

                _response.ContentEncoding.ShouldBe(encoding);
                _response.Headers.AllKeys.ShouldNotContain("Content-Disposition");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsFileStreamAsAttachment()
            {
                const string filename = "myfile.png";

                _response.ContentType = ContentType.PNG;
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedPngBytes);
                });

                _response.SendResponse(new FileStream(Pngpath, FileMode.Open), filename);

                _response.Headers.AllKeys.ShouldContain("Content-Disposition");
                _response.Headers["Content-Disposition"].ShouldBe($"attachment; filename=\"{filename}\"");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsStream()
            {
                _response.ContentEncoding = Encoding.ASCII;
                _response.ContentType = ContentType.TXT;
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedTxtBytes);
                });

                _response.SendResponse(new FileStream(Txtpath, FileMode.Open));

                _response.Headers.AllKeys.ShouldNotContain("Content-Disposition");
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }

            [Fact]
            public void SendsStreamWithoutModifingExpiresHeader()
            {
                var expires = DateTime.Now.AddHours(2).ToString("R");

                _response.AddHeader("Expires", expires);
                _response.ContentType = ContentType.PNG;
                _response.When(x => x.SendResponse(Arg.Any<byte[]>())).Do(info =>
                {
                    info.ArgAt<byte[]>(0).ShouldBe(ExpectedPngBytes);
                });

                _response.SendResponse(new FileStream(Pngpath, FileMode.Open));

                _response.Headers.AllKeys.ShouldContain("Expires");
                _response.Headers["Expires"].ShouldBe(expires);
                _response.Received().SendResponse(Arg.Any<byte[]>());
            }
        }
    }
}
