using System.IO;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;

namespace Grapevine.Server
{
    public interface IPublicFolder
    {
        /// <summary>
        /// Gets or sets the default file to return when a directory is requested
        /// </summary>
        string DefaultFileName { get; set; }

        /// <summary>
        /// Gets or sets the optional prefix for specifying when static content should be returned
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the folder to be scanned for static content requests
        /// </summary>
        string FolderPath { get; set; }

        /// <summary>
        /// If it exists, responds to the request with the requested file
        /// </summary>
        IHttpContext SendPublicFile(IHttpContext context);
    }

    /// <summary>
    /// Provides methods for working with files and folder for static content
    /// </summary>
    public class PublicFolder : IPublicFolder
    {
        protected const string DefaultFolder = "public";
        protected const bool IsFilePath = true;
        private string _folderPath;
        private string _prefix = string.Empty;

        /// <inheritdoc/>
        public string DefaultFileName { get; set; } = "index.html";

        /// <inheritdoc/>
        public string Prefix
        {
            get { return _prefix; }
            set { _prefix = value == null ? string.Empty : $"/{value.Trim().TrimStart('/').TrimEnd('/')}"; }
        }

        /// <inheritdoc/>
        public string FolderPath
        {
            get
            {
                if (_folderPath != null) return _folderPath;
                var path = Path.Combine(Directory.GetCurrentDirectory(), DefaultFolder);
                _folderPath = Directory.Exists(path) ? path : Directory.CreateDirectory(path).FullName;
                return _folderPath;
            }
            set
            {
                _folderPath = Directory.Exists(value) ? value : Directory.CreateDirectory(value).FullName;
            }
        }

        /// <inheritdoc/>
        public IHttpContext SendPublicFile(IHttpContext context)
        {
            if (context.Request.HttpMethod != HttpMethod.GET && context.Request.HttpMethod != HttpMethod.HEAD)
                return context;

            if (!string.IsNullOrWhiteSpace(Prefix) && !context.Request.PathInfo.StartsWith(Prefix)) return context;

            var filepath = GetFilePath(context.Request.PathInfo);
            if (filepath != null) context.Response.SendResponse(filepath, IsFilePath);

            return context;
        }

        /// <summary>
        /// Returns the path to the file specified by the pathinfo
        /// </summary>
        private string GetFilePath(string pathinfo)
        {
            if (string.IsNullOrWhiteSpace(pathinfo)) return null;

            var path = Path.Combine(FolderPath,
                (string.IsNullOrWhiteSpace(Prefix) ? pathinfo : pathinfo.Replace(Prefix, "")).TrimStart('/', '\\')
                    .Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(path)) return path;
            if (!Directory.Exists(path)) return null;

            path = Path.Combine(path, DefaultFileName);
            return File.Exists(path) ? path : null;
        }
    }
}
