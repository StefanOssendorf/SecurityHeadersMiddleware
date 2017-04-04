using System;
using FluentAssertions;
using Xunit;

namespace SecurityHeaders.Tests {
    public class XXssProtectionHeaderValueTests {

        public class EnabledAnReportShouldThrowPreconditionException {
            [Fact]
            public void When_url_string_is_null() {
                Action action = () => XssProtectionHeaderValue.EnabledAndReport((string) null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void When_url_uri_is_null() {
                Action action = () => XssProtectionHeaderValue.EnabledAndReport((Uri)null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void When_url_is_empty() {
                Action action = () => XssProtectionHeaderValue.EnabledAndReport("");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void When_url_consists_only_of_whiteSpaces() {
                Action action = () => XssProtectionHeaderValue.EnabledAndReport("      ");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void When_url_is_malformed() {
                Action action = () => XssProtectionHeaderValue.EnabledAndReport("1http://www.example.org");
                action.ShouldThrow<FormatException>();
            }
        }

        [Fact]
        public void When_disabled_the_value_should_be_0() {
            var header = XssProtectionHeaderValue.Disabled();

            header.HeaderValue.Should().Be("0");
        }
    }
}