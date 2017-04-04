using FluentAssertions;
using Xunit;
namespace SecurityHeaders.Tests {
    public class AntiClickjackingSettingsTests {
        [Fact]
        public void Default_ctor_should_have_the_expected_default_values() {
            var settings = new AntiClickjackingSettings();
            settings.HeaderHandling.Should().Be(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            settings.HeaderValue.Should().Be(XFrameOptionHeaderValue.Deny());
        }
    }

    public class XssProtectionSettingsTests {
        [Fact]
        public void Default_ctor_should_have_the_expected_default_values() {
            var settings = new XssProtectionSettings();
            settings.HeaderHandling.Should().Be(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            settings.HeaderValue.Should().Be(XssProtectionHeaderValue.EnabledAndBlock());
        }
    }
}