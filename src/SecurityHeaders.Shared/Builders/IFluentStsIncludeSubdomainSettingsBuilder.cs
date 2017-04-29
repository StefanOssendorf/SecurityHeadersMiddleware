namespace SecurityHeaders.Builders {

    /// <summary>
    /// Fluent interface for the "includeSubDomains" value of the Strict-Transport-Security settings.
    /// </summary>
    public interface IFluentStsIncludeSubdomainSettingsBuilder : IHideObjectMethods {
        /// <summary>
        /// Set the "includeSubDomains" value.
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentStsPreloadSettingsBuilder IncludeSubdomains();

        /// <summary>
        /// Do not set the "includeSubDomains" value.
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentStsPreloadSettingsBuilder NotIncludingSubdomains();
    }
}
