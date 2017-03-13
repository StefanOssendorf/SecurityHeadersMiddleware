namespace SecurityHeaders.Owin {
    internal class OwinContextInternal : IHttpContext {
        private readonly OwinContext mContext;

        public OwinContextInternal(OwinContext context) {
            mContext = context;
        }

        public bool HeaderExist(string headerName) => mContext.Response.Headers.ContainsKey(headerName);


        public void OverrideHeader(string headerName, string value) => mContext.Response.Headers.Set(headerName, value);


        public void AppendToHeader(string headerName, string value) => mContext.Response.Headers.Append(headerName, value);
    }
}