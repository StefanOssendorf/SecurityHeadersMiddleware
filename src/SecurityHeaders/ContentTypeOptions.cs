using System;

namespace SecurityHeaders {
    internal class ContentTypeOptions {
        internal const string XContentTypeOptions = "X-Content-Type-Options";
        internal const string XContentTypeOptionsValue = "nosniff";
        private readonly ContentTypeOptionsSettings mSettings;

        public ContentTypeOptions(ContentTypeOptionsSettings settings) {
            mSettings = settings;
        }

        public void ApplyHeader(IHttpContext context) {
            if(!SetHeader(context.HeaderExist)) {
                return;
            }

            Action<string, string> actionToModifyHeader;
            switch(mSettings.HeaderHandling) {
                case ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet:
                    actionToModifyHeader = context.OverrideHeader;
                    break;
                case ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet:
                    actionToModifyHeader = context.AppendToHeader;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown enum-value '{mSettings.HeaderHandling}' of enum ${typeof(ContentTypeOptionsSettings.HeaderControl).FullName}");
            }
            actionToModifyHeader(XContentTypeOptions, XContentTypeOptionsValue);
        }

        private bool SetHeader(Func<string, bool> headerExist) {
            if(mSettings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return true;
            }
            return !headerExist(XContentTypeOptions);
        }
    }
}