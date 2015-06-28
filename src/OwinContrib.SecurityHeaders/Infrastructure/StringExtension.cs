using System.Linq;

namespace SecurityHeadersMiddleware.Infrastructure {
    internal static class StringExtension {
        public static string FormatWith(this string source, params object[] values) {
            return string.Format(source, values);
        }

        public static bool IsNullOrWhiteSpace(this string source) {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool IsEmpty(this string source) {
            return source == string.Empty;
        }

        public static string PercentEncode(this string source) {
            var value = source;
            if (source.Contains(";")) {
                value = source.Replace(";", "%3B");
            }
            if (source.Contains(",")) {
                value = source.Replace(",", "%2C");
            }
            return value;
        }
    }
}