using System;
using System.Collections.Generic;
using System.Text;
using SecurityHeadersMiddleware.Infrastructure;

namespace SecurityHeadersMiddleware {
    /// <summary>
    ///     Represents a sandbox-token-list according to the CSP specification (http://www.w3.org/TR/CSP2/#directive-sandbox).
    /// </summary>
    public class CspSandboxTokenList : IDirectiveValueBuilder {
        private readonly List<string> mTokens;
        /// <summary>
        ///     Initializes a new instance of the <see cref="CspSandboxTokenList" /> class.
        /// </summary>
        public CspSandboxTokenList() {
            mTokens = new List<string>();
            IsEmpty = false;
        }
        internal bool IsEmpty { get; private set; }

        /// <summary>
        ///     Creates the directive header value.
        /// </summary>
        /// <returns>The directive header value without directive-name.</returns>
        public string ToDirectiveValue() {
            if (IsEmpty) {
                return "";
            }
            var sb = new StringBuilder();
            foreach (string token in mTokens) {
                sb.AppendFormat(" {0} ", token);
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        ///     Adds the keyword to the sandbox-token-list. <br />
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        public void AddKeyword(SandboxKeyword keyword) {
            string token = TokenValueOfKeyword(keyword);
            AddToken(token);
        }

        /// <summary>
        ///     Adds a token to the sandbox-token-list.
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
                const string msg = "Token value '{0}' is invalid.{1}" +
                                   "Valid tokens: allow-forms or abcedfg{1}" +
                                   "All characters must be a-z, A-Z, 0-9 or !, #, $, %, &, \\, *, +, -, ., ^, _, `, |, ~ " +
                                   "For more Information see: {2}";
                throw new FormatException(msg.FormatWith(token, Environment.NewLine, "http://www.w3.org/TR/CSP2/#directive-sandbox"));
            }
            mTokens.Add(token);
            IsEmpty = false;
        }

        /// <summary>
        ///     Sets the value to an empty token. <br />
        ///     According to http://lists.w3.org/Archives/Public/public-webappsec/2014Aug/0019.html and
        ///     http://developers.whatwg.org/the-iframe-element.html#attr-iframe-sandbox it's an valid exception.
        /// </summary>
        public void SetToEmptyValue() {
            mTokens.Clear();
            IsEmpty = true;
        }

        private static string TokenValueOfKeyword(SandboxKeyword keyword) {
            switch (keyword) {
                case SandboxKeyword.AllowForms:
                    return "allow-forms";
                case SandboxKeyword.AllowPointerLock:
                    return "allow-pointer-lock";
                case SandboxKeyword.AllowPopups:
                    return "allow-popups";
                case SandboxKeyword.AllowSameOrigin:
                    return "allow-same-origin";
                case SandboxKeyword.AllowScripts:
                    return "allow-scripts";
                case SandboxKeyword.AllowTopNavigation:
                    return "allow-top-navigation";
                default:
                    throw new ArgumentOutOfRangeException("keyword");
            }
        }
    }
}