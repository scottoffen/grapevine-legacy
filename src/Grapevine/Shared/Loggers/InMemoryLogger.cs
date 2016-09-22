using System;
using System.Collections.Generic;
using System.Linq;
using Grapevine.Interfaces.Shared;

namespace Grapevine.Shared.Loggers
{
    /// <summary>
    /// In-memory only implementation of IGrapevineLogger for testing purposes; this class cannot be inherited
    /// </summary>
    public sealed class InMemoryLogger : IGrapevineLogger
    {
        /// <inheritdoc/>
        public LogLevel Level { get; set; }

        /// <summary>
        /// A value that represents the current time
        /// </summary>
        public static DateTime RightNow => DateTime.Now;

        /// <summary>
        /// Contains all the messages that have been logged
        /// </summary>
        public List<LogEvent> Logs { get; }

        public List<string> LogMessages { get { return Logs.Select(l => l.Message).ToList(); } }

        public InMemoryLogger() : this(LogLevel.Trace){}

        public InMemoryLogger(LogLevel level)
        {
            Level = level;
            Logs = new List<LogEvent>();
        }

        /// <inheritdoc/>
        public void Log(LogEvent evt)
        {
            if (evt.Level > Level) return;
            Logs.Add(evt);
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
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = message, Exception = ex });
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
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = message, Exception = ex });
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
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = message, Exception = ex });
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
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = message, Exception = ex });
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
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = message, Exception = ex });
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
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = message, Exception = ex });
        }
    }
}