using System.IO;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server
{
    public class ContentRoot
    {
        private string _folder;

        public ContentRoot()
        {
            var path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (path != null) _folder = Path.Combine(path, "webroot");
            if (!FolderExists(_folder)) CreateFolder(_folder);
        }

        public string Folder
        {
            get { return _folder; }
            set { _folder = FolderExists(value) ? value : CreateFolder(value); }
        }

        public IHttpContext ReturnFile(IHttpContext context)
        {
            if (context.Request.HttpMethod != HttpMethod.GET || string.IsNullOrWhiteSpace(_folder)) return context;

            var filepath = GetFilePath(context.Request.PathInfo);
            if (filepath != null) context.Response.SendResponse(filepath, true);

            return context;
        }

        public string DefaultFileName { get; set; } = "index.html";

        private static bool FolderExists (string folder)
        {
            return !string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder);
        }

        private static string CreateFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) return folder;
            var info = Directory.CreateDirectory(folder);
            return info.FullName;
        }

        private string GetFilePath(string pathinfo)
        {
            if (string.IsNullOrWhiteSpace(_folder)) return null;
            var path = Path.Combine(_folder, pathinfo);

            if (File.Exists(path)) return path;
            if (!Directory.Exists(path)) return null;

            path = Path.Combine(path, DefaultFileName);
            return File.Exists(path) ? path : null;
        }
    }
}
