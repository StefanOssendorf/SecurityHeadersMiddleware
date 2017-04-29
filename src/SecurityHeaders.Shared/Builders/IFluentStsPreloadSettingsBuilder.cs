namespace SecurityHeaders.Builders {
    /// <summary>
    /// Fluent interface for the "preload" value of the Strict-Transport-Security settings.
    /// </summary>
    public interface IFluentStsPreloadSettingsBuilder : IHideObjectMethods {

        /// <summary>
        /// Set the "preload" value.
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentStsRedirectSettingsBuilder WithPreload();

        /// <summary>
        /// Do not set the "preload" value.
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentStsRedirectSettingsBuilder WithoutPreload();
    }
}
