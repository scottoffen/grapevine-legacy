using System;
using Grapevine.Shared;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared
{
    public class PatternParserFacts
    {
        public class GenerateRegExMethod
        {
            [Fact]
            public void ReturnsRegularExpressionFromPathInfoPattern()
            {
                PatternParser.GenerateRegEx("/path/[param1]/[param2]").ToString().ShouldBe(@"^/path/([^/]+)/([^/]+)$");
            }

            [Fact]
            public void ReturnsSameExpressionWhenPathInfoPatternIsRegularExpression()
            {
                const string pattern = @"^\/path\/(\d+)\/(.+)$";
                PatternParser.GenerateRegEx(pattern).ToString().ShouldBe(pattern);
            }

            [Fact]
            public void IgnoresSquareBracketsInRegularExpressions()
            {
                const string pattern = @"^\/path\/([0123456789]+)\/(.+)$";
                PatternParser.GenerateRegEx(pattern).ToString().ShouldBe(pattern);
            }

            [Fact]
            public void ReturnsDefaultExpressionWhenParameterIsNull()
            {
                PatternParser.GenerateRegEx(null).ToString().ShouldBe(@"^.*$");
            }

            [Fact]
            public void ReturnsDefaultExpressionWhenParameterIsEmptyString()
            {
                PatternParser.GenerateRegEx(string.Empty).ToString().ShouldBe(@"^.*$");
            }

            [Fact]
            public void ReturnedExressionEndsInSingleDollar()
            {
                PatternParser.GenerateRegEx(@"/path/info$").ToString().EndsWith("$").ShouldBeTrue();
                PatternParser.GenerateRegEx(@"/path/info").ToString().EndsWith("$").ShouldBeTrue();
            }
        }

        public class GeneratePatternKeysMethod
        {
            [Fact]
            public void ReturnsListOfKeysFoundInPathInfoPattern()
            {
                const string param1 = "api";
                const string param2 = "user";
                var pattern = $"/[{param1}]/[{param2}]";

                var parsed = PatternParser.GeneratePatternKeys(pattern);

                parsed.Count.ShouldBe(2);
                parsed[0].ShouldBe(param1);
                parsed[1].ShouldBe(param2);
            }

            [Fact]
            public void DoesNotParsePatternParamsWithRegularExpressions()
            {
                PatternParser.GeneratePatternKeys(@"^\/path\/(\d+)\/(.+)$").Count.ShouldBe(0);
            }

            [Fact]
            public void ReturnsEmptyDictionaryWhenPathInfoIsNull()
            {
                PatternParser.GeneratePatternKeys(null).ShouldBeEmpty();
            }

            [Fact]
            public void ThrowsExceptionWhenPathInfoHasDuplicateKeys()
            {
                Should.Throw<ArgumentException>(() => PatternParser.GeneratePatternKeys("/[part]/[part]"));
            }
        }
    }
}
