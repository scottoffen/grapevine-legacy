using System.IO;
using System.Reflection;
using Grapevine.Util;

namespace Grapevine.Server
{
    /// <summary>
    /// Provides methods for working with files and folder for static content
    /// </summary>
    public class ContentRoot
    {
        private const bool IsFilePath = true;
        private string _folder;

        public ContentRoot()
        {
            var path = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            if (path != null) _folder = Path.Combine(path, "webroot");
            if (!FolderExists(_folder)) CreateFolder(_folder);
        }

        /// <summary>
        /// Gets or sets the folder to be scanned for static content requests
        /// </summary>
        public string Folder
        {
            get { return _folder; }
            set { _folder = FolderExists(value) ? value : CreateFolder(value); }
        }

        /// <summary>
        /// If it exists, responds to the request with the requested file
        /// </summary>
        public IHttpContext ReturnFile(IHttpContext context, string prefix = null)
        {
            if (context.Request.HttpMethod != HttpMethod.GET || string.IsNullOrWhiteSpace(_folder)) return context;

            var path = string.IsNullOrWhiteSpace(prefix) ? context.Request.PathInfo : context.Request.PathInfo.Replace(prefix, "");
            path = path.TrimStart('/', '\\');

            var filepath = GetFilePath(path);
            if (filepath != null) context.Response.SendResponse(filepath, IsFilePath);

            return context;
        }

        /// <summary>
        /// Gets or sets the default file to return when a directory is requested
        /// </summary>
        public string DefaultFileName { get; set; } = "index.html";

        /// <summary>
        /// Returns true if the specified directory exists
        /// </summary>
        private static bool FolderExists (string folder)
        {
            return !string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder);
        }

        /// <summary>
        /// Returns the path the folder created
        /// </summary>
        private static string CreateFolder(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder)) return null;
            var info = Directory.CreateDirectory(folder);
            return info.FullName;
        }

        /// <summary>
        /// Returns the path to the specified file
        /// </summary>
        private string GetFilePath(string pathinfo)
        {
            if (string.IsNullOrWhiteSpace(_folder)) return null;
            var path = pathinfo.Replace("/", Path.DirectorySeparatorChar.ToString());
            path = Path.Combine(_folder, path);

            if (File.Exists(path)) return path;
            if (!Directory.Exists(path)) return null;

            path = Path.Combine(path, DefaultFileName);
            return File.Exists(path) ? path : null;
        }
    }
}
