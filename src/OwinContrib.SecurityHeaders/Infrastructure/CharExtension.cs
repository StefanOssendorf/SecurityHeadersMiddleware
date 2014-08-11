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
}