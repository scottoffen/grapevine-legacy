using System.IO;
using Grapevine.Server;
using Shouldly;
using Xunit;

namespace Grapevine.Test.Server
{
    public class ContentRootTester
    {
        [Fact]
        public void default_index_is_index_dot_html()
        {
            var root = new ContentRoot();
            root.DefaultFileName.ShouldBe("index.html");
        }

        [Fact]
        public void default_index_can_be_changed()
        {
            var root = new ContentRoot {DefaultFileName = "default.html"};
            root.DefaultFileName.ShouldBe("default.html");
        }

        [Fact]
        public void default_folder_is_webroot()
        {
            var root = new ContentRoot();
            root.Folder.EndsWith("webroot").ShouldBe(true);
        }

        [Fact]
        public void default_folder_can_be_changed()
        {
            var newdir = "newdir";
            if (Directory.Exists(newdir)) Directory.Delete(newdir);

            var root = new ContentRoot {Folder = newdir};
            root.Folder.EndsWith(newdir).ShouldBe(true);
        }

    }
}
