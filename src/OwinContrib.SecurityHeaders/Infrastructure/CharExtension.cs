namespace SecurityHeadersMiddleware.Infrastructure {
    public static class CharExtension {
        public static bool IsAscii(this char source) {
            return source >= 0 && source <= 127;
        }

        public static bool IsCTL(this char source) {
            return source <= 31 || source == 127;
        }
    }
}