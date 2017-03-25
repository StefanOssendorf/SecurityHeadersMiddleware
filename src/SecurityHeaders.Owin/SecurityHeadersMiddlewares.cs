namespace SecurityHeaders.Owin {

    /// <summary>
    ///     OWIN extension methods.
    /// </summary>
    public static partial class SecurityHeadersMiddlewares {
        private static OwinContextInternal AsInternalCtx(this OwinContext source) {
            return new OwinContextInternal(source);
        }
    }
}