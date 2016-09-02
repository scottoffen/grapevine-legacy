using System;
using System.IO;
using Grapevine.Util;
using Grapevine.Util.Loggers;
using Shouldly;
using Xunit;

namespace Grapevine.Tests.Util.Loggers
{
    public class ConsoleLoggerTester
    {
        [Fact]
        public void debug_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger(LogLevel.Info);
                logger.Debug(msg);

                var result = sw.ToString();
                result.ShouldBeNullOrWhiteSpace();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void debug_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                var logger = new ConsoleLogger(LogLevel.Debug);

                logger.Debug(msg, ex);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Debug");
                result[2].TrimEnd('\r', '\n').ShouldBe($"{msg}:{exmsg}");
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void debug_logs_object_to_buffer()
        {
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                ValueType obj = true;
                var logger = new ConsoleLogger();

                logger.Debug(obj);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Debug");
                result[2].TrimEnd('\r', '\n').Equals("True").ShouldBeTrue();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void debug_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();

                logger.Debug(msg);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Debug");
                result[2].TrimEnd('\r', '\n').ShouldBe(msg);
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void error_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger(LogLevel.Fatal);
                logger.Error(msg);

                var result = sw.ToString();
                result.ShouldBeNullOrWhiteSpace();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void error_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                var logger = new ConsoleLogger(LogLevel.Error);

                logger.Error(msg, ex);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Error");
                result[2].TrimEnd('\r', '\n').ShouldBe($"{msg}:{exmsg}");
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void error_logs_object_to_buffer()
        {
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                ValueType obj = true;
                var logger = new ConsoleLogger();

                logger.Error(obj);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Error");
                result[2].TrimEnd('\r', '\n').Equals("True").ShouldBeTrue();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void error_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();

                logger.Error(msg);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Error");
                result[2].TrimEnd('\r', '\n').ShouldBe(msg);
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void fatal_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                var logger = new ConsoleLogger(LogLevel.Fatal);

                logger.Fatal(msg, ex);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Fatal");
                result[2].TrimEnd('\r', '\n').ShouldBe($"{msg}:{exmsg}");
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void fatal_logs_object_to_buffer()
        {
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                ValueType obj = true;
                var logger = new ConsoleLogger();

                logger.Fatal(obj);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Fatal");
                result[2].TrimEnd('\r', '\n').Equals("True").ShouldBeTrue();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void fatal_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();

                logger.Fatal(msg);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Fatal");
                result[2].TrimEnd('\r', '\n').ShouldBe(msg);
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void info_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger(LogLevel.Warn);
                logger.Info(msg);

                var result = sw.ToString();
                result.ShouldBeNullOrWhiteSpace();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void info_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                var logger = new ConsoleLogger(LogLevel.Info);

                logger.Info(msg, ex);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Info");
                result[2].TrimEnd('\r', '\n').ShouldBe($"{msg}:{exmsg}");
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void info_logs_object_to_buffer()
        {
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                ValueType obj = true;
                var logger = new ConsoleLogger();

                logger.Info(obj);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Info");
                result[2].TrimEnd('\r', '\n').Equals("True").ShouldBeTrue();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void info_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();

                logger.Info(msg);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Info");
                result[2].TrimEnd('\r', '\n').ShouldBe(msg);
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void log_inserts_log_event_into_buffer()
        {
            var time = DateTime.Now;
            const string msg = "This is the message";
            var evt = new LogEvent { Level = LogLevel.Fatal, DateTime = time, Message = msg };
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();

                logger.Log(evt);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[0].Equals(time.ToString(logger.DateFormat)).ShouldBeTrue();
                result[1].ShouldBe("Fatal");
                result[2].TrimEnd('\r', '\n').ShouldBe(msg);
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void warn_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger(LogLevel.Error);
                logger.Warn(msg);

                var result = sw.ToString();
                result.ShouldBeNullOrWhiteSpace();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void warn_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                var logger = new ConsoleLogger(LogLevel.Warn);

                logger.Warn(msg, ex);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Warn");
                result[2].TrimEnd('\r', '\n').ShouldBe($"{msg}:{exmsg}");
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void warn_logs_object_to_buffer()
        {
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                ValueType obj = true;
                var logger = new ConsoleLogger();

                logger.Warn(obj);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Warn");
                result[2].TrimEnd('\r', '\n').Equals("True").ShouldBeTrue();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void warn_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();

                logger.Warn(msg);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Warn");
                result[2].TrimEnd('\r', '\n').ShouldBe(msg);
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void trace_does_not_log_if_logger_level_is_lower()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger(LogLevel.Debug);
                logger.Trace(msg);

                var result = sw.ToString();
                result.ShouldBeNullOrWhiteSpace();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void trace_logs_exception_to_buffer()
        {
            const string msg = "This is the message";
            const string exmsg = "This is the exception";
            var ex = new Exception(exmsg);
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);
                var logger = new ConsoleLogger(LogLevel.Trace);

                logger.Trace(msg, ex);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Trace");
                result[2].TrimEnd('\r', '\n').ShouldBe($"{msg}:{exmsg}");
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void trace_logs_object_to_buffer()
        {
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                ValueType obj = true;
                var logger = new ConsoleLogger();

                logger.Trace(obj);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Trace");
                result[2].TrimEnd('\r', '\n').Equals("True").ShouldBeTrue();
            }

            Console.SetOut(stdout);
        }

        [Fact]
        public void trace_logs_string_to_buffer()
        {
            const string msg = "This is the message";
            var stdout = Console.Out;

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                var logger = new ConsoleLogger();

                logger.Trace(msg);

                var result = sw.ToString().Split('\t');
                result.Length.ShouldBe(3);
                result[1].ShouldBe("Trace");
                result[2].TrimEnd('\r', '\n').ShouldBe(msg);
            }

            Console.SetOut(stdout);
        }
    }
}
