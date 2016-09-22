namespace Grapevine.Shared
{
    internal static class UriSchemeExtensions
    {
        internal static string ToScheme(this UriScheme scheme)
        {
            return scheme.ToString().ToLower();
        }
    }
}
