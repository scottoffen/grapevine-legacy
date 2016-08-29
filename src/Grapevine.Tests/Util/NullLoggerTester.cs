using System;
using Grapevine.Util;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util
{
    public class NullLoggerTester
    {
        [Fact]
        public void null_logger_does_nothing()
        {
            var obj = new object();
            var ex = new Exception();
            var msg = "Log Message";

            var logger = new NullLogger();

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
