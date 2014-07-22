namespace SecurityHeadersMiddleware.Infrastructure {
    internal static class StringExtension {
        public static string FormatWith(this string source, params object[] values) {
            return string.Format(source, values);
        }

        public static bool IsNullOrWhiteSpace(this string source) {
            return string.IsNullOrWhiteSpace(source);
        }
    }
}