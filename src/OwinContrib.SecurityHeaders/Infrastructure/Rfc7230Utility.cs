using System.Linq;

namespace SecurityHeadersMiddleware.Infrastructure {
    internal static class Rfc7230Utility {
        private static readonly char[] TCharSpecials = {'!', '#', '$', '%', '&', '\'', '*', '+', '-', '.', '^', '_', '`', '|', '~'};

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