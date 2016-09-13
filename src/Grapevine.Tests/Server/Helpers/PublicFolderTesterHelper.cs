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
    }
}
