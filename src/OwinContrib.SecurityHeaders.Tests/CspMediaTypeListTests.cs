using System;
using Xunit;

namespace SecurityHeadersMiddleware.Tests {
    public class CspMediaTypeListTests {
        [Fact]
        public void When_trying_to_add_a_mediaType_without_slash_it_should_throw_a_formatException() {
            var csp = new CspMediaTypeList();
            Assert.Throws<FormatException>(() => csp.AddMediaType("abcd"));
        }

        [Fact]
        public void When_trying_to_add_a_mediaType_with_a_ctl_in_type_it_should_throw_a_formatException() {
            var csp = new CspMediaTypeList();
            var mediaType = (char) 10 + "acd/x-abcd";
            Assert.Throws<FormatException>(() => csp.AddMediaType(mediaType));
        }

        [Fact]
        public void When_trying_to_add_a_mediaType_with_a_space_it_should_throw_a_formatException() {
            var csp = new CspMediaTypeList();
            Assert.Throws<FormatException>(() => csp.AddMediaType("ac d/x-abcd"));
        }

        [Fact]
        public void When_trying_to_add_an_invalid_subType_it_should_throw_a_formatException() {
            var csp = new CspMediaTypeList();
            Assert.Throws<FormatException>(() => csp.AddMediaType("xml/x;sd"));
        }

        // Source of mediatypes: https://www.iana.org/assignments/media-types/media-types.xhtml
        [Fact]
        public void When_adding_some_IANA_specified_mediaTypes_it_should_not_throw() {
            var csp = new CspMediaTypeList();
            csp.AddMediaType("text/1d-interleaved-parityfec");
            csp.AddMediaType("text/provenance-notation");
            csp.AddMediaType("text/vnd.net2phone.commcenter.command");
            csp.AddMediaType("video/H264-RCDO");
            csp.AddMediaType("video/vnd.dece.mobile");
            csp.AddMediaType("video/vnd.iptvforum.1dparityfec-2005");
            csp.AddMediaType("multipart/form-data");
            csp.AddMediaType("multipart/form-data");
            csp.AddMediaType("multipart/report");
            csp.AddMediaType("model/vnd.valve.source.compiled-map");
            csp.AddMediaType("model/vnd.moml+xml");
            csp.AddMediaType("model/example");
            csp.AddMediaType("message/global-disposition-notification");
            csp.AddMediaType("message/disposition-notification");
            csp.AddMediaType("message/s-http");
            csp.AddMediaType("image/vnd.sealedmedia.softseal-jpg");
            csp.AddMediaType("image/vnd.airzip.accelerator.azv");
            csp.AddMediaType("image/jpeg");
            csp.AddMediaType("audio/vnd.sealedmedia.softseal-mpeg");
            csp.AddMediaType("audio/vnd.dolby.heaac.2");
            csp.AddMediaType("audio/dsr-es202050");
            csp.AddMediaType("application/xml-external-parsed-entity");
            csp.AddMediaType("application/vnd.yamaha.openscoreformat.osfpvg+xml");
            csp.AddMediaType("application/xml");
        }

        [Fact]
        public void When_adding_valid_mediaTypes_it_should_not_throw() {
            var csp = new CspMediaTypeList();
            csp.AddMediaType("text/plain");
            csp.AddMediaType("text/x-myownsubtype");
        }
    }
}