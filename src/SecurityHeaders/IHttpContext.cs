namespace SecurityHeaders {
    /// <summary>
    /// Defines the interface which all header middlewares will use to work.
    /// </summary>
    public interface IHttpContext {
        
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
        void OverrideHeader(string headerName, string value);
        
        /// <summary>
        /// Appends the value to the header. If the header does not exist, it is addded.
        /// </summary>
        /// <param name="headerName">The header name.</param>
        /// <param name="value">The value for the header.</param>
        void AppendToHeader(string headerName, string value);
    }
}