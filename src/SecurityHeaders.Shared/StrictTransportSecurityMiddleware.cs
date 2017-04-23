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
        public StrictTransportSecurityMiddleware(StrictTransportSecuritySettings settings) {
            Guard.NotNull(settings, nameof(settings));
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
            Uri uriToRedirectTo = mSettings.RedirectUriBuilder(context.RequestUri);
            context.PermanentRedirectTo(uriToRedirectTo);
            return new BeforeNextResult {
                EndPipeline = true
            };
        }

        private bool ShouldRedirectToSecureTransport(IHttpContext context) {
            return !context.IsSecure && mSettings.PermanentRedirectToSecureTransport;
        }

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

            context.OverrideHeader(StrictTransportSecurityHeaderName, mSettings.HeaderValue);
        }


        private bool HeaderShouldNotBeSet(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == StrictTransportSecuritySettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return false;
            }

            return headerExist(StrictTransportSecurityHeaderName) && mSettings.HeaderHandling == StrictTransportSecuritySettings.HeaderControl.IgnoreIfHeaderAlreadySet;
        }
    }

    /// <summary>
    /// Represents the result of the <see cref="StrictTransportSecurityMiddleware.BeforeNext"/> method call.
    /// </summary>
    public class BeforeNextResult {

        /// <summary>
        /// Get if the <see cref="StrictTransportSecurityMiddleware.BeforeNext"/> call requests an end of the pipeline.
        /// </summary>
        public bool EndPipeline { get; set; }
    }

    /// <summary>
    /// Defines the Strict-Transport-Security settings. <br/>
    /// This type is immutable.
    /// </summary>
    public class StrictTransportSecuritySettings {

        /// <summary>
        /// Get how the middleware should handle this header.
        /// </summary>
        public HeaderControl HeaderHandling { get; }

        /// <summary>
        /// Get if the request should be permanently redirected (301) to a secure url.
        /// </summary>
        public bool PermanentRedirectToSecureTransport { get; private set; }

        /// <summary>
        /// Get the value of the header to be set when applied.
        /// </summary>
        public string HeaderValue { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="StrictTransportSecuritySettings"/> with default behavior.
        /// </summary>
        public StrictTransportSecuritySettings() {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permanentRedirect"></param>
        public StrictTransportSecuritySettings(bool permanentRedirect) {
            PermanentRedirectToSecureTransport = permanentRedirect;
        }

        /// <summary>
        /// Get the secure uri for the given uri.
        /// </summary>
        /// <param name="contextRequestUri">The unsecure uri which should be redirected to a secure uri.</param>
        /// <returns>The secure uri.</returns>
        public Uri RedirectUriBuilder(Uri contextRequestUri) {
            var builder = new UriBuilder(contextRequestUri) {
                Scheme = "https",
                Port = -1
            };
            return builder.Uri;
        }

        /// <summary>
        /// Specifies the handling of the header.
        /// </summary>
        public enum HeaderControl {

            /// <summary>
            /// If the header is already set, it will be replaced with the configured value.
            /// </summary>
            OverwriteIfHeaderAlreadySet = 0,

            /// <summary>
            /// If the header is already set, it will not be overwritten and the configured value will be ignored.
            /// </summary>
            IgnoreIfHeaderAlreadySet = 1,
        }
    }
}