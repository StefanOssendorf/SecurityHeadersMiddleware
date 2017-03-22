using System;
using Shouldly;
using Xunit;

namespace SecurityHeaders.Tests {
    public class XFrameOptionHeaderValueTests {
        public class CreatingAnAllowFromShouldThrowAPreconditionException {
            [Fact]
            public void When_string_is_null() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom((string)null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void When_uri_is_null() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom((Uri)null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void When_string_is_empty() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom("");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void When_string_consists_only_of_whiteSpaces() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom("        ");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void When_uri_is_malformed() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom("1http://www.example.org");
                action.ShouldThrow<FormatException>();
            }
        }
    }
}
