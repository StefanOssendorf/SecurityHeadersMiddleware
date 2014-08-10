using System;
using System.Collections.Generic;
using System.Linq;
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
            config.ImgSrc.AddKeyword(SourceListKeyword.Self);

            var value = config.ToHeaderValue();
            var split = value.Split(new[] { ";" }, StringSplitOptions.None);

            split.Length.Should().Be(2);
            split.Should().Contain(item => item.Trim().Equals("script-src https:"));
            split.Should().Contain(item => item.Trim().Equals("img-src 'self'"));
        }

        [Fact]
        public void Source_types_should_be_separated_by_a_semicolon() {
            var config = new ContentSecurityPolicyConfiguration();
            config.StyleSrc.AddKeyword(SourceListKeyword.Self);
            config.ImgSrc.AddScheme("https");
            config.MediaSrc.AddKeyword(SourceListKeyword.UnsafeInline);
            config.BaseUri.AddScheme("https");

            var split = config.ToHeaderValue().Split(new[] { ";" }, StringSplitOptions.None);

            split.Length.Should().Be(4);
        }


        [Fact]
        public void All_source_types_should_be_in_the_header_value() {
            var config = new ContentSecurityPolicyConfiguration();
            config.BaseUri.AddKeyword(SourceListKeyword.Self);
            config.ChildSrc.AddKeyword(SourceListKeyword.Self);
            config.ConnectSrc.AddKeyword(SourceListKeyword.Self);
            config.DefaultSrc.AddKeyword(SourceListKeyword.Self);
            config.FontSrc.AddKeyword(SourceListKeyword.Self);
            config.FormAction.AddKeyword(SourceListKeyword.Self);
            config.FrameAncestors.AddKeyword(SourceListKeyword.Self);
            config.FrameSrc.AddKeyword(SourceListKeyword.Self);
            config.ImgSrc.AddKeyword(SourceListKeyword.Self);
            config.MediaSrc.AddKeyword(SourceListKeyword.Self);
            config.ObjectSrc.AddKeyword(SourceListKeyword.Self);
            config.ScriptSrc.AddKeyword(SourceListKeyword.Self);
            config.StyleSrc.AddKeyword(SourceListKeyword.Self);
            config.PluginTypes.AddMediaType("application/xml");
            config.Referrer = ReferrerKeyword.None;
            config.ReflectedXss = ReflectedXssKeyword.Allow;
            config.ReportUri.AddReportUri("https://www.example.com/report-uri");
            config.Sandbox.AddToken("allow-scripts");

            var expected = new List<string> {
                "base-uri", "child-src", "connect-src", "default-src","font-src", "form-action", "frame-ancestors", "frame-src",
                "img-src", "media-src", "object-src", "plugin-types", "referrer", "reflected-xss", "report-uri", "sandbox", 
                "script-src", "style-src"
            };

            var values = config.ToHeaderValue().Split(new[] { ";" }, StringSplitOptions.None).SelectMany(i => i.Split(new[] { " " }, StringSplitOptions.None)).ToList();
            values.Should().Contain(expected);
        }
    }
}