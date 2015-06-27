using System;
using System.IO;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    
    public class HostSourceTests {
        [Fact]
        public void When_creating_a_valid_full_host_source_it_should_not_throw_a_formatException() {
            new HostSource("http://*.example.com:80/path/");
        }

        [Fact]
        public void When_creating_a_valid_host_source_without_schemePart_it_should_not_throw_a_formatException() {
            new HostSource("*.example.com:80/path/");
        }

        [Fact]
        public void When_creating_a_valid_host_source_with_only_path_part_it_should_not_throw_a_formatException() {
            new HostSource("*.example.com/path/");
        }

        [Fact]
        public void When_creating_a_valid_host_source_with_only_host_part_it_should_not_throw_a_formatException() {
            new HostSource("*.example.com");
        }

        [Fact]
        public void When_creating_an_invalid_host_part_it_should_throw_a_formatException() {
            AssertFormatException("ftp://*.example./abcd/");
        }

        [Fact]
        public void When_creating_an_invalid_port_part_it_should_throw_a_formatException() {
            AssertFormatException("*.example.com:1as");
        }

        [Fact]
        public void When_creating_an_invalid_path_it_should_throw_a_formatException() {
            AssertFormatException("*.example.com/%1");
        }

        [Fact]
        public void When_creating_an_empty_host_it_should_throw_a_argumentException() {
            Assert.Throws<ArgumentException>(() => new HostSource(""));
        }

        [Fact]
        public void When_creating_a_valid_host_source_with_queryString() {
            new HostSource("http://www.exmpale.com/abcd/?a=1");
        }

        [Fact]
        public void When_creating_a_valid_host_source_with_fragment() {
            new HostSource("http://www.example.com/abcd/#clearly");
        }

        [Fact]
        public void When_creating_a_host_with_invalid_scheme_it_should_throw_a_formatException() {
            AssertFormatException(".http://www.example.com");
        }

        [Fact]
        public void When_creating_a_host_without_hostPart_it_should_throw_a_formatException() {
            AssertFormatException("http://:80/path/");
        }

        [Fact]
        public void When_creating_a_host_with_only_star_char_should_be_valid() {
            new HostSource("*");
        }

        private static void AssertFormatException(string value) {
            Assert.Throws<FormatException>(() => new HostSource(value));
        }
    }
}