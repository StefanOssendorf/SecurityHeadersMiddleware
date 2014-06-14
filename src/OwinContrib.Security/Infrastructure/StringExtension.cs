namespace OwinContrib.Security.Infrastructure {
    internal static class StringExtension {
        public static string FormatWith(this string source, params object[] values) {
            return string.Format(source, values);
        }
    }
}