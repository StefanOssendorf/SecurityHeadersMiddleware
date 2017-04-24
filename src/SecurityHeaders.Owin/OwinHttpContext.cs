using System;
using System.Collections.Generic;
using SecurityHeaders.Owin.Infrastructure;

namespace SecurityHeaders.Owin {
    /// <summary>
    /// Represents the OWIN Environment-Wrapper.
    /// </summary>
    internal class OwinHttpContext : IHttpContext {
        private readonly OwinContext mContext;

        private const string LocationHeaderName = "Location";

        internal OwinHttpContext(OwinContext context) {
            mContext = context;
        }

        /// <inheritdoc />
        public bool HeaderExist(string headerName) => mContext.Response.Headers.ContainsKey(headerName);


        /// <inheritdoc />
        public void OverrideHeader(string headerName, string value) => mContext.Response.Headers.Set(headerName, value);


        /// <inheritdoc />
        public void AppendToHeader(string headerName, string value) => mContext.Response.Headers.Append(headerName, value);

        /// <inheritdoc />
        public bool IsSecure => mContext.Request.IsSecure;

        /// <inheritdoc />
        public Uri RequestUri => mContext.Request.Uri;

        /// <inheritdoc />
        public void PermanentRedirectTo(Uri redirectedTo) {
            var response = mContext.Response;
            response.StatusCode = 301;
            response.Headers[LocationHeaderName] = redirectedTo.ToString();
        }
    }
}