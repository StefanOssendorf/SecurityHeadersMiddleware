using System;

namespace SecurityHeadersMiddleware {
    public class StrictTransportSecurityOptions {
        private Func<int, string> mRedirectReasonPhrase;
        private Func<Uri, string> mRedirectUriBuilder;
        private const uint DefaultMaxAge = 31536000; // 12 Month

        public uint MaxAge { get; set; }
        public bool IncludeSubDomains { get; set; }
        public bool RedirectToSecureTransport { get; set; }

        public Func<int, string> RedirectReasonPhrase {
            get { return mRedirectReasonPhrase ?? DefaultResonPhrase; }
            set { mRedirectReasonPhrase = value; }
        }

        public Func<Uri, string> RedirectUriBuilder {
            get { return mRedirectUriBuilder ?? DefaultRedirectUriBuilder; }
            set { mRedirectUriBuilder = value; }
        }

        public StrictTransportSecurityOptions() {
            MaxAge = DefaultMaxAge;
            IncludeSubDomains = true;
        }

        private static string DefaultResonPhrase(int statusCode) {
            return "";
        }
        private static string DefaultRedirectUriBuilder(Uri uri) {
            var builder = new UriBuilder(uri) {
                Scheme = Uri.UriSchemeHttps,
                Port = -1
            };
            return builder.ToString();
        }
    }
}