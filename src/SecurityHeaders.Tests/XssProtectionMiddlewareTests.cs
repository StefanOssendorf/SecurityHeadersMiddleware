using System;
using Shouldly;
using Xunit;

namespace SecurityHeaders.Tests {
    public class XssProtectionMiddlewareTests {
        [Fact]
        public void When_header_already_exist_it_should_not_be_added_or_overriden() {
            var cto = CreateXp(XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet);
            var ctx = new TestContext {
                HeaderExistFunc = _ => true,
                OverrideHeaderValueAction = Helper.IoExceptionThrower,
                AppendHeaderValueAction = Helper.IoExceptionThrower
            };

            Action sut = () => cto.ApplyHeader(ctx);
            sut.ShouldNotThrow();
        }

        [Theory]
        [InlineData(XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet, false)]
        [InlineData(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet, false)]
        [InlineData(XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet, true)]
        [InlineData(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet, true)]
        public void AppendHeaderValue_should_never_be_called(XssProtectionSettings.HeaderControl handling, bool headerExist) {
            var xpm = CreateXp(handling);
            bool appendValueCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => headerExist,
                OverrideHeaderValueAction = (a, b) => { },
                AppendHeaderValueAction = (a, b) => appendValueCalled = true
            };

            xpm.ApplyHeader(ctx);

            appendValueCalled.ShouldBeFalse();
        }

        [Theory]
        [InlineData(XssProtectionSettings.HeaderControl.IgnoreIfHeaderAlreadySet)]
        [InlineData(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet)]
        public void When_header_does_not_exist_it_should_be_added(XssProtectionSettings.HeaderControl handling) {
            var xpm = CreateXp(handling);
            bool overrideCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => overrideCalled = true
            };
            xpm.ApplyHeader(ctx);
            overrideCalled.ShouldBeTrue("Override header should be called");
        }

        [Fact]
        public void When_adding_the_header_the_correct_headerName_should_be_given() {
            var xpm = CreateXp(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            string headerName = "";
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => headerName = a
            };

            xpm.ApplyHeader(ctx);

            headerName.ShouldBe("X-Xss-Protection");
        }

        [Fact]
        public void When_disabled_it_should_be_0() {
            string actualValueToBeAdded = "";
            var middleware = CreateXp(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet, XssProtectionHeaderValue.Disabled());
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.ShouldBe("0");
        }

        [Fact]
        public void When_enabled_it_should_be_1() {
            string actualValueToBeAdded = "";
            var middleware = CreateXp(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet, XssProtectionHeaderValue.Enabled());
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.ShouldBe("1");
        }

        [Fact]
        public void When_enabled_and_block_it_should_be_1_and_block_mode() {
            string actualValueToBeAdded = "";
            var middleware = CreateXp(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet, XssProtectionHeaderValue.EnabledAndBlock());
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.ShouldBe("1; mode=block");
        }

        [Fact]
        public void When_enabled_and_report_it_should_be_1_and_have_report_url() {
            string actualValueToBeAdded = "";
            string reportUrl = "http://www.exmampe.com";

            var middleware = CreateXp(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet, XssProtectionHeaderValue.EnabledAndReport(reportUrl));
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.ShouldBe($"1; report={reportUrl}");
        }

        private static XssProtectionMiddleware CreateXp(XssProtectionSettings.HeaderControl headerControl) {
            return CreateXp(headerControl, XssProtectionHeaderValue.Disabled());
        }

        private static XssProtectionMiddleware CreateXp(XssProtectionSettings.HeaderControl headerControl, XssProtectionHeaderValue headerValue) {
            return new XssProtectionMiddleware(new XssProtectionSettings(headerValue, headerControl));
        }
    }
}