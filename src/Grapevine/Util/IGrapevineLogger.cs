using System;
using System.Collections.Generic;

namespace Grapevine.Util
{
    public interface IGrapevineLogger
    {
        /// <summary>
        /// Value indicating what level of logging should occur
        /// </summary>
        LogLevel Level { get; }

        /// <summary>
        /// Writes the diagnostic message at the level specified in the LogEvent
        /// </summary>
        /// <param name="evt"></param>
        void Log(LogEvent evt);

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
        public LogLevel Level { get; set; }

        public string DateFormat => @"M/d/yyyy hh:mm:ss tt";

        public DateTime RightNow => DateTime.Now;

        public ConsoleLogger() : this(LogLevel.Trace) { }

        public ConsoleLogger(LogLevel level)
        {
            Level = level;
        }

        public void Log(LogEvent evt)
        {
            if (evt.Level > Level) return;
            Console.WriteLine(CreateMessage(evt));
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
            Log(new LogEvent { Level = LogLevel.Trace, DateTime = RightNow, Message = ExceptionToString(message, ex) });
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
            Log(new LogEvent { Level = LogLevel.Debug, DateTime = RightNow, Message = ExceptionToString(message, ex) });
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
            Log(new LogEvent { Level = LogLevel.Info, DateTime = RightNow, Message = ExceptionToString(message, ex) });
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
            Log(new LogEvent { Level = LogLevel.Warn, DateTime = RightNow, Message = ExceptionToString(message, ex) });
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
            Log(new LogEvent { Level = LogLevel.Error, DateTime = RightNow, Message = ExceptionToString(message, ex) });
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

    /// <summary>
    /// No-op implementation of IGrapevineLogger; this class cannot be inherited
    /// </summary>
    public sealed class NullLogger : IGrapevineLogger
    {
        public LogLevel Level => LogLevel.Trace;

        public void Log(LogEvent evt) { }

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

    public struct LogEvent
    {
        public LogLevel Level { get; set; }
        public DateTime DateTime { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
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