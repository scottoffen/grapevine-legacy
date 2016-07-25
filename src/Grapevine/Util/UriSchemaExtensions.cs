namespace Grapevine.Util
{
    internal static class UriSchemaExtensions
    {
        internal static string ToScheme(this UriScheme scheme)
        {
            return scheme.ToString().ToLower();
        }
    }
}
