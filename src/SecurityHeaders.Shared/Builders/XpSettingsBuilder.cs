using System;

namespace SecurityHeaders.Builders {
    internal class XpSettingsBuilder : IFluentXpSettingsBuilder, IFluentHeaderHandlingBuilder, IFluentBuilder<XssProtectionSettings> {
        private XssProtectionSettings.HeaderControl mHeaderHandling = XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet;
        private XssProtectionHeaderValue mHeaderValue = XssProtectionHeaderValue.EnabledAndBlock();

        public IFluentHeaderHandlingBuilder Disabled() {
            mHeaderValue = XssProtectionHeaderValue.Disabled();
            return this;
        }

        public IFluentHeaderHandlingBuilder Enabled() {
            mHeaderValue = XssProtectionHeaderValue.Enabled();
            return this;
        }

        public IFluentHeaderHandlingBuilder EnabledAndBlock() => this; // Default used

        public IFluentHeaderHandlingBuilder EnabledAndReport(Uri url) {
            Guard.NotNull(url, nameof(url));
            mHeaderValue = XssProtectionHeaderValue.EnabledAndReport(url);
            return this;
        }

        public void IgnoreIfHeaderIsPresent() => mHeaderHandling = XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet;

        public void OverwriteHeaderIfHeaderIsPresent() {
            // Default used
        }

        public XssProtectionSettings Build() => new XssProtectionSettings(mHeaderValue, mHeaderHandling);
    }
}
