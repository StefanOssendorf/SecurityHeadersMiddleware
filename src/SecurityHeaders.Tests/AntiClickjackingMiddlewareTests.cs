using System;
using Shouldly;
using Xunit;

namespace SecurityHeaders.Tests {
    public class AntiClickjackingMiddlewareTests {
        [Fact]
        public void WhenHeaderAlreadyExistItShouldNotBeAddedOrOverridden() {
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
        [InlineData(AntiClickjackingSettings.HeaderControl.IgnoreIfHeaderAlreadySet)]
        [InlineData(AntiClickjackingSettings.HeaderControl.OverwriteIfHeaderAlreadySet)]
        public void WhenHeaderDoesNotExistItShouldBeAdded(AntiClickjackingSettings.HeaderControl headerHandling) {
            var middleware = CreateXfo(headerHandling);
            bool overrideCalled = false;
            bool appendCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => overrideCalled = true,
                AppendHeaderValueAction = (a, b) => appendCalled = true
            };
            middleware.ApplyHeader(ctx);
            bool eitherOrCalled = overrideCalled ^ appendCalled;
            eitherOrCalled.ShouldBeTrue("Either Override or Append header should be called");
        }

        private static AntiClickjackingMiddleware CreateXfo(AntiClickjackingSettings.HeaderControl headerHandling) {
            return new AntiClickjackingMiddleware(new AntiClickjackingSettings(XFrameOptionHeaderValue.Deny(), headerHandling));
        }
    }
}