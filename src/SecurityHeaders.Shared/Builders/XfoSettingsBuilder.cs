using System;

namespace SecurityHeaders.Builders {
    internal class XfoSettingsBuilder : IFluentXfoSettingsBuilder, IFluentHeaderHandlingBuilder, IFluentBuilder<XFrameOptionsSettings>, IHideObjectMethods {
        private XFrameOptionsSettings.HeaderControl mHeaderHandling = XFrameOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet;
        private XFrameOptionsHeaderValue mHeaderValue = XFrameOptionsHeaderValue.Deny();

        public IFluentHeaderHandlingBuilder AllowFrom(Uri url) {
            Guard.NotNull(url, nameof(url));
            mHeaderValue = XFrameOptionsHeaderValue.AllowFrom(url);
            return this;
        }

        public IFluentHeaderHandlingBuilder Deny() => this; // Default used

        public IFluentHeaderHandlingBuilder SameOrigin() {
            mHeaderValue = XFrameOptionsHeaderValue.SameOrigin();
            return this;
        }

        public void IgnoreIfHeaderIsPresent() => mHeaderHandling = XFrameOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet;

        public void OverwriteHeaderIfHeaderIsPresent() {
            // Default used
        }

        public XFrameOptionsSettings Build() => new XFrameOptionsSettings(mHeaderValue, mHeaderHandling);
    }
}