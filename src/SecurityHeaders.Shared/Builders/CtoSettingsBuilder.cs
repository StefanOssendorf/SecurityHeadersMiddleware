namespace SecurityHeaders.Builders {
    internal class CtoSettingsBuilder : IFluentCtoSettingsBuilder, IFluentBuilder<ContentTypeOptionsSettings> {
        private ContentTypeOptionsSettings.HeaderControl mHeaderHandling = ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet;


        public ContentTypeOptionsSettings Build() => new ContentTypeOptionsSettings(mHeaderHandling);

        public void OverwriteHeaderIfHeaderIsPresent() {
            // Default used
        }

        public void IgnoreIfHeaderIsPresent() {
            mHeaderHandling = ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet;
        }
    }
}