using System;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    partial class CspSourceList {
        private readonly HostSourceCollection mHosts;

        /// <summary>
        ///     Adds a host to the source-list.
        /// </summary>
        /// <param name="host">The host.</param>
        public void AddHost(Uri host) {
            AddHost(host.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped));
        }

        /// <summary>
        ///     Adds a host to the source-list.
        /// </summary>
        /// <param name="host">The host.</param>
        public void AddHost(string host) {
            ThrowIfNoneIsSet();
            host.MustNotNull("host");
            host.MustNotBeWhiteSpaceOrEmpty("host");
            var hostSource = new HostSource(host);
            mHosts.Add(hostSource);
        }

        private string BuildHostValues() {
            return string.Join(" ", mHosts).PercentEncode();
        }
    }
}