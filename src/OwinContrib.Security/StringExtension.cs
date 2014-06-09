namespace OwinContrib.Security {
    internal static class StringExtension {
        public static string Formatted(this string source, params object[] values) {
            return string.Format(source, values);
        }
    }
}