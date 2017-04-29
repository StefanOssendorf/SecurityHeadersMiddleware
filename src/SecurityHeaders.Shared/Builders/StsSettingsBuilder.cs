using System;

namespace SecurityHeaders.Builders {
    internal class StsSettingsBuilder : IFluentStsMaxAgeSettingsBuilder, IFluentStsIncludeSubdomainSettingsBuilder, IFluentStsPreloadSettingsBuilder,
                                        IFluentHeaderHandlingBuilder, IFluentStsRedirectSettingsBuilder, IFluentBuilder<StrictTransportSecuritySettings> {
        private uint mMaxAge = 31536000; // 12 Month
        private bool mIncludeSubdomains = true;
        private bool mPreload = false;
        private StrictTransportSecuritySettings.HeaderControl mHeaderHandling = StrictTransportSecuritySettings.HeaderControl.OverwriteIfHeaderAlreadySet;
        private bool mRedirect = false;
        private Func<Uri, Uri> mRedirectBuilder;


        public IFluentStsIncludeSubdomainSettingsBuilder WithMaxAge(TimeSpan maxAge) {
            Guard.AtLeast((int)maxAge.TotalSeconds, 0, nameof(maxAge));
            mMaxAge = (uint)maxAge.TotalSeconds;
            return this;
        }
        public IFluentStsIncludeSubdomainSettingsBuilder WithMaxAge(uint maxAge) {
            mMaxAge = maxAge;
            return this;
        }

        public IFluentStsPreloadSettingsBuilder IncludeSubdomains() => this;
        public IFluentStsPreloadSettingsBuilder NotIncludingSubdomains() {
            mIncludeSubdomains = false;
            return this;
        }

        public IFluentStsRedirectSettingsBuilder WithPreload() {
            mPreload = true;
            return this;
        }
        public IFluentStsRedirectSettingsBuilder WithoutPreload() => this;

        public void IgnoreIfHeaderIsPresent() => mHeaderHandling = StrictTransportSecuritySettings.HeaderControl.IgnoreIfHeaderAlreadySet;
        public void OverwriteHeaderIfHeaderIsPresent() {
        }

        public IFluentHeaderHandlingBuilder RedirectUnsecureToSecureRequests() => RedirectUnsecureToSecureRequests(uri => {
            var builder = new UriBuilder(uri) {
                Scheme = "https",
                Port = -1
            };
            return builder.Uri;
        });

        public IFluentHeaderHandlingBuilder RedirectUnsecureToSecureRequests(Func<Uri, Uri> redirectToSecureRequestBuilder) {
            Guard.NotNull(redirectToSecureRequestBuilder, nameof(redirectToSecureRequestBuilder));
            mRedirect = true;
            mRedirectBuilder = redirectToSecureRequestBuilder;
            return this;
        }

        public StrictTransportSecuritySettings Build() {
            return new StrictTransportSecuritySettings(StrictTransportSecurityHeaderValue.Create(mMaxAge, mIncludeSubdomains, mPreload), mHeaderHandling, mRedirect, mRedirectBuilder);
        }
    }
}
