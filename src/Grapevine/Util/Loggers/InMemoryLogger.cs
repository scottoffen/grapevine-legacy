using System;
using System.Collections.Generic;

namespace Grapevine.Util.Loggers
{
    /// <summary>
    /// In-memory only implementation of IGrapevineLogger for testing purposes; this class cannot be inherited
    /// </summary>
    public sealed class InMemoryLogger : IGrapevineLogger
    {
        public LogLevel Level { get; set; }

        private static DateTime RightNow => DateTime.Now;

        public List<LogEvent> Logs { get; }

        public InMemoryLogger() : this(LogLevel.Trace){}

        public InMemoryLogger(LogLevel level)
        {
            Level = level;
            Logs = new List<LogEvent>();
        }

        public void Log(LogEvent evt)
        {
            if (evt.Level > Level) return;
            Logs.Add(evt);
        }

        public void Trace(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Trace(string message)
        {
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = message });
        }

        public void Trace(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Debug(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Debug(string message)
        {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = message });
        }

        public void Debug(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Info(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Info(string message)
        {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = message });
        }

        public void Info(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Warn(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Warn(string message)
        {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = message });
        }

        public void Warn(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Error(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Error(string message)
        {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = message });
        }

        public void Error(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Fatal(object obj)
        {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Fatal(string message)
        {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = message });
        }

        public void Fatal(string message, Exception ex)
        {
            Log(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = message, Exception = ex });
        }
    }
}