using System;
using FluentAssertions;
using Machine.Specifications;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CspSourceListTests {
        [Fact]
        public void When_adding_an_invalid_scheme_it_should_throw_a_formatException() {
            var list = new CspSourceList();
            Assert.Throws<FormatException>(() => list.AddScheme("+invalid"));
        }

        [Fact]
        public void When_adding_an_scheme_it_should_create_the_correct_value_for_the_header() {
            var list = new CspSourceList();
            list.AddScheme("http");

            list.ToDirectiveValue().Trim().ShouldEqual("http:");
        }

        [Fact]
        public void When_adding_multiple_schemes_it_should_create_the_schemes_whiteSpace_separated() {
            var list = new CspSourceList();
            list.AddScheme("http");
            list.AddScheme("ftp");
            list.AddScheme("https");

            list.ToDirectiveValue().ShouldEqual("http: ftp: https:");
        }

        [Fact]
        public void When_set_list_to_none_adding_a_scheme_should_throw_an_invalidOperationException() {
            var list = new CspSourceList();
            list.SetToNone();

            Assert.Throws<InvalidOperationException>(() => list.AddScheme("http"));
        }

        [Fact]
        public void When_set_list_to_none_adding_a_host_should_throw_an_invalidOperationException() {
            var list = new CspSourceList();
            list.SetToNone();

            Assert.Throws<InvalidOperationException>(() => list.AddHost("http://www.example.com"));
        }

        [Fact]
        public void When_set_list_to_none_adding_a_keyWord_should_throw_an_invalidOperationException() {
            var list = new CspSourceList();
            list.SetToNone();

            Assert.Throws<InvalidOperationException>(() => list.AddKeyword(SourceListKeyword.Self));
        }

        [Fact]
        public void When_set_list_to_none_it_should_create_header_value_with_none() {
            var list = new CspSourceList();
            list.SetToNone();

            list.ToDirectiveValue().ShouldEqual("'none'");
        }

        [Fact]
        public void When_adding_a_keyword_it_should_create_the_correct_value_for_the_header() {
            var list = new CspSourceList();
            list.AddKeyword(SourceListKeyword.Self);

            list.ToDirectiveValue().Trim().ShouldEqual("'self'");
        }

        [Fact]
        public void When_adding_a_keyword_multiple_times_it_should_return_it_only_once_in_header_value() {
            var list = new CspSourceList();
            list.AddKeyword(SourceListKeyword.Self);
            list.AddKeyword(SourceListKeyword.Self);
            list.AddKeyword(SourceListKeyword.Self);
            list.AddKeyword(SourceListKeyword.Self);

            list.ToDirectiveValue().Trim().ShouldEqual("'self'");
        }

        [Fact]
        public void When_addin_mutliple_keywords_it_should_create_the_keywords_whiteSpace_separated() {
            var list = new CspSourceList();
            list.AddKeyword(SourceListKeyword.Self);
            list.AddKeyword(SourceListKeyword.UnsafeEval);
            list.AddKeyword(SourceListKeyword.Self);
            list.AddKeyword(SourceListKeyword.UnsafeInline);
            list.AddKeyword(SourceListKeyword.UnsafeRedirect);

            list.ToDirectiveValue().Trim().ShouldEqual("'self' 'unsafe-eval' 'unsafe-inline' 'unsafe-redirect'");
        }

        [Fact]
        public void When_adding_a_valid_full_host_source_it_should_not_throw_a_formatException() {
            var list = new CspSourceList();

            Assert.DoesNotThrow(() => list.AddHost("http://*.example.com:80/path/"));
        }

        [Fact]
        public void When_adding_a_valid_host_source_without_schemePart_it_should_not_throw_a_formatException() {
            var list = new CspSourceList();

            Assert.DoesNotThrow(() => list.AddHost("*.example.com:80/path/"));
        }

        [Fact]
        public void When_adding_a_valid_host_source_with_only_path_part_it_should_not_throw_a_formatException() {
            var list = new CspSourceList();

            Assert.DoesNotThrow(() => list.AddHost("*.example.com/path/"));
        }

        [Fact]
        public void When_adding_a_valid_host_source_with_only_host_part_it_should_not_throw_a_formatException() {
            var list = new CspSourceList();

            Assert.DoesNotThrow(() => list.AddHost("*.example.com"));
        }

        [Fact]
        public void When_adding_one_scheme_multiple_times_it_should_only_be_once_in_the_header_value() {
            var list = new CspSourceList();
            list.AddScheme("http:");
            list.AddScheme("http");

            list.ToDirectiveValue().Trim().ShouldEqual("http:");
        }

        [Fact]
        public void When_adding_an_invalid_host_part_it_should_throw_a_formatException() {
            var list = new CspSourceList();

            Assert.Throws<FormatException>(() => list.AddHost("ftp://*.example./abcd/"));
        }

        [Fact]
        public void When_adding_an_invalid_port_part_it_should_throw_a_formatException() {
            var list = new CspSourceList();

            Assert.Throws<FormatException>(() => list.AddHost("*.example.com:1as"));
        }

        [Fact]
        public void When_adding_an_invalid_path_it_should_throw_a_formatException() {
            var list = new CspSourceList();

            Assert.Throws<FormatException>(() => list.AddHost("*.example.com/%1"));
        }

        [Fact]
        public void When_adding_one_host_it_should_create_the_correct_header_value() {
            var list = new CspSourceList();
            list.AddHost("http://*.example.com:*/path/file.js");

            list.ToDirectiveValue().Trim().ShouldEqual("http://*.example.com:*/path/file.js");
        }
        [Fact]
        public void When_adding_an_empty_host_it_should_throw_a_argumentException() {
            var list = new CspSourceList();

            Assert.Throws<ArgumentException>(() => list.AddHost(""));
        }

        [Fact]
        public void When_adding_scheme_host_and_keyword_it_should_create_correct_header_value() {
            var list = new CspSourceList();
            list.AddScheme("https");
            list.AddKeyword(SourceListKeyword.Self);
            list.AddHost("https://www.example.com/");

            list.ToDirectiveValue().Trim().ShouldEqual("https:  https://www.example.com/  'self'");
        }

        [Fact]
        public void When_adding_a_valid_uri_as_host_it_should_not_throw_a_exception() {
            var list = new CspSourceList();

            Assert.DoesNotThrow(() => list.AddHost(new Uri("https://www.example.com/abcd/")));
        }

        [Fact]
        public void When_adding_keyword_unsafeinline_it_should_create_the_correct_header_value() {
            var list = new CspSourceList();
            list.AddKeyword(SourceListKeyword.UnsafeInline);

            list.ToDirectiveValue().Trim().ShouldEqual("'unsafe-inline'");
        }

        [Fact]
        public void When_using_a_semicolon_in_a_value_it_should_be_escaped() {
            var list = new CspSourceList();
            list.AddHost("http://example.com/abcd;/asdas");

            list.ToDirectiveValue().Should().Be("http://example.com/abcd%3B/asdas");
        }

        [Fact]
        public void When_using_a_comma_in_a_value_it_should_be_escaped() {
            var list = new CspSourceList();
            list.AddHost("http://example.com/abcd,/asdas");

            list.ToDirectiveValue().Should().Be("http://example.com/abcd%2C/asdas");
        }
    }
}