using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;
using FileNotFoundException = Grapevine.Exceptions.Server.FileNotFoundException;

namespace Grapevine.Server
{
    public interface IPublicFolder : IDisposable
    {
        /// <summary>
        /// Gets or sets the default file to return when a directory is requested
        /// </summary>
        string IndexFileName { get; set; }

        /// <summary>
        /// Gets or sets the optional prefix for specifying when static content should be returned
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the folder to be scanned for static content requests
        /// </summary>
        string FolderPath { get; }

        /// <summary>
        /// Send file
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        void SendFile(IHttpContext context);
    }

    public class PublicFolder : IPublicFolder
    {
        protected ConcurrentDictionary<string, string> DirectoryList { get; set; }

        public static string DefaultFolderName { get; } = "public";
        public static string DefaultIndexFileName { get; } = "index.html";

        private FileSystemWatcher _watcher;
        private string _indexFileName = "index.html";
        private string _prefix;
        private string _path;

        public PublicFolder() : this(Path.Combine(Directory.GetCurrentDirectory(), DefaultFolderName), string.Empty) { }

        public PublicFolder(string path) : this(path, string.Empty) { }

        public PublicFolder(string path, string prefix)
        {
            DirectoryList = new ConcurrentDictionary<string, string>();
            FolderPath = Path.GetFullPath(path);
            Prefix = prefix;

            Watcher = new FileSystemWatcher
            {
                Path = FolderPath,
                Filter = "*",
                EnableRaisingEvents = true,
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName
            };

            Watcher.Created += (sender, args) => { AddToDirectoryList(args.FullPath); };
            Watcher.Deleted += (sender, args) => { RemoveFromDirectoryList(args.FullPath); };
            Watcher.Renamed += (sender, args) => { RenameInDirectoryList(args.OldFullPath, args.FullPath); };

            PopulateDirectoryList();
        }

        public string IndexFileName
        {
            get { return _indexFileName; }
            set
            {
                if (string.IsNullOrEmpty(value) || value == _indexFileName) return;
                _indexFileName = value;
                PopulateDirectoryList();
            }
        }

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                var prefix = string.IsNullOrWhiteSpace(value) ? string.Empty : $"/{value.Trim().TrimStart('/').TrimEnd('/').Trim()}";
                if (prefix.Equals(_prefix)) return;

                _prefix = prefix;
                PopulateDirectoryList();
            }
        }

        public string FolderPath
        {
            get
            {
                return _path;
            }

            protected internal set
            {
                var path = Path.GetFullPath(value);
                if (!Directory.Exists(path)) path = Directory.CreateDirectory(path).FullName;
                _path = path;
            }
        }

        public FileSystemWatcher Watcher
        {
            get { return _watcher; }
            protected internal set
            {
                if (value == null || value == _watcher) return;
                var tmpwatcher = _watcher;
                _watcher = value;
                tmpwatcher?.Dispose();
            }
        }

        public IDictionary<string, string> DirectoryListing => DirectoryList as IDictionary<string, string>;

        //public void UploadFile(IHttpContext context)
        //{
        //    throw new NotImplementedException();
        //}

        public void SendFile(IHttpContext context)
        {
            if (DirectoryList.ContainsKey(context.Request.PathInfo))
            {
                var filepath = DirectoryList[context.Request.PathInfo];

                var lastModified = File.GetLastWriteTimeUtc(filepath).ToString("R");
                context.Response.AddHeader("Last-Modified", lastModified);

                if (context.Request.Headers.AllKeys.Contains("If-Modified-Since"))
                {
                    if (context.Request.Headers["If-Modified-Since"].Equals(lastModified))
                    {
                        context.Response.SendResponse(HttpStatusCode.NotModified);
                        return;
                    }
                }

                context.Response.ContentType = ContentType.DEFAULT.FromExtension(filepath);
                context.Response.SendResponse(new FileStream(filepath, FileMode.Open));
            }

            if (!string.IsNullOrEmpty(Prefix) && context.Request.PathInfo.StartsWith(Prefix) && !context.WasRespondedTo)
            {
                throw new FileNotFoundException(context);
            }
        }

        protected void PopulateDirectoryList()
        {
            DirectoryList.Clear();
            foreach (var item in Directory.GetFiles(FolderPath, "*", SearchOption.AllDirectories).ToList())
            {
                AddToDirectoryList(item);
            }
        }

        protected void AddToDirectoryList(string fullPath)
        {
            DirectoryList[CreateDirectoryListKey(fullPath)] = fullPath;
            if (fullPath.EndsWith($"\\{_indexFileName}"))
                DirectoryList[CreateDirectoryListKey(fullPath.Replace($"\\{_indexFileName}", ""))] = fullPath;
        }

        protected void RemoveFromDirectoryList(string fullPath)
        {
            DirectoryList.Where(x => x.Value == fullPath).ToList().ForEach(pair =>
            {
                string key;
                DirectoryList.TryRemove(pair.Key, out key);
            });
        }

        protected void RenameInDirectoryList(string oldFullPath, string newFullPath)
        {
            RemoveFromDirectoryList(oldFullPath);
            AddToDirectoryList(newFullPath);
        }

        protected string CreateDirectoryListKey(string item)
        {
            return $"{Prefix}{item.Replace(FolderPath, string.Empty).Replace(@"\", "/")}";
        }

        public void Dispose()
        {
            _watcher.Dispose();
        }
    }
}