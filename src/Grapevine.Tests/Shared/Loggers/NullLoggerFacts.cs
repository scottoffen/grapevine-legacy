using System;
using System.Reflection;
using Grapevine.Interfaces.Shared;
using Grapevine.Shared.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Shared.Loggers
{
    public class NullLoggerFacts
    {
        [Fact]
        public void CreateInitializesLoggerOnFirstAccess()
        {
            var field = typeof(NullLogger).GetField("_logger", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, null);
            field.GetValue(null).ShouldBeNull();

            var logger1 = NullLogger.GetInstance();
            field.GetValue(null).ShouldNotBeNull();

            var logger2 = NullLogger.GetInstance();
            logger2.Equals(logger1).ShouldBeTrue();
        }

        [Fact]
        public void CreateReturnsSameInstance()
        {
            var logger1 = NullLogger.GetInstance();
            var logger2 = NullLogger.GetInstance();
            logger2.Equals(logger1).ShouldBeTrue();
        }

        [Fact]
        public void AllMethodsDoNothing()
        {
            const string msg = "Log Message";
            var obj = new object();
            var ex = new Exception();

            var logger = NullLogger.GetInstance();

            logger.Log(new LogEvent());

            logger.Debug(obj);
            logger.Debug(msg);
            logger.Debug(msg, ex);

            logger.Error(obj);
            logger.Error(msg);
            logger.Error(msg, ex);

            logger.Fatal(obj);
            logger.Fatal(msg);
            logger.Fatal(msg, ex);

            logger.Info(obj);
            logger.Info(msg);
            logger.Info(msg, ex);

            logger.Warn(obj);
            logger.Warn(msg);
            logger.Warn(msg, ex);

            logger.Trace(obj);
            logger.Trace(msg);
            logger.Trace(msg, ex);

            logger.Level.ShouldBe(LogLevel.Trace);
        }
    }
}
