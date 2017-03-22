using System;
using Shouldly;
using Xunit;

namespace SecurityHeaders.Tests {

    public class ContentTypeOptionsTests {
        [Fact]
        public void When_header_already_exist_it_should_not_be_added_or_overriden() {
            var cto = CreateCto(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet);
            var ctx = new TestContext {
                HeaderExistFunc = _ => true,
                OverrideHeaderValueAction = Helper.IoExceptionThrower,
                AppendHeaderValueAction = Helper.IoExceptionThrower
            };

            Action sut = () => cto.ApplyHeader(ctx);
            sut.ShouldNotThrow();
        }

        [Theory]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet, false)]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet, false)]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet, true)]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet, true)]
        public void AppendHeaderValue_should_never_be_called(ContentTypeOptionsSettings.HeaderControl handling, bool headerExist) {
            var cto = CreateCto(handling);
            bool appendValueCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => headerExist,
                OverrideHeaderValueAction = (a, b) => { },
                AppendHeaderValueAction = (a, b) => appendValueCalled = true
            };

            appendValueCalled.ShouldBeFalse();
        }

        [Theory]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet)]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet)]
        public void When_header_does_not_exist_it_should_be_added(ContentTypeOptionsSettings.HeaderControl handling) {
            var cto = CreateCto(handling);
            bool overrideCalled = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => overrideCalled = true
            };
            cto.ApplyHeader(ctx);
            overrideCalled.ShouldBeTrue("Either Override or Append header should be called");
        }

        [Fact]
        public void When_adding_the_header_the_correct_headerName_should_be_given() {
            var cto = CreateCto(ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            string headerName = "";
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => headerName = a
            };

            cto.ApplyHeader(ctx);

            headerName.ShouldBe("X-Content-Type-Options");
        }

        [Fact]
        public void When_adding_the_header_the_correct_headerValue_should_be_added() {
            var cto = CreateCto(ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
            string headerValue = "";
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => headerValue = b
            };

            cto.ApplyHeader(ctx);

            headerValue.ShouldBe("nosniff");
        }

        private static ContentTypeOptionsMiddleware CreateCto(ContentTypeOptionsSettings.HeaderControl headerHandling) => new ContentTypeOptionsMiddleware(new ContentTypeOptionsSettings(headerHandling));
    }
}