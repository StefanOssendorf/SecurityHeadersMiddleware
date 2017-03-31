using System.Collections.Generic;

namespace SecurityHeaders.Owin {
    /// <summary>
    /// Represents the OWIN Environment-Wrapper.
    /// </summary>
    public class OwinHttpContext : IHttpContext {
        private readonly OwinContext mContext;

        /// <summary>
        /// Initializes a new <see cref="OwinHttpContext"/>-Instance.
        /// </summary>
        /// <param name="environment">The current OWIN environment.</param>
        public OwinHttpContext(IDictionary<string, object> environment) : this(environment.AsOwinContext()) {
        }

        internal OwinHttpContext(OwinContext context) {
            mContext = context;
        }

        /// <inheritdoc />
        public bool HeaderExist(string headerName) => mContext.Response.Headers.ContainsKey(headerName);


        /// <inheritdoc />
        public void OverrideHeader(string headerName, string value) => mContext.Response.Headers.Set(headerName, value);


        /// <inheritdoc />
        public void AppendToHeader(string headerName, string value) => mContext.Response.Headers.Append(headerName, value);
    }
}