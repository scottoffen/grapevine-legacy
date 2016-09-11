using System.Collections.Generic;
using System.Reflection;
using Grapevine.Server;

namespace Grapevine.Tests.Server.Helpers
{
    public static class PublicFolderExtensions
    {
        internal static string FilePathGetter(this PublicFolder folder, string pathInfo)
        {
            var memberInfo = folder.GetType();
            var method = memberInfo?.GetMethod("GetFilePath", BindingFlags.Instance | BindingFlags.NonPublic);
            return (string) method?.Invoke(folder, new object[] { pathInfo });
        }

        internal static string FolderCreator(this PublicFolder folder, string path)
        {
            var memberInfo = folder.GetType();
            var method = memberInfo?.GetMethod("CreateFolder", BindingFlags.Static | BindingFlags.NonPublic);
            return (string)method?.Invoke(null, new object[] { path });
        }
    }
}
