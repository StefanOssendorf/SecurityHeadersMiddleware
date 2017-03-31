using Microsoft.AspNetCore.Http;
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
        public void AppendToHeader(string headerName, string value) => TypedHeaders.Append(headerName, value);

        /// <inheritdoc />
        public void OverrideHeader(string headerName, string value) {
            if(Headers.ContainsKey(headerName)) {
                Headers.Remove(headerName);
            }

            TypedHeaders.Set(headerName, value);
        }
    }
}
