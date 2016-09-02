using System;
using Grapevine.Util;
using Grapevine.Util.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util.Loggers
{
    public class InMemoryLoggerTester
    {
        [Fact]
        public void debug_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger(LogLevel.Info);

            logger.Logs.Count.ShouldBe(0);

            logger.Debug(msg);

            logger.Logs.Count.ShouldBe(0);
        }

        [Fact]
        public void debug_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var logger = new InMemoryLogger(LogLevel.Debug);

            logger.Logs.Count.ShouldBe(0);

            logger.Debug(msg, ex);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldNotBeNull();
            entry.Exception.Message.Equals(exmsg).ShouldBeTrue();
            entry.Level.ShouldBe(LogLevel.Debug);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void debug_logs_object_to_buffer()
        {
            ValueType obj = true;
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Debug(obj);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Debug);
            entry.Message.Equals("True").ShouldBeTrue();
        }

        [Fact]
        public void debug_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Debug(msg);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Debug);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void error_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger(LogLevel.Fatal);

            logger.Logs.Count.ShouldBe(0);

            logger.Error(msg);

            logger.Logs.Count.ShouldBe(0);
        }

        [Fact]
        public void error_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var logger = new InMemoryLogger(LogLevel.Error);

            logger.Logs.Count.ShouldBe(0);

            logger.Error(msg, ex);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldNotBeNull();
            entry.Exception.Message.Equals(exmsg).ShouldBeTrue();
            entry.Level.ShouldBe(LogLevel.Error);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void error_logs_object_to_buffer()
        {
            ValueType obj = true;
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Error(obj);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Error);
            entry.Message.Equals("True").ShouldBeTrue();
        }

        [Fact]
        public void error_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Error(msg);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Error);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void fatal_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var logger = new InMemoryLogger(LogLevel.Fatal);

            logger.Logs.Count.ShouldBe(0);

            logger.Fatal(msg, ex);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldNotBeNull();
            entry.Exception.Message.Equals(exmsg).ShouldBeTrue();
            entry.Level.ShouldBe(LogLevel.Fatal);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void fatal_logs_object_to_buffer()
        {
            ValueType obj = true;
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Fatal(obj);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Fatal);
            entry.Message.Equals("True").ShouldBeTrue();
        }

        [Fact]
        public void fatal_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Fatal(msg);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Fatal);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void info_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger(LogLevel.Warn);

            logger.Logs.Count.ShouldBe(0);

            logger.Info(msg);

            logger.Logs.Count.ShouldBe(0);
        }

        [Fact]
        public void info_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var logger = new InMemoryLogger(LogLevel.Info);

            logger.Logs.Count.ShouldBe(0);

            logger.Info(msg, ex);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldNotBeNull();
            entry.Exception.Message.Equals(exmsg).ShouldBeTrue();
            entry.Level.ShouldBe(LogLevel.Info);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void info_logs_object_to_buffer()
        {
            ValueType obj = true;
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Info(obj);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Info);
            entry.Message.Equals("True").ShouldBeTrue();
        }

        [Fact]
        public void info_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Info(msg);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Info);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void log_inserts_log_event_into_buffer()
        {
            var time = DateTime.Now;
            var msg = "This is the message";
            var evt = new LogEvent { Level = LogLevel.Fatal, DateTime = time, Message = msg };
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Log(evt);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.DateTime.ShouldBe(time);
            entry.Level.ShouldBe(LogLevel.Fatal);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void warn_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger(LogLevel.Error);

            logger.Logs.Count.ShouldBe(0);

            logger.Warn(msg);

            logger.Logs.Count.ShouldBe(0);
        }

        [Fact]
        public void warn_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var logger = new InMemoryLogger(LogLevel.Warn);

            logger.Logs.Count.ShouldBe(0);

            logger.Warn(msg, ex);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldNotBeNull();
            entry.Exception.Message.Equals(exmsg).ShouldBeTrue();
            entry.Level.ShouldBe(LogLevel.Warn);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void warn_logs_object_to_buffer()
        {
            ValueType obj = true;
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Warn(obj);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Warn);
            entry.Message.Equals("True").ShouldBeTrue();
        }

        [Fact]
        public void warn_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Warn(msg);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Warn);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void trace_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger(LogLevel.Debug);

            logger.Logs.Count.ShouldBe(0);

            logger.Trace(msg);

            logger.Logs.Count.ShouldBe(0);
        }

        [Fact]
        public void trace_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var logger = new InMemoryLogger(LogLevel.Trace);

            logger.Logs.Count.ShouldBe(0);

            logger.Trace(msg, ex);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldNotBeNull();
            entry.Exception.Message.Equals(exmsg).ShouldBeTrue();
            entry.Level.ShouldBe(LogLevel.Trace);
            entry.Message.Equals(msg).ShouldBeTrue();
        }

        [Fact]
        public void trace_logs_object_to_buffer()
        {
            ValueType obj = true;
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Trace(obj);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Trace);
            entry.Message.Equals("True").ShouldBeTrue();
        }

        [Fact]
        public void trace_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var logger = new InMemoryLogger();

            logger.Logs.Count.ShouldBe(0);

            logger.Trace(msg);

            var entry = logger.Logs[0];
            entry.ShouldNotBeNull();
            entry.Exception.ShouldBeNull();
            entry.Level.ShouldBe(LogLevel.Trace);
            entry.Message.Equals(msg).ShouldBeTrue();
        }
    }
}
