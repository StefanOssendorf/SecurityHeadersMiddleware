using System;

namespace SecurityHeaders {

    /// <summary>
    ///  The middleware to apply the strict-transport-security header.
    /// </summary>
    internal class StrictTransportSecurityMiddleware {
        private readonly StrictTransportSecuritySettings mSettings;

        /// <summary>
        /// The http-header name of the strict-transport-security header.
        /// </summary>
        public const string StrictTransportSecurityHeaderName = "Strict-Transport-Security";

        /// <summary>
        /// Initializes a new instance of <see cref="StrictTransportSecurityMiddleware"/>.
        /// </summary>
        /// <param name="settings">The settings. Must not be <code>null</code>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="settings"/> is <code>null</code>.</exception>
        internal StrictTransportSecurityMiddleware(StrictTransportSecuritySettings settings) {
            mSettings = settings;
        }

        /// <summary>
        /// Must be called before <see cref="ApplyHeader"/> is called.
        /// </summary>
        /// <param name="context">The context to apply on.</param>
        /// <returns>The result of before next, since it's possible to shortcircuit the pipeline.</returns>
        public BeforeNextResult BeforeNext(IHttpContext context) {

            if (!ShouldRedirectToSecureTransport(context)) {
                return new BeforeNextResult();
            }

            Uri uriToRedirectTo = mSettings.RedirectBuilder(context.RequestUri);
            context.PermanentRedirectTo(uriToRedirectTo);

            return new BeforeNextResult {
                EndPipeline = true
            };
        }

        private bool ShouldRedirectToSecureTransport(IHttpContext context) => !context.IsSecure && mSettings.Redirect;

        /// <summary>
        /// Applies the middleware on the context.
        /// </summary>
        /// <param name="context">The context. Must not be <code>null</code>.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="context"/> is <code>null</code>.</exception>
        public void ApplyHeader(IHttpContext context) {

            // Only over secure transport (http://tools.ietf.org/html/rfc6797#section-7.2)
            // Quotation: "An HSTS Host MUST NOT include the STS header field in HTTP responses conveyed over non-secure transport."
            if(!context.IsSecure) {
                return;
            }

            if (HeaderShouldNotBeSet(context.HeaderExist)) {
                return;
            }

            context.SetHeader(StrictTransportSecurityHeaderName, mSettings.HeaderValue);
        }

        private bool HeaderShouldNotBeSet(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == StrictTransportSecuritySettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return false;
            }

            return headerExist(StrictTransportSecurityHeaderName) && mSettings.HeaderHandling == StrictTransportSecuritySettings.HeaderControl.IgnoreIfHeaderAlreadySet;
        }
    }
}