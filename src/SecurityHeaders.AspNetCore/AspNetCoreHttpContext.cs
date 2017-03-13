using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace SecurityHeaders.AspNetCore {
    internal class AspNetCoreHttpContext : IHttpContext {
        private readonly HttpContext mContext;
        private IHeaderDictionary Headers => mContext.Response.Headers;
        private ResponseHeaders TypedHeaders => mContext.Response.GetTypedHeaders();

        public AspNetCoreHttpContext(HttpContext context) {
            mContext = context;
        }

        public bool HeaderExist(string headerName) => Headers.ContainsKey(headerName);

        public void AppendToHeader(string headerName, string value) => TypedHeaders.Append(headerName, value);

        public void OverrideHeader(string headerName, string value) {
            if(Headers.ContainsKey(headerName)) {
                Headers.Remove(headerName);
            }

            TypedHeaders.Set(headerName, value);
        }
    }
}
