using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityHeaders.Builders {

    /// <summary>
    /// Fluent interface to set the max-age value of the Strict-Transport-Security settings.
    /// </summary>
    public interface IFluentStsMaxAgeSettingsBuilder {

        /// <summary>
        /// Set the max-age value to the provided value.
        /// </summary>
        /// <param name="maxAge">The max-age value in seconds.</param>
        IFluentStsIncludeSubdomainSettingsBuilder WithMaxAge(uint maxAge);

        /// <summary>
        /// Set the max-age value to the provided value.
        /// </summary>
        /// <param name="maxAge">The max-age value in seconds.</param>
        /// <exception cref="ArgumentOutOfRangeException">The value of the <paramref name="maxAge"/> must be greater or equal to 0.</exception>
        IFluentStsIncludeSubdomainSettingsBuilder WithMaxAge(TimeSpan maxAge);
    }

    public interface IFluentStsIncludeSubdomainSettingsBuilder {
        IFluentStsPreloadSettingsBuilder IncludeSubdomains();

        IFluentStsPreloadSettingsBuilder NotIncludingSubdomains();
    }

    public interface IFluentStsPreloadSettingsBuilder {
        IFluentHeaderHandlingBuilder UsePreload();
        IFluentHeaderHandlingBuilder WithoutPreload();
    }
}
