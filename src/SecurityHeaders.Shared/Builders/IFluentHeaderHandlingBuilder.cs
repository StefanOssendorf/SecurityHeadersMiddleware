namespace SecurityHeaders.Builders {

    /// <summary>
    /// Fluent interface for setting the header handling setting.
    /// </summary>
    public interface IFluentHeaderHandlingBuilder : IHideObjectMethods {
        
        /// <summary>
        /// The header should be overwritten when present.
        /// </summary>
        void OverwriteHeaderIfHeaderIsPresent();

        /// <summary>
        /// The header should not be added if already present.
        /// </summary>
        void IgnoreIfHeaderIsPresent();
    }
}