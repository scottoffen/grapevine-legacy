using System;
using System.Collections.Generic;

namespace Grapevine.Util
{
    public interface IGrapevineLogger
    {
        /// <summary>
        /// Writes the diagnostic message at the <c>Trace</c> level
        /// </summary>
        /// <param name="obj"></param>
        void Trace(object obj);

        /// <summary>
        /// Writes the diagnostic message at the <c>Trace</c> level
        /// </summary>
        /// <param name="message"></param>
        void Trace(string message);

        /// <summary>
        /// Writes the diagnostic message at the <c>Trace</c> level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Trace(string message, Exception ex);

        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level
        /// </summary>
        /// <param name="obj"></param>
        void Debug(object obj);

        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level
        /// </summary>
        /// <param name="message"></param>
        void Debug(string message);

        /// <summary>
        /// Writes the diagnostic message at the <c>Debug</c> level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Debug(string message, Exception ex);

        /// <summary>
        /// Writes the diagnostic message at the <c>Info</c> level
        /// </summary>
        /// <param name="obj"></param>
        void Info(object obj);

        /// <summary>
        /// Writes the diagnostic message at the <c>Info</c> level
        /// </summary>
        /// <param name="message"></param>
        void Info(string message);

        /// <summary>
        /// Writes the diagnostic message at the <c>Info</c> level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Info(string message, Exception ex);

        /// <summary>
        /// Writes the diagnostic message at the <c>Warn</c> level
        /// </summary>
        /// <param name="obj"></param>
        void Warn(object obj);

        /// <summary>
        /// Writes the diagnostic message at the <c>Warn</c> level
        /// </summary>
        /// <param name="message"></param>
        void Warn(string message);

        /// <summary>
        /// Writes the diagnostic message at the <c>Warn</c> level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Warn(string message, Exception ex);

        /// <summary>
        /// Writes the diagnostic message at the <c>Error</c> level
        /// </summary>
        /// <param name="obj"></param>
        void Error(object obj);

        /// <summary>
        /// Writes the diagnostic message at the <c>Error</c> level
        /// </summary>
        /// <param name="message"></param>
        void Error(string message);

        /// <summary>
        /// Writes the diagnostic message at the <c>Error</c> level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Error(string message, Exception ex);

        /// <summary>
        /// Writes the diagnostic message at the <c>Fatal</c> level
        /// </summary>
        /// <param name="obj"></param>
        void Fatal(object obj);

        /// <summary>
        /// Writes the diagnostic message at the <c>Error</c> level
        /// </summary>
        /// <param name="message"></param>
        void Fatal(string message);

        /// <summary>
        /// Writes the diagnostic message at the <c>Error</c> level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        void Fatal(string message, Exception ex);
    }

    /// <summary>
    /// Console based implementation of IGrapevineLogger; this class cannot be inherited
    /// </summary>
    public sealed class ConsoleLogger : IGrapevineLogger
    {
        private readonly LogLevel _level;

        private static string RightNow => DateTime.Now.ToString(@"M/d/yyyy hh:mm:ss tt");

        public ConsoleLogger() : this(LogLevel.Trace) { }

        public ConsoleLogger(LogLevel level)
        {
            _level = level;
        }

        public void Trace(object obj)
        {
            if (LogLevel.Trace > _level) return;
            Trace(obj.ToString());
        }

        public void Trace(string message)
        {
            if (LogLevel.Trace > _level) return;
            Log(LogLevel.Trace, message);
        }

        public void Trace(string message, Exception ex)
        {
            if (LogLevel.Trace > _level) return;
            Trace(ExceptionToString(message, ex));
        }

        public void Debug(object obj)
        {
            if (LogLevel.Debug > _level) return;
            Debug(obj.ToString());
        }

        public void Debug(string message)
        {
            if (LogLevel.Debug > _level) return;
            Log(LogLevel.Debug, message);
        }

        public void Debug(string message, Exception ex)
        {
            if (LogLevel.Debug > _level) return;
            Debug(ExceptionToString(message, ex));
        }

        public void Info(object obj)
        {
            if (LogLevel.Info > _level) return;
            Info(obj.ToString());
        }

        public void Info(string message)
        {
            if (LogLevel.Info > _level) return;
            Log(LogLevel.Info, message);
        }

        public void Info(string message, Exception ex)
        {
            if (LogLevel.Info > _level) return;
            Info(ExceptionToString(message, ex));
        }

        public void Warn(object obj)
        {
            if (LogLevel.Warn > _level) return;
            Warn(obj.ToString());
        }

        public void Warn(string message)
        {
            if (LogLevel.Warn > _level) return;
            Log(LogLevel.Warn, message);
        }

