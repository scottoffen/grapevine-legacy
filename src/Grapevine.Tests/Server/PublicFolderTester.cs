using System;
using System.IO;
using Grapevine.Server;
using Grapevine.Tests.Server.Helpers;
using Grapevine.Util;
using Rhino.Mocks;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Server
{
    public class PublicFolderTester
    {
        [Fact]
        public void public_folder_default_values_are_correct()
        {
            var root = new PublicFolder();
            root.DefaultFileName.ShouldBe("index.html");
            root.FolderPath.EndsWith("public").ShouldBe(true);
            root.Prefix.Equals(string.Empty).ShouldBeTrue();
        }

        [Fact]
        public void public_folder_default_folder_can_be_changed()
        {
            const string newdir = "webroot";
            if (Directory.Exists(newdir)) Directory.Delete(newdir);

            var root = new PublicFolder { FolderPath = newdir };
            root.FolderPath.Equals(Path.Combine(Directory.GetCurrentDirectory(), newdir)).ShouldBe(true);
        }

        [Fact]
        public void public_folder_throws_exception_when_public_folder_is_set_to_empty_or_null()
        {
            var root = new PublicFolder();
            Should.Throw<ArgumentNullException>(() => root.FolderPath = null);
            Should.Throw<ArgumentException>(() => root.FolderPath = string.Empty);
        }

        [Fact]
        public void public_folder_prefix_setter_trims_whitespace_and_slashes_and_prepends_slash()
        {
            var folder = new PublicFolder();

            folder.Prefix = null;
            folder.Prefix.Equals(string.Empty).ShouldBeTrue();

            folder.Prefix = "hello";
            folder.Prefix.Equals("hello").ShouldBeFalse();
            folder.Prefix.Equals("/hello").ShouldBeTrue();

            folder.Prefix = "/hello";
            folder.Prefix.Equals("/hello").ShouldBeTrue();

            folder.Prefix = "hello/";
            folder.Prefix.Equals("/hello").ShouldBeTrue();

            folder.Prefix = "/hello/";
            folder.Prefix.Equals("/hello").ShouldBeTrue();

            folder.Prefix = "  /hello/  ";
            folder.Prefix.Equals("/hello").ShouldBeTrue();
        }

        [Fact]
        public void public_folder_send_public_file_does_not_send_when_http_verb_is_not_get_or_head()
        {
            var response = MockContext.GetMockResponse();
            var request = MockContext.GetMockRequest(new MockProperties { HttpMethod = HttpMethod.POST });
            var context = MockContext.GetMockContext(request, response);
            var root = new PublicFolder();

            root.SendPublicFile(context);

            response.AssertWasNotCalled(r => r.SendResponse(Arg<string>.Is.NotNull, Arg.Is(true)));
        }

        [Fact]
        public void public_folder_send_public_file_does_not_send_when_pathinfo_does_not_match_prefix()
        {
            var response = MockContext.GetMockResponse();
            var request = MockContext.GetMockRequest(new MockProperties { PathInfo = "/some/file.txt"});
            var context = MockContext.GetMockContext(request, response);
            var root = new PublicFolder {Prefix = "test"};

            root.SendPublicFile(context);

            response.AssertWasNotCalled(r => r.SendResponse(Arg<string>.Is.NotNull, Arg.Is(true)));
        }

        [Fact]
        public void public_folder_send_public_file_does_not_send_when_filepath_does_not_exist()
        {
            var response = MockContext.GetMockResponse();
            var request = MockContext.GetMockRequest(new MockProperties { PathInfo = @"/no/file/here"});
            var context = MockContext.GetMockContext(request, response);
            var root = new PublicFolder();

            root.SendPublicFile(context);

            response.AssertWasNotCalled(r => r.SendResponse(Arg<string>.Is.NotNull, Arg.Is(true)));
        }

        [Fact]
        public void public_folder_send_public_file_calls_send_response_when_file_exists()
        {
            const string folder = "sendresponse1";

            var response = MockContext.GetMockResponse();
            var request = MockContext.GetMockRequest(new MockProperties { PathInfo = $"/{folder}" });
            var context = MockContext.GetMockContext(request, response);
            var root = new PublicFolder();

            var folderpath = Directory.CreateDirectory(Path.Combine(root.FolderPath, folder)).FullName;
            if (!Directory.Exists(folderpath)) throw new Exception("Folder to test did not get created");

            var filepath = Path.Combine(folderpath, root.DefaultFileName);
            using (var sw = File.CreateText(filepath))
            {
                sw.WriteLine("Hello");
                sw.WriteLine("And");
                sw.WriteLine("Welcome");
            }

            root.SendPublicFile(context);

            response.AssertWasCalled(r => r.SendResponse(Arg<string>.Is.NotNull, Arg.Is(true)));

            File.Delete(filepath);
            Directory.Delete(folderpath);
        }

        [Fact]
        public void public_folder_send_public_file_calls_send_response_when_file_exists_with_prefix()
        {
            const string prefix = "prefix";
            const string folder = "sendresponse2";

            var response = MockContext.GetMockResponse();
            var request = MockContext.GetMockRequest(new MockProperties { PathInfo = $"/{prefix}/{folder}" });
            var context = MockContext.GetMockContext(request, response);
            var root = new PublicFolder {Prefix = prefix};

            var folderpath = Directory.CreateDirectory(Path.Combine(root.FolderPath, folder)).FullName;
            if (!Directory.Exists(folderpath)) throw new Exception("Folder to test did not get created");

            var filepath = Path.Combine(folderpath, root.DefaultFileName);
            using (var sw = File.CreateText(filepath)) { sw.WriteLine("Hello"); }

            root.SendPublicFile(context);

            response.AssertWasCalled(r => r.SendResponse(Arg<string>.Is.NotNull, Arg.Is(true)));

            File.Delete(filepath);
            Directory.Delete(folderpath);
        }

        [Fact]
        public void public_folder_get_file_path_returns_null_when_pathinfo_is_null_or_empty_or_whitespace()
        {
            var root = new PublicFolder();
            root.FilePathGetter(null).ShouldBeNull();
            root.FilePathGetter(string.Empty).ShouldBeNull();
            root.FilePathGetter(" ").ShouldBeNull();
        }

        [Fact]
        public void public_folder_get_file_path_returns_null_when_path_does_not_exist()
        {
            var root = new PublicFolder();
            root.FilePathGetter("/nope.txt").ShouldBeNull();
        }

        [Fact]
        public void public_folder_get_file_path_returns_null_when_default_file_from_folder_does_not_exist()
        {
            const string folder = "nodefault";

            var root = new PublicFolder();
            var folderpath = Directory.CreateDirectory(Path.Combine(root.FolderPath, folder)).FullName;
            if (!Directory.Exists(folderpath)) throw new Exception("Folder to test did not get created");

            root.FilePathGetter($"/{folder}").ShouldBeNull();

            Directory.Delete(folderpath);
        }

        [Fact]
        public void public_folder_get_file_path_returns_file_path()
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
        public void public_folder_get_file_path_returns_default_file_from_folder_path()
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
}
