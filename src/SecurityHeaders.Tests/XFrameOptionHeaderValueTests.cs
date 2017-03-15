using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;

namespace SecurityHeaders.Tests {
    public class XFrameOptionHeaderValueTests {
        public class CreatingAnAllowFromShouldThrowAPreconditionException {
            [Fact]
            public void WhenStringIsNull() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom((string)null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void WhenUriIsNull() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom((Uri)null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void WhenStringIsEmpty() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom("");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void WhenStringConsistsOnlyOfWhiteSpaces() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom("        ");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void WhenUriIsMalformed() {
                Action action = () => XFrameOptionHeaderValue.AllowFrom("1http://www.example.org");
                action.ShouldThrow<FormatException>();
            }
        }
    }
}
