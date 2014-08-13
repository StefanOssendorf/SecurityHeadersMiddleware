namespace SecurityHeadersMiddleware.Infrastructure {
    internal static class HeaderConstants {
        public const string XssProtection = "X-Xss-Protection";
        public const string XFrameOptions = "X-Frame-Options";
        public const string StrictTransportSecurity = "Strict-Transport-Security";
        public const string Location = "Location";
        public const string XContentTypeOptions = "X-Content-Type-Options";
        public const string ContentSecurityPolicy = "Content-Security-Policy";
        public const string ContentSecurityPolicyReportOnly = "Content-Security-Policy-Report-Only";
    }
}