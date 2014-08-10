using System.Linq;

namespace SecurityHeadersMiddleware.Infrastructure {
    internal static class CharExtension {
        public static bool IsAscii(this char source) {
            return source >= 0 && source <= 127;
        }

        public static bool IsCTL(this char source) {
            return source <= 31 || source == 127;
        }

        public static bool IsRfc5234Alpha(this char source) {
            // A-Z and a-z
            return source <= 0x5A && source >= 0x41 || source <= 0x7A && source >= 0x61;
        }

        public static bool IsRfc5234Digit(this char source) {
            return source <= 0x39 && source >= 0x30;
        }
    }

    internal static class Rfc7230Utility {
        private static readonly char[] TCharSpecials = { '!', '#', '$', '%', '&', '\'', '*', '+', '-', '.', '^', '_', '`', '|', '~' };

        public static bool IsToken(string token) {
            if (token.IsNullOrWhiteSpace()) {
                return false;
            }
            return token.All(IsTChar);
        }

        public static bool IsTChar(char source) {
            return source.IsRfc5234Alpha() || source.IsRfc5234Digit() || TCharSpecials.Any(c => c == source);
        }
    }
}