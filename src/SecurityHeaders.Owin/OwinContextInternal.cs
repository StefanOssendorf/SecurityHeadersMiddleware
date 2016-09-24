using System;

namespace SecurityHeaders.Owin {
    internal class OwinContextInternal : IHttpContext {
        private readonly OwinContext mContext;

        public OwinContextInternal(OwinContext context) {
            mContext = context;
        }

        public bool HeaderExist(string headerName) {
            throw new NotImplementedException();
        }

        public void OverrideHeader(string name, string value) {
            throw new NotImplementedException();
        }

        public void AppendToHeader(string name, string value) {
            throw new NotImplementedException();
        }
    }
}