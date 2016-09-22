using System;
using Grapevine.Interfaces.Shared;
using Grapevine.Shared.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared.Loggers
{
    public class InMemoryLoggerFacts
    {
        public static string MessageText = "This is the message";
        public static string ExceptionText = "This is the exception";
        public static Exception FakeException = new Exception(ExceptionText);
        public static ValueType ObjectToLog = true;

        [Fact]
        public void LoggedMessagesAreStoredAndAccessible()
        {
            const string debug = "Debug Message";
            const string trace = "Trace Message";
            const string fatal = "Fatal Message";

            var logger = new InMemoryLogger();
            logger.Logs.Count.ShouldBe(0);

            logger.Debug(debug);
            logger.Trace(trace);
            logger.Fatal(fatal);

            logger.Logs.Count.ShouldBe(3);

            var logs = logger.LogMessages;

            logs[0].ShouldBe(debug);
            logs[1].ShouldBe(trace);
            logs[2].ShouldBe(fatal);
        }

        [Theory,
            InlineData("Trace", LogLevel.Debug),
            InlineData("Debug", LogLevel.Info),
            InlineData("Info", LogLevel.Warn),
            InlineData("Warn", LogLevel.Error),
            InlineData("Error", LogLevel.Fatal)]
        public void DoesNotLogAtLowerLogLevel(string levelText, LogLevel level)
        {
            var method = typeof(InMemoryLogger).GetMethod(levelText, new[] { typeof(string) });
            var logger = new InMemoryLogger(level);

            logger.Logs.Count.ShouldBe(0);

            method.Invoke(logger, new object[] { MessageText });

            logger.Logs.Count.ShouldBe(0);
        }

        [Theory,
            InlineData(LogLevel.Trace),
            InlineData(LogLevel.Debug),
            InlineData(LogLevel.Info),
            InlineData(LogLevel.Warn),
            InlineData(LogLevel.Error),
            InlineData(LogLevel.Fatal)]
        public void LogsExceptionToBuffer(LogLevel logLevel)
        {
            var method = typeof(InMemoryLogger).GetMethod(logLevel.ToString(), new[] { typeof(string), typeof(Exception) });
            var logger = new InMemoryLogger();

            method.Invoke(logger, new object[] { MessageText, FakeException });

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldNotBeNull();
            entry.Exception.Message.Equals(ExceptionText).ShouldBeTrue();
            entry.Level.ShouldBe(logLevel);
            entry.Message.Equals(MessageText).ShouldBeTrue();
        }

        [Theory,
            InlineData(LogLevel.Trace),
            InlineData(LogLevel.Debug),
            InlineData(LogLevel.Info),
            InlineData(LogLevel.Warn),
            InlineData(LogLevel.Error),
            InlineData(LogLevel.Fatal)]
        public void LogsObjectToBuffer(LogLevel logLevel)
        {
            var method = typeof(InMemoryLogger).GetMethod(logLevel.ToString(), new[] { typeof(object) });
            var logger = new InMemoryLogger();

            method.Invoke(logger, new object[] { ObjectToLog });

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(logLevel);
            entry.Message.Equals(ObjectToLog.ToString()).ShouldBeTrue();
        }

        [Theory,
            InlineData(LogLevel.Trace),
            InlineData(LogLevel.Debug),
            InlineData(LogLevel.Info),
            InlineData(LogLevel.Warn),
            InlineData(LogLevel.Error),
            InlineData(LogLevel.Fatal)]
        public void LogsStringToBuffer(LogLevel logLevel)
        {
            var method = typeof(InMemoryLogger).GetMethod(logLevel.ToString(), new[] { typeof(object) });
            var logger = new InMemoryLogger();

            method.Invoke(logger, new object[] { MessageText });

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(logLevel);
            entry.Message.Equals(MessageText).ShouldBeTrue();
        }
    }
}
