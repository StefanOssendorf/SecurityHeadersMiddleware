namespace SecurityHeaders.Owin {
    public static partial class SecurityHeaders {
        private static OwinContextInternal AsInternalCtx(this OwinContext source) {
            return new OwinContextInternal(source);
        }
    }
}