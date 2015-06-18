using System;

namespace SecurityHeadersMiddleware {
    /// <summary>
    ///     Options for strict-transport-security (STS).
    /// </summary>
    public class StrictTransportSecurityOptions {
        private const uint DefaultMaxAge = 31536000; // 12 Month
        private Func<int, string> mRedirectReasonPhrase;
        private Func<Uri, string> mRedirectUriBuilder;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StrictTransportSecurityOptions" /> class.
        /// </summary>
        public StrictTransportSecurityOptions() {
            MaxAge = DefaultMaxAge;
            IncludeSubDomains = true;
            RedirectUriBuilder = DefaultRedirectUriBuilder;
        }

        /// <summary>
        ///     Gets or sets the maximum age.
        /// </summary>
        /// <value>
        ///     The maximum age.
        /// </value>
        public uint MaxAge { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [include sub domains].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [include sub domains]; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeSubDomains { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether redirect to secure transport or not..
        /// </summary>
        /// <value>
        ///     <c>true</c> if [redirect to secure transport]; otherwise, <c>false</c>.
        /// </value>
        public bool RedirectToSecureTransport { get; set; }

        /// <summary>
        ///     Gets or sets the delegate to set a reasonphrase.<br />
        ///     Default reasonphrase is empty.
        /// </summary>
        public Func<int, string> RedirectReasonPhrase {
            get { return mRedirectReasonPhrase ?? DefaultResonPhrase; }
            set { mRedirectReasonPhrase = value; }
        }

        /// <summary>
        ///     Gets or sets the delegate to build the redirect uri.<br />
        ///     Default builder changes the url to https scheme.
        /// </summary>
        public Func<Uri, string> RedirectUriBuilder {
            get { return mRedirectUriBuilder; }
            set { mRedirectUriBuilder = value ?? DefaultRedirectUriBuilder; }
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