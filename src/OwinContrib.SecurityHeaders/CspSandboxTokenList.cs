using System;
using System.Collections.Generic;
using System.Text;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {

    /// <summary>
    /// Represents a sandbox-token-list according to the CSP specification (http://www.w3.org/TR/CSP2/#directive-sandbox).
    /// </summary>
    public class CspSandboxTokenList : IDirectiveValueBuilder {
        private readonly List<string> mTokens;
        /// <summary>
        /// Initializes a new instance of the <see cref="CspSandboxTokenList"/> class.
        /// </summary>
        public CspSandboxTokenList() {
            mTokens = new List<string>();
        }

        /// <summary>
        /// Adds a token to the sandbox-toke-list.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <exception cref="System.FormatException">
        ///     For more information see: http://www.w3.org/TR/CSP2/#sandbox-token
        /// </exception>
        public void AddToken(string token) {
            token.MustNotNull("token");
            token.MustNotBeWhiteSpaceOrEmpty("token");

            if (mTokens.Contains(token)) {
                return;
            }

            if (!Rfc7230Utility.IsToken(token)) {
                throw new FormatException();
            }

            mTokens.Add(token);
        }

        /// <summary>
        /// Creates the directive header value.
        /// </summary>
        /// <returns>The directive header value without directive-name.</returns>
        public string ToDirectiveValue() {
            var sb = new StringBuilder();

            foreach (var token in mTokens) {
                sb.AppendFormat(" {0} ", token);
            }

            return sb.ToString().Trim();
        }
    }
}