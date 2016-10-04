using System;

namespace Grapevine.Interfaces.Shared
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