        public void Warn(string message, Exception ex)
        {
            if (LogLevel.Warn > _level) return;
            Warn(ExceptionToString(message, ex));
        }

        public void Error(object obj)
        {
            if (LogLevel.Error > _level) return;
            Error(obj.ToString());
        }

        public void Error(string message)
        {
            if (LogLevel.Error > _level) return;
            Log(LogLevel.Error, message);
        }

        public void Error(string message, Exception ex)
        {
            if (LogLevel.Error > _level) return;
            Error(ExceptionToString(message, ex));
        }

        public void Fatal(object obj)
        {
            if (LogLevel.Fatal > _level) return;
            Fatal(obj.ToString());
        }

        public void Fatal(string message)
        {
            if (LogLevel.Fatal > _level) return;
            Log(LogLevel.Fatal, message);
        }

        public void Fatal(string message, Exception ex)
        {
            if (LogLevel.Fatal > _level) return;
            Fatal(ExceptionToString(message, ex));
        }

        private static void Log(LogLevel level, string message)
        {
            Console.WriteLine(CreateMessage(level, message));
        }

        private static string CreateMessage(LogLevel level, string message)
        {
            return $"{RightNow}\t{level}\t{message}";
        }

        private static string ExceptionToString(string message, Exception ex)
        {
            return $"{message}:{ex.Message}\r\n{ex.StackTrace}";
        }
    }

    /// <summary>
    /// In-memory only implementation of IGrapevineLogger for testing purposes; this class cannot be inherited
    /// </summary>
    public sealed class InMemoryLogger : IGrapevineLogger
    {
        private readonly LogLevel _level;

        private static DateTime RightNow => DateTime.Now;

        public List<LogEvent> Logs { get; }

        public InMemoryLogger(LogLevel level)
        {
            _level = level;
            Logs = new List<LogEvent>();
        }

        public void Trace(object obj)
        {
            if (LogLevel.Trace > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Trace(string message)
        {
            if (LogLevel.Trace > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = message });
        }

        public void Trace(string message, Exception ex)
        {
            if (LogLevel.Trace > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Debug(object obj)
        {
            if (LogLevel.Debug > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Debug(string message)
        {
            if (LogLevel.Debug > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = message });
        }

        public void Debug(string message, Exception ex)
        {
            if (LogLevel.Debug > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Info(object obj)
        {
            if (LogLevel.Info > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Info(string message)
        {
            if (LogLevel.Info > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = message });
        }

        public void Info(string message, Exception ex)
        {
            if (LogLevel.Info > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Warn(object obj)
        {
            if (LogLevel.Warn > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Warn(string message)
        {
            if (LogLevel.Warn > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = message });
        }

        public void Warn(string message, Exception ex)
        {
            if (LogLevel.Warn > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Error(object obj)
        {
            if (LogLevel.Error > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Error(string message)
        {
            if (LogLevel.Error > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = message });
        }

        public void Error(string message, Exception ex)
        {
            if (LogLevel.Error > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = message, Exception = ex });
        }

        public void Fatal(object obj)
        {
            if (LogLevel.Fatal > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = obj.ToString() });
        }

        public void Fatal(string message)
        {
            if (LogLevel.Fatal > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = message });
        }

        public void Fatal(string message, Exception ex)
        {
            if (LogLevel.Fatal > _level) return;
            Logs.Add(new LogEvent { Level = LogLevel.Fatal, DateTime = RightNow, Message = message, Exception = ex });
        }

        public struct LogEvent
        {
            public LogLevel Level { get; set; }
            public DateTime DateTime { get; set; }
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }
    }

    /// <summary>
    /// No-op implementation of IGrapevineLogger; this class cannot be inherited
    /// </summary>
    public sealed class NullLogger : IGrapevineLogger
    {
        public void Debug(string message) { }

        public void Debug(object obj) { }

        public void Debug(string message, Exception ex) { }

        public void Error(string message) { }

        public void Error(object obj) { }

        public void Error(string message, Exception ex) { }

        public void Fatal(string message) { }

        public void Fatal(object obj) { }

        public void Fatal(string message, Exception ex) { }

        public void Info(string message) { }

        public void Info(object obj) { }

        public void Info(string message, Exception ex) { }

        public void Trace(string message) { }

        public void Trace(object obj) { }

        public void Trace(string message, Exception ex) { }

        public void Warn(string message) { }

        public void Warn(object obj) { }

        public void Warn(string message, Exception ex) { }
    }

    /// <summary>
    /// Enumeration of a standard set of logging levels
    /// </summary>
    public enum LogLevel
    {
        Trace = 5,
        Debug = 4,
        Info = 3,
        Warn = 2,
        Error = 1,
        Fatal = 0
    }
}