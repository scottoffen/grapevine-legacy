using System;
using System.Reflection;
using Grapevine.Util;
using Grapevine.Util.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util.Loggers
{
    public class NullLoggerTester
    {
        [Fact]
        public void null_logger_initialized_at_first_access()
        {
            var field = typeof(NullLogger).GetField("_logger", BindingFlags.Static | BindingFlags.NonPublic);
            field.SetValue(null, null);
            field.GetValue(null).ShouldBeNull();

            var logger1 = NullLogger.GetInstance();
            var logger2 = NullLogger.GetInstance();

            field.GetValue(null).ShouldNotBeNull();
        }

        [Fact]
        public void null_logger_previously_initialized()
        {
            var obj = new object();
            var ex = new Exception();
            var msg = "Log Message";

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
