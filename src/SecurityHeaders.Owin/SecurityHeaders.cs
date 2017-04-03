namespace SecurityHeaders.Owin {

    /// <summary>
    ///     OWIN extension methods.
    /// </summary>
    public static partial class SecurityHeaders {
        private static OwinHttpContext AsInternalCtx(this OwinContext source) {
            return new OwinHttpContext(source);
        }
    }
}