using System;
using Shouldly;
using Xunit;

namespace SecurityHeaders.Core.Tests {
    public class ContentTypeOptionsTests {
        private static readonly Action<string, string> IoExceptionThrower = (a, b) => { throw new InvalidOperationException(); };

        [Fact]
        public void When_header_already_exist_it_should_not_be_added_or_overriden() {
            var cto = CreateCto(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet);
            var ctx = new TestContext {
                HeaderExistFunc = _ => true,
                OverrideHeaderValueAction = IoExceptionThrower,
                AppendHeaderValueAction = IoExceptionThrower
            };

            cto.ApplyHeader(ctx);
        }

        [Theory]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet)]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet)]
        public void When_header_does_not_exist_it_should_be_added(ContentTypeOptionsSettings.HeaderControl handling) {
            var cto = CreateCto(handling);
            bool overrideCalled = false;
            bool appendCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => overrideCalled = true,
                AppendHeaderValueAction = (a, b) => appendCalled = true
            };
            cto.ApplyHeader(ctx);
            bool eitherOrCalled = overrideCalled ^ appendCalled;
            eitherOrCalled.ShouldBeTrue("Either Override or Append header should be called");
        }

        private static ContentTypeOptionsMiddleware CreateCto(ContentTypeOptionsSettings.HeaderControl headerHandling) {
            return new ContentTypeOptionsMiddleware(new ContentTypeOptionsSettings {
                HeaderHandling = headerHandling
            });
        }
    }
}