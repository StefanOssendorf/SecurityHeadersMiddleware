using System;

namespace OwinContrib.Security {
    internal static class Rfc6454Utility {
        public static bool HasSameOrigin(Uri uri1, Uri uri2) {
            return Uri.Compare(uri1, uri2, UriComponents.SchemeAndServer, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static string SerializeOrigin(Uri uri) {
            return uri.GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
        }
    }
}