using System;

namespace SecurityHeaders {

    /// <summary>
    ///  The middleware to apply the X-Frame-Options header.
    /// </summary>
    public class AntiClickjackingMiddleware {
        /// <summary>
        /// The http-header name of the x-frame-options header.
        /// </summary>
        public const string XFrameOptionsHeaderName = "X-Frame-Options";

        private readonly AntiClickjackingSettings mSettings;

        /// <summary>
        /// Initializes a new instance of <see cref="AntiClickjackingMiddleware"/>.
        /// </summary>
        /// <param name="settings">The settings. Must not be <code>null</code>.</param>
        public AntiClickjackingMiddleware(AntiClickjackingSettings settings) {
            Guard.NotNull(settings, nameof(settings));
            mSettings = settings;
        }

        /// <summary>
        /// Applies the middleware on the context.
        /// </summary>
        /// <param name="context">The context. Must not be <code>null</code>.</param>
        public void ApplyHeader(IHttpContext context) {
            Guard.NotNull(context, nameof(context));

            if(!SetHeader(context.HeaderExist)) {
                return;
            }

            Action<string, string> actionToModifyHeader;
            switch(mSettings.HeaderHandling) {
                case AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet:
                    actionToModifyHeader = context.OverrideHeader;
                    break;
                case AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet:
                    actionToModifyHeader = context.AppendToHeader;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown enum-value '{mSettings.HeaderHandling}' of enum ${typeof(AntiClickjackingSettings.HeaderControl).FullName}");
            }
            actionToModifyHeader(XFrameOptionsHeaderName, mSettings.HeaderValue);
        }

        private bool SetHeader(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return true;
            }
            return !headerExist(XFrameOptionsHeaderName);
        }
    }
}
