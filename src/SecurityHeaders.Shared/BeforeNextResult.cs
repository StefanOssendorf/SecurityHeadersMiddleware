namespace SecurityHeaders {
    /// <summary>
    /// Represents the result of the <see cref="StrictTransportSecurityMiddleware.BeforeNext"/> method call.
    /// </summary>
    internal class BeforeNextResult {

        /// <summary>
        /// Get if the <see cref="StrictTransportSecurityMiddleware.BeforeNext"/> call requests an end of the pipeline.
        /// </summary>
        public bool EndPipeline { get; set; }
    }
}
