using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Test.Util
{
    public class PatternParserTester
    {
        [Fact]
        public void parses_pattern_to_regular_expression ()
        {
            var pattern = "/path/[param1]/[param2]";
            PatternParser.GenerateRegEx(pattern).ToString().ShouldBe(@"^/path/(.+)/(.+)$");
        }

        [Fact]
        public void parses_pattern_params_to_list()
        {
            var pattern = "/path/[param1]/[param2]";
            var parsed = PatternParser.GeneratePatternKeys(pattern);

            parsed.Count.ShouldBe(2);
            parsed[0].ShouldBe("param1");
            parsed[1].ShouldBe("param2");
        }

        [Fact]
        public void recognizes_pattern_as_regex()
        {
            var pattern = @"^\/path\/(\d+)\/(.+)$";
            PatternParser.GenerateRegEx(pattern).ToString().ShouldBe(pattern);
        }

        [Fact]
        public void does_not_parse_pattern_params_with_regex()
        {
            var pattern = @"^\/path\/(\d+)\/(.+)$";
            PatternParser.GeneratePatternKeys(pattern).Count.ShouldBe(0);
        }

        [Fact]
        public void handles_regex_with_square_brackets()
        {
            var pattern = @"^\/path\/([0123456789]+)\/(.+)$";
            PatternParser.GenerateRegEx(pattern).ToString().ShouldBe(pattern);
        }

        [Fact]
        public void parses_null_to_default_expression()
        {
            PatternParser.GenerateRegEx(null).ToString().ShouldBe(@"^.*$");
        }

        [Fact]
        public void parses_empty_string_to_default_expression()
        {
            PatternParser.GenerateRegEx("").ToString().ShouldBe(@"^.*$");
        }
    }
}
