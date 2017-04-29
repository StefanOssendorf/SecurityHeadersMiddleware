using System;

namespace SecurityHeaders.Builders {

    /// <summary>
    /// Fluent interface for the max-age directive of the Strict-Transport-Security settings.
    /// </summary>
    public interface IFluentStsMaxAgeSettingsBuilder : IHideObjectMethods {

        /// <summary>
        /// Set the max-age value to the provided value.
        /// </summary>
        /// <param name="maxAge">The max-age value in seconds.</param>
        /// <returns>Next step in settings configuration.</returns>
        IFluentStsIncludeSubdomainSettingsBuilder WithMaxAge(uint maxAge);

        /// <summary>
        /// Set the max-age value to the provided value.
        /// </summary>
        /// <param name="maxAge">The max-age value in seconds. <see cref="TimeSpan.TotalMinutes"/> will be rounded.</param>
        /// <exception cref="ArgumentOutOfRangeException">The value of the <paramref name="maxAge"/> must be greater or equal to 0.</exception>
        /// <returns>Next step in settings configuration.</returns>
        IFluentStsIncludeSubdomainSettingsBuilder WithMaxAge(TimeSpan maxAge);
    }
}
