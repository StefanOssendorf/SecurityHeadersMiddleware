using System;
using FluentAssertions;
using Xunit;

namespace SecurityHeaders.Tests {
    public class AntiClickjackingMiddlewareTests {
        [Fact]
        public void When_header_already_exist_it_should_not_be_added_or_overwritten() {
            var middleware = CreateXfo(AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet);

            var ctx = new TestContext() {
                HeaderExistFunc = _ => true,
                AppendHeaderValueAction = Helper.IoExceptionThrower,
                OverrideHeaderValueAction = Helper.IoExceptionThrower
            };

            Action sut = () => middleware.ApplyHeader(ctx);
            sut.ShouldNotThrow();
        }

        [Theory]
        [InlineData(AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet, false)]
        [InlineData(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet, false)]
        [InlineData(AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet, true)]
        [InlineData(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet, true)]
        public void AppendHeaderValue_should_never_be_called(AntiClickjackingSettings.HeaderControl handling, bool headerExist) {
            var cto = CreateXfo(handling);
            bool appendValueCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => headerExist,
                OverrideHeaderValueAction = (a, b) => { },
                AppendHeaderValueAction = (a, b) => appendValueCalled = true
            };

            cto.ApplyHeader(ctx);

            appendValueCalled.Should().BeFalse();
        }

        [Theory]
        [InlineData(AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet)]
        [InlineData(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet)]
        public void When_header_does_not_exist_it_should_be_added(AntiClickjackingSettings.HeaderControl headerHandling) {
            var middleware = CreateXfo(headerHandling);
            bool overrideCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => overrideCalled = true
            };
            middleware.ApplyHeader(ctx);

            overrideCalled.Should().BeTrue("Override header should be called");
        }

        [Fact]
        public void When_adding_the_header_the_correct_headerName_should_be_given() {
            var cto = CreateXfo(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            string headerName = "";
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => headerName = a
            };

            cto.ApplyHeader(ctx);

            headerName.Should().Be("X-Frame-Options");
        }

        [Fact]
        public void When_add_deny_it_should_set_deny() {
            string actualValueToBeAdded = "";
            var middleware = CreateXfo(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet, AntiClickjackingHeaderValue.Deny());
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.Should().Be("DENY");
        }

        [Fact]
        public void When_add_sameOrigin_it_should_set_sameOrigin() {
            string actualValueToBeAdded = "";
            var middleware = CreateXfo(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet, AntiClickjackingHeaderValue.SameOrigin());
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.Should().Be("SAMEORIGIN");
        }

        [Fact]
        public void When_add_allowFrom_it_should_set_allowFrom_with_origin() {
            string actualValueToBeAdded = "";
            var middleware = CreateXfo(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet, AntiClickjackingHeaderValue.AllowFrom("http://www.example.org"));
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => actualValueToBeAdded = b
            };

            middleware.ApplyHeader(ctx);

            actualValueToBeAdded.Should().Be("ALLOW-FROM http://www.example.org");
        }

        private static AntiClickjackingMiddleware CreateXfo(AntiClickjackingSettings.HeaderControl headerHandling) {
            return CreateXfo(headerHandling, AntiClickjackingHeaderValue.Deny());
        }
        private static AntiClickjackingMiddleware CreateXfo(AntiClickjackingSettings.HeaderControl headerHandling, AntiClickjackingHeaderValue headerValue) {
            return new AntiClickjackingMiddleware(new AntiClickjackingSettings(headerValue, headerHandling));
        }
    }
}