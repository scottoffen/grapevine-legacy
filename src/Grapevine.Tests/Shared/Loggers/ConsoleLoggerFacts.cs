using System;
using System.IO;
using Grapevine.Interfaces.Shared;
using Grapevine.Shared.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared.Loggers
{
    public class ConsoleLoggerFacts
    {
        public static string MessageText = "This is the message";
        public static string ExceptionText = "This is the exception";
        public static Exception FakeException = new Exception(ExceptionText);
        public static ValueType ObjectToLog = true;

        [Theory,
            InlineData("Trace", LogLevel.Debug),
            InlineData("Debug", LogLevel.Info),
            InlineData("Info", LogLevel.Warn),
            InlineData("Warn", LogLevel.Error),
            InlineData("Error", LogLevel.Fatal)]
        public void DoesNotLogAtLowerLogLevel(string levelText, LogLevel level)
        {
            var stdout = Console.Out;

            var method = typeof(ConsoleLogger).GetMethod(levelText, new [] {typeof(string)});

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger(level);
                method.Invoke(logger, new object[] {MessageText});

                var result = sw.ToString();
                result.ShouldBeNullOrWhiteSpace();
            }

            Console.SetOut(stdout);
        }

        [Theory,
            InlineData("Trace"),
            InlineData("Debug"),
            InlineData("Info"),
            InlineData("Warn"),
            InlineData("Error"),
            InlineData("Fatal")]
        public void LogsExceptionToConsole(string levelText)
        {
            var stdout = Console.Out;
            var method = typeof(ConsoleLogger).GetMethod(levelText, new[] { typeof(string), typeof(Exception) });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();
                method.Invoke(logger, new object[] { MessageText, FakeException });

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe(levelText);
                result[2].TrimEnd('\r', '\n').ShouldBe($"{MessageText}:{ExceptionText}");
            }

            Console.SetOut(stdout);
        }

        [Theory,
            InlineData("Trace"),
            InlineData("Debug"),
            InlineData("Info"),
            InlineData("Warn"),
            InlineData("Error"),
            InlineData("Fatal")]
        public void LogsObjectToConsole(string levelText)
        {
            var stdout = Console.Out;
            var method = typeof(ConsoleLogger).GetMethod(levelText, new[] { typeof(object) });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                    
                var logger = new ConsoleLogger();
                method.Invoke(logger, new object[] { ObjectToLog });

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe(levelText);
                result[2].TrimEnd('\r', '\n').Equals(ObjectToLog.ToString()).ShouldBeTrue();
            }

            Console.SetOut(stdout);
        }

        [Theory,
            InlineData("Trace"),
            InlineData("Debug"),
            InlineData("Info"),
            InlineData("Warn"),
            InlineData("Error"),
            InlineData("Fatal")]
        public void LogsStringToConsole(string levelText)
        {
            var stdout = Console.Out;
            var method = typeof(ConsoleLogger).GetMethod(levelText, new[] { typeof(string) });

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();
                method.Invoke(logger, new object[] { MessageText });

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe(levelText);
                result[2].TrimEnd('\r', '\n').ShouldBe(MessageText);
            }

            Console.SetOut(stdout);
        }
    }
}
