using System;
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

            list.ToHeaderValue().Trim().ShouldEqual("http:");
        }

        [Fact]
        public void When_adding_multiple_schemes_it_should_create_the_schemes_whiteSpace_separated() {
            var list = new CspSourceList();
            list.AddScheme("http");
            list.AddScheme("ftp");
            list.AddScheme("https");

            list.ToHeaderValue().ShouldEqual("http: ftp: https:");
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

            Assert.Throws<InvalidOperationException>(() => list.AddKeyword(CspKeyword.Self));
        }

        [Fact]
        public void When_set_list_to_none_it_should_create_header_value_with_none() {
            var list = new CspSourceList();
            list.SetToNone();

            list.ToHeaderValue().ShouldEqual("'none'");
        }

        [Fact]
        public void When_adding_a_keyword_it_should_create_the_correct_value_for_the_header() {
            var list = new CspSourceList();
            list.AddKeyword(CspKeyword.Self);

            list.ToHeaderValue().Trim().ShouldEqual("'self'");
        }

        [Fact]
        public void When_adding_a_keyword_multiple_times_it_should_return_it_only_once_in_header_value() {
            var list = new CspSourceList();
            list.AddKeyword(CspKeyword.Self);
            list.AddKeyword(CspKeyword.Self);
            list.AddKeyword(CspKeyword.Self);
            list.AddKeyword(CspKeyword.Self);

            list.ToHeaderValue().Trim().ShouldEqual("'self'");
        }

        [Fact]
        public void When_addin_mutliple_keywords_it_should_create_the_keywords_whiteSpace_separated() {
            var list = new CspSourceList();
            list.AddKeyword(CspKeyword.Self);
            list.AddKeyword(CspKeyword.UnsafeEval);
            list.AddKeyword(CspKeyword.Self);
            list.AddKeyword(CspKeyword.UnsafeInline);
            list.AddKeyword(CspKeyword.UnsafeRedirect);

            list.ToHeaderValue().Trim().ShouldEqual("'self' 'unsafeeval' 'unsafeinline' 'unsaferedirect'");
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

            list.ToHeaderValue().Trim().ShouldEqual("http:");
        }

    }
}