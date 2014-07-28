using System;
using FluentAssertions;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CspConfigurationTests {
        [Fact]
        public void When_generating_header_value_and_no_configurations_are_set_return_empty_string() {
            var config = new ContentSecurityPolicyConfiguration();

            config.ToHeaderValue().Should().BeEmpty();
        }

        [Fact]
        public void When_set_scriptSrc_to_none_the_header_value_should_contain_the_directive_with_none() {
            var config = new ContentSecurityPolicyConfiguration();
            config.ScriptSrc.SetToNone();

            config.ToHeaderValue().Should().Be("script-src 'none'");
        }

        [Fact]
        public void When_set_two_sources_they_should_be_separated_by_a_semicolon() {
            var config = new ContentSecurityPolicyConfiguration();
            config.ScriptSrc.AddScheme("https");
            config.ImgSrc.AddKeyword(CspKeyword.Self);

            var value = config.ToHeaderValue();
            var split = value.Split(new[] {";"}, StringSplitOptions.None);

            split.Length.Should().Be(2);
            split.Should().Contain(item => item.Trim().Equals("script-src https:"));
            split.Should().Contain(item => item.Trim().Equals("img-src 'self'"));
        }

        [Fact]
        public void Each_source_type_should_be_separated_by_a_semicolon() {
            var config = new ContentSecurityPolicyConfiguration();
            config.StyleSrc.AddKeyword(CspKeyword.Self);
            config.ImgSrc.AddScheme("https");
            config.MediaSrc.AddKeyword(CspKeyword.UnsafeInline);
            config.BaseUri.AddScheme("https");

            var split = config.ToHeaderValue().Split(new[] {";"}, StringSplitOptions.None);
            
            split.Length.Should().Be(4);
        }

        
    }
}