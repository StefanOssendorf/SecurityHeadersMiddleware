using System;

namespace SecurityHeaders {
    
    /// <summary>
    /// Defines the interface which all header middlewares will use to work.
    /// </summary>
    internal interface IHttpContext {
        
        /// <summary>
        /// Checks if the header is already present in the response or not.
        /// </summary>
        /// <param name="headerName">The header name to check for.</param>
        /// <returns><code>true</code>, if header already exist. Otherwise <code>false</code>.</returns>
        bool HeaderExist(string headerName);
        
        /// <summary>
        /// Writes the value to the header and overrides, if present, the current value.
        /// </summary>
        /// <param name="headerName">The header name.</param>
        /// <param name="value">The value for the header.</param>
        void SetHeader(string headerName, string value);

        /// <summary>
        /// Get if the underlying connection is secure.
        /// </summary>
        bool IsSecure { get; }

        /// <summary>
        /// Get the uri of the request.
        /// </summary>
        Uri RequestUri { get; }

        /// <summary>
        /// Set the response to permanent redirect (301).
        /// </summary>
        /// <param name="redirectedTo">The url where the redirect point to.</param>
        void PermanentRedirectTo(Uri redirectedTo);
    }
}