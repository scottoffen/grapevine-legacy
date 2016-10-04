using System;
using Grapevine.Interfaces.Shared;

namespace Grapevine.Shared.Loggers
{
    /// <summary>
    /// Console based implementation of IGrapevineLogger; this class cannot be inherited
    /// </summary>
    public sealed class ConsoleLogger : IGrapevineLogger
    {
        /// <inheritdoc/>
        public LogLevel Level { get; set; }

        /// <summary>
        /// String defining the way the date and time should be formtted when logged
        /// </summary>
        public string DateFormat => @"M/d/yyyy hh:mm:ss tt";

        /// <summary>
        /// A value that represents the current time
        /// </summary>
        public DateTime RightNow => DateTime.Now;

        public ConsoleLogger() : this(LogLevel.Trace) { }

        public ConsoleLogger(LogLevel level)
        {
            Level = level;
        }

        /// <inheritdoc/>
        public void Log(LogEvent evt)
        {
            if (evt.Level > Level) return;
            Console.WriteLine(CreateMessage(evt));
        }

        /// <inheritdoc/>
        public void Trace(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = obj.ToString() });
        }

        /// <inheritdoc/>
        public void Trace(string message)
        {
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = message });
        }

        /// <inheritdoc/>
        public void Trace(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        /// <inheritdoc/>
        public void Debug(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = obj.ToString() });
        }

        /// <inheritdoc/>
        public void Debug(string message)
        {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = message });
        }

        /// <inheritdoc/>
        public void Debug(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        /// <inheritdoc/>
        public void Info(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = obj.ToString() });
        }

        /// <inheritdoc/>
        public void Info(string message)
        {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = message });
        }

        /// <inheritdoc/>
        public void Info(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        /// <inheritdoc/>
        public void Warn(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = obj.ToString() });
        }

        /// <inheritdoc/>
        public void Warn(string message)
        {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = message });
        }

        /// <inheritdoc/>
        public void Warn(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        /// <inheritdoc/>
        public void Error(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = obj.ToString() });
        }

        /// <inheritdoc/>
        public void Error(string message)
        {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = message });
        }

        /// <inheritdoc/>
        public void Error(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        /// <inheritdoc/>
        public void Fatal(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = obj.ToString() });
        }

        /// <inheritdoc/>
        public void Fatal(string message)
        {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = message });
        }

        /// <inheritdoc/>
        public void Fatal(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = ExceptionToString(message, ex) });
        }

        private string CreateMessage(LogEvent evt)
        {
            return $"{RightNow.ToString(DateFormat)}\t{evt.Level}\t{evt.Message}";
        }

        private static string ExceptionToString(string message, Exception ex)
        {
            return $"{message}:{ex.Message}\r\n{ex.StackTrace}";
        }
    }
}