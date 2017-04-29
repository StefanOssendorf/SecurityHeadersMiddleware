using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Headers;

namespace SecurityHeaders.AspNetCore {

    /// <summary>
    /// Represents the ASP.Net Core HttpContext-Wrapper.
    /// </summary>
    public class AspNetCoreHttpContext : IHttpContext {
        private readonly HttpContext mContext;
        private IHeaderDictionary Headers => mContext.Response.Headers;
        private ResponseHeaders TypedHeaders => mContext.Response.GetTypedHeaders();

        /// <summary>
        /// Initializes a new <see cref="AspNetCoreHttpContext"/>-Instance.
        /// </summary>
        /// <param name="context">The underlying ASP.Net Core context.</param>
        public AspNetCoreHttpContext(HttpContext context) {
            mContext = context;
        }

        /// <inheritdoc />
        public bool HeaderExist(string headerName) => Headers.ContainsKey(headerName);

        /// <inheritdoc />
        public bool IsSecure => mContext.Request.IsHttps;

        /// <inheritdoc />
        public Uri RequestUri => new Uri(mContext.Request.GetEncodedUrl());

        /// <inheritdoc />
        public void PermanentRedirectTo(Uri redirectedTo) => mContext.Response.Redirect(redirectedTo.ToString(), true);

        /// <inheritdoc />
        public void SetHeader(string headerName, string value) {
            if(Headers.ContainsKey(headerName)) {
                Headers.Remove(headerName);
            }

            TypedHeaders.Set(headerName, value);
        }
    }
}
