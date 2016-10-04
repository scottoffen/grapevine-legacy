using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Grapevine.Server;
using Grapevine.Shared;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class PublicFolderFacts
    {
        public class Constructors
        {
            [Fact]
            public void DefaultValues()
            {
                var root = new PublicFolder();
                root.DefaultFileName.ShouldBe("index.html");
                root.FolderPath.EndsWith("public").ShouldBe(true);
                root.Prefix.Equals(string.Empty).ShouldBeTrue();
            }
        }

        public class FolderPathProperty
        {
            [Fact]
            public void CreatesFolderIfNotExists()
            {
                const string newdir = "createme";
                if (Directory.Exists(newdir)) Directory.Delete(newdir);

                var root = new PublicFolder { FolderPath = newdir };
                root.FolderPath.Equals(Path.Combine(Directory.GetCurrentDirectory(), newdir)).ShouldBe(true);
            }

            [Fact]
            public void ThrowsExceptionWhenSetToEmpty()
            {
                var root = new PublicFolder();
                Should.Throw<ArgumentException>(() => root.FolderPath = string.Empty);
            }

            [Fact]
            public void ThrowsExceptionWhenSetToNull()
            {
                var root = new PublicFolder();
                Should.Throw<ArgumentNullException>(() => root.FolderPath = null);
            }
        }

        public class GetFilePathMethod
        {
            [Fact]
            public void ReturnsNullWhenPathInfoIsNull()
            {
                var root = new PublicFolder();
                root.FilePathGetter(null).ShouldBeNull();
            }

            [Fact]
            public void ReturnsNullWhenPathInfoIsEmptyString()
            {
                var root = new PublicFolder();
                root.FilePathGetter(string.Empty).ShouldBeNull();
            }

            [Fact]
            public void ReturnsNullWhenPathInfoIsWhiteSpace()
            {
                var root = new PublicFolder();
                root.FilePathGetter(" ").ShouldBeNull();
            }

            [Fact]
            public void ReturnsNullWhenPathDoesNotExist()
            {
                var root = new PublicFolder();
                root.FilePathGetter("/nope.txt").ShouldBeNull();
            }

            [Fact]
            public void ReturnsNullWhenDefaultFileDoesNotExist()
            {
                const string folder = "nodefault";
                var root = new PublicFolder();

                var folderpath = Directory.CreateDirectory(Path.Combine(root.FolderPath, folder)).FullName;
                if (!Directory.Exists(folderpath)) throw new Exception("Folder to test did not get created");

                root.FilePathGetter($"/{folder}").ShouldBeNull();

                Directory.Delete(folderpath);
            }

            [Fact]
            public void ReturnsExistingFilePath()
            {
                var root = new PublicFolder();
                const string file = "myfile";

                var filepath = Path.Combine(root.FolderPath, file);
                using (var sw = File.CreateText(filepath)) { sw.WriteLine("Hello"); }

                root.FilePathGetter(file).ShouldBe(filepath);
                root.FilePathGetter($"/{file}").ShouldBe(filepath);

                File.Delete(filepath);
            }

            [Fact]
            public void ReturnsExistingDefaultFilePath()
            {
                var root = new PublicFolder();
                const string folder = "folder";

                var folderpath = Directory.CreateDirectory(Path.Combine(root.FolderPath, folder)).FullName;
                if (!Directory.Exists(folderpath)) throw new Exception("Folder to test did not get created");

                var filepath = Path.Combine(folderpath, root.DefaultFileName);
                using (var sw = File.CreateText(filepath)) { sw.WriteLine("Hello"); }

                root.FilePathGetter(folder).ShouldBe(filepath);
                root.FilePathGetter($"/{folder}").ShouldBe(filepath);

                File.Delete(filepath);
                Directory.Delete(folderpath);
            }
        }

        public class PrefixProperty
        {
            [Fact]
            public void IsEmptyStringWhenSetToNull()
            {
                var folder = new PublicFolder {Prefix = null};
                folder.Prefix.Equals(string.Empty).ShouldBeTrue();
            }

            [Fact]
            public void PrependsMissingForwardSlash()
            {
                var folder = new PublicFolder {Prefix = "hello"};
                folder.Prefix.Equals("hello").ShouldBeFalse();
                folder.Prefix.Equals("/hello").ShouldBeTrue();
            }

            [Fact]
            public void DoesNotPrependForwadSlashWhenExists()
            {
                var folder = new PublicFolder {Prefix = "/hello"};
                folder.Prefix.Equals("/hello").ShouldBeTrue();
            }

            [Fact]
            public void TrimsTrailingSlash()
            {
                var folder = new PublicFolder {Prefix = "hello/"};
                folder.Prefix.Equals("/hello").ShouldBeTrue();

                folder.Prefix = "/hello/";
                folder.Prefix.Equals("/hello").ShouldBeTrue();
            }

            [Fact]
            public void TrimsLeadingAndTrailingWhitespace()
            {
                var folder = new PublicFolder {Prefix = "  /hello/  "};
                folder.Prefix.Equals("/hello").ShouldBeTrue();
            }
        }

        public class SendPublicFileMethod
        {
            [Fact]
            public void DoesNotSendWhenHttpVerbIsNotGetOrHead()
            {
                var properties = new Dictionary<string, object> { { "HttpMethod", HttpMethod.POST } };
                var context = Mocks.HttpContext(properties);
                var root = new PublicFolder();

                root.SendPublicFile(context);

                context.Response.DidNotReceiveWithAnyArgs().SendResponse(HttpStatusCode.Ok);
            }

            [Fact]
            public void DoesNotSendWhenPathInfoDoesNotMatchPrefix()
            {
                var properties = new Dictionary<string, object> { { "PathInfo", "/some/file.txt" } };
                var context = Mocks.HttpContext(properties);
                var root = new PublicFolder { Prefix = "test" };

                root.SendPublicFile(context);

                context.Response.DidNotReceiveWithAnyArgs().SendResponse(HttpStatusCode.Ok);
            }

            [Fact]
            public void DoesNotSendWhenFileDoesNotExist()
            {
                var properties = new Dictionary<string, object> { { "PathInfo", "/no/file/here" } };
                var context = Mocks.HttpContext(properties);
                var root = new PublicFolder();

                root.SendPublicFile(context);

                context.Response.DidNotReceiveWithAnyArgs().SendResponse(HttpStatusCode.Ok);
            }

            [Fact]
            public void SendsWhenFileExists()
            {
                const string folder = "sendresponse";
                var root = new PublicFolder { FolderPath = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().Truncate()) };
                var folderpath = Path.Combine(root.FolderPath, folder);
                var properties = new Dictionary<string, object> { { "PathInfo", $"/{folder}" } };
                var context = Mocks.HttpContext(properties);

                // Create the directory
                if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);

                // Create the required file
                var filepath = Path.Combine(folderpath, root.DefaultFileName);
                using (var sw = File.CreateText(filepath)) { sw.WriteLine("Hello"); }

                root.SendPublicFile(context);
                context.Response.Received().SendResponse(filepath, true);

                //// Clean up
                File.Delete(filepath);
                Directory.Delete(folderpath);
                Directory.Delete(root.FolderPath);
            }

            [Fact]
            public void SendsWhenFileExistsWithPrefix()
            {
                const string prefix = "prefix";
                const string folder = "sendresponse";
                var root = new PublicFolder { Prefix = prefix, FolderPath = Path.Combine(Directory.GetCurrentDirectory(), Guid.NewGuid().Truncate()) };
                var folderpath = Path.Combine(root.FolderPath, folder);
                var properties = new Dictionary<string, object> { { "PathInfo", $"/{prefix}/{folder}" } };
                var context = Mocks.HttpContext(properties);

                // Create the directory
                if (!Directory.Exists(folderpath)) Directory.CreateDirectory(folderpath);

                // Create the required file
                var filepath = Path.Combine(folderpath, root.DefaultFileName);
                using (var sw = File.CreateText(filepath)) { sw.WriteLine("Hello"); }

                root.SendPublicFile(context);
                context.Response.Received().SendResponse(filepath, true);

                // Clean up
                File.Delete(filepath);
                Directory.Delete(folderpath);
                Directory.Delete(root.FolderPath);
            }
        }
    }

    public static class PublicFolderExtensions
    {
        internal static string FilePathGetter(this PublicFolder folder, string pathInfo)
        {
            var memberInfo = folder.GetType();
            var method = memberInfo?.GetMethod("GetFilePath", BindingFlags.Instance | BindingFlags.NonPublic);
            return (string)method?.Invoke(folder, new object[] { pathInfo });
        }
    }
}
