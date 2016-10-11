using System;

namespace SecurityHeaders.Owin {
    internal class OwinContextInternal : IHttpContext {
        private readonly OwinContext mContext;

        public OwinContextInternal(OwinContext context) {
            mContext = context;
        }

        public bool HeaderExist(string headerName) => mContext.Response.Headers.ContainsKey(headerName);


        public void OverrideHeader(string name, string value) => mContext.Response.Headers.Set(name, value);


        public void AppendToHeader(string name, string value) => mContext.Response.Headers.Append(name, value);
    }
}