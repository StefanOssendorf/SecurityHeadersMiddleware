using System;
using FluentAssertions;
using Xunit;

namespace SecurityHeaders.Tests {
    public class XFrameOptionHeaderValueTests {
        public class CreatingAnAllowFromShouldThrowAPreconditionException {
            [Fact]
            public void When_string_is_null() {
                Action action = () => AntiClickjackingHeaderValue.AllowFrom((string)null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void When_uri_is_null() {
                Action action = () => AntiClickjackingHeaderValue.AllowFrom((Uri)null);
                action.ShouldThrow<ArgumentNullException>();
            }

            [Fact]
            public void When_string_is_empty() {
                Action action = () => AntiClickjackingHeaderValue.AllowFrom("");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void When_string_consists_only_of_whiteSpaces() {
                Action action = () => AntiClickjackingHeaderValue.AllowFrom("        ");
                action.ShouldThrow<ArgumentException>();
            }

            [Fact]
            public void When_uri_is_malformed() {
                Action action = () => AntiClickjackingHeaderValue.AllowFrom("1http://www.example.org");
                action.ShouldThrow<FormatException>();
            }
        }

        [Fact]
        public void When_creating_a_deny_it_should_have_the_value_deny() {
            var header = AntiClickjackingHeaderValue.Deny();

            header.HeaderValue.Should().Be("DENY");
        }

        [Fact]
        public void When_creating_a_sameOrigin_it_should_have_the_vale_sameOrigin() {
            var header = AntiClickjackingHeaderValue.SameOrigin();

            header.HeaderValue.Should().Be("SAMEORIGIN");
        }

        [Theory]
        [InlineData("http://www.example.org")]
        [InlineData("https://www.example.org")]
        public void When_creating_allowForm_it_should_have_the_correct_Value(string origin) {
            var header = AntiClickjackingHeaderValue.AllowFrom(origin);

            header.HeaderValue.Should().Be($"ALLOW-FROM {origin}");
        }
    }
}
