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
        public void When_add_0_it_should_be_0() {
            string actualValueToBeAdded = "";
            var middleware = CreateXp(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.ShouldBe("0");
        }

        [Fact]
        public void When_adding_the_header_the_correct_headerValue_should_be_added() {
            var xpm = CreateXp(XssProtectionSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            string headerValue = "";
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => headerValue = b
            };

            xpm.ApplyHeader(ctx);

            headerValue.ShouldBe("nosniff");
        }

        private XssProtectionMiddleware CreateXp(XssProtectionSettings.HeaderControl headerControl) {
            return new XssProtectionMiddleware();
        }
    }
}