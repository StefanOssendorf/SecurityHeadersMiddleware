using System;

namespace SecurityHeaders.Builders {

    /// <summary>
    /// Fluent interface to define the way if an url upgrade is handled.
    /// </summary>
    public interface IFluentStsRedirectSettingsBuilder : IFluentHeaderHandlingBuilder, IHideObjectMethods {

        /// <summary>
        /// Redirects a url from http to https with a StatusCode 301 with a location header.
        /// </summary>
        /// <returns>Next step in settings configuration.</returns>
        IFluentHeaderHandlingBuilder RedirectUnsecureToSecureRequests();

        /// <summary>
        /// Redirects a url from http thorugh the defined <paramref name="redirectToSecureRequestBuilder"/> with a StatusCode 301 with a location header.
        /// </summary>
        /// <param name="redirectToSecureRequestBuilder">The func to upgread the url from</param>
        /// <returns>Next step in settings configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="redirectToSecureRequestBuilder"/> is <code>null</code>.</exception>
        IFluentHeaderHandlingBuilder RedirectUnsecureToSecureRequests(Func<Uri,Uri> redirectToSecureRequestBuilder);
    }
}
