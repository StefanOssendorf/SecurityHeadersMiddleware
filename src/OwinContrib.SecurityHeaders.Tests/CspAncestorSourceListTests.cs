using System;
using FluentAssertions;
using Machine.Specifications;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CspAncestorSourceListTests {
        private CspAncestorSourceList CreateSUT() {
            return new CspAncestorSourceList();
        }

        [Fact]
        public void When_adding_a_valid_host_source_it_should_succeed() {
            var list = CreateSUT();
            list.AddHost("http://*.example.com:80/path/");
        }

        [Fact]
        public void When_adding_an_invalid_host_source_it_should_fail() {
            var list = CreateSUT();
            Assert.Throws<FormatException>(() => list.AddHost("holy crap this will fail!"));
        }

        [Fact]
        public void When_adding_an_invalid_scheme_it_should_throw_a_formatException() {
            var list = CreateSUT();
            Assert.Throws<FormatException>(() => list.AddScheme("+invalid"));
        }

        [Fact]
        public void When_adding_an_scheme_it_should_create_the_correct_value_for_the_header() {
            var list = CreateSUT();
            list.AddScheme("http");
            list.ToDirectiveValue().Trim().ShouldEqual("http:");
        }

        [Fact]
        public void When_adding_multiple_schemes_it_should_create_the_schemes_whiteSpace_separated() {
            var list = CreateSUT();
            list.AddScheme("http");
            list.AddScheme("ftp");
            list.AddScheme("https");
            list.ToDirectiveValue().ShouldEqual("http: ftp: https:");
        }

        [Fact]
        public void When_set_list_to_none_adding_a_scheme_should_throw_an_invalidOperationException() {
            var list = CreateSUT();
            list.SetToNone();
            Assert.Throws<InvalidOperationException>(() => list.AddScheme("http"));
        }

        [Fact]
        public void When_set_list_to_none_adding_a_host_should_throw_an_invalidOperationException() {
            var list = CreateSUT();
            list.SetToNone();
            Assert.Throws<InvalidOperationException>(() => list.AddHost("http://www.example.com"));
        }

        [Fact]
        public void When_set_list_to_none_it_should_create_header_value_with_none() {
            var list = CreateSUT();
            list.SetToNone();
            list.ToDirectiveValue().ShouldEqual("'none'");
        }

        [Fact]
        public void When_adding_one_scheme_multiple_times_it_should_only_be_once_in_the_header_value() {
            var list = CreateSUT();
            list.AddScheme("http:");
            list.AddScheme("http");
            list.ToDirectiveValue().Trim().ShouldEqual("http:");
        }

        [Fact]
        public void When_adding_one_host_it_should_create_the_correct_header_value() {
            var list = CreateSUT();
            list.AddHost("http://*.example.com:*/path/file.js");
            list.ToDirectiveValue().Trim().ShouldEqual("http://*.example.com:*/path/file.js");
        }

        [Fact]
        public void When_adding_one_host_multiple_times_it_should_only_be_once_in_the_header_value() {
            var sut = CreateSUT();
            sut.AddHost("http://www.example.org");
            sut.AddHost("http://www.example.org"); 
            sut.ToDirectiveValue().Trim().Should().Be("http://www.example.org");
        }

        [Fact]
        public void When_adding_an_http_host_with_and_without_default_values_they_should_be_treated_as_equal() {
            var sut = CreateSUT();
            sut.AddHost("http://www.example.org");
            sut.AddHost("http://www.example.org:80");
            sut.ToDirectiveValue().Trim().Should().Be("http://www.example.org");
        }

        [Fact]
        public void When_adding_a_valid_uri_as_host_it_should_not_throw_a_exception() {
            var list = CreateSUT();
            list.AddHost(new Uri("https://www.example.com/abcd/"));
        }

        [Fact]
        public void When_using_a_semicolon_in_a_value_it_should_be_escaped() {
            var list = CreateSUT();
            list.AddHost("http://example.com/abcd;/asdas");
            list.ToDirectiveValue().Should().Be("http://example.com/abcd%3B/asdas");
        }

        [Fact]
        public void When_using_a_comma_in_a_value_it_should_be_escaped() {
            var list = CreateSUT();
            list.AddHost("http://example.com/abcd,/asdas");
            list.ToDirectiveValue().Should().Be("http://example.com/abcd%2C/asdas");
        }

    }
}