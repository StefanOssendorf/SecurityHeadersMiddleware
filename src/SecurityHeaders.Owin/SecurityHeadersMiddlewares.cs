namespace SecurityHeaders.Owin {
    public static partial class SecurityHeadersMiddlewares {
        private static OwinContextInternal AsInternalCtx(this OwinContext source) {
            return new OwinContextInternal(source);
        }
    }
}