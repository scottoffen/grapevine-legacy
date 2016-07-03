using System;

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

        public ConsoleLogger(LogLevel level = LogLevel.Debug)
        {
            _level = level;
        }

        public void Trace(object obj)
        {
            Trace(obj.ToString());
        }

        public void Trace(string message)
        {
            if (LogLevel.Trace > _level) return;
            Console.WriteLine($"{RightNow}\tTRACE\t{message}");
        }

        public void Trace(string message, Exception ex)
        {
            Trace($"{message}:{ex.Message}\r\n{ex.StackTrace}");
        }

        public void Debug(object obj)
        {
            Debug(obj.ToString());
        }

        public void Debug(string message)
        {
            if (LogLevel.Debug > _level) return;
            Console.WriteLine($"{RightNow}\tDEBUG\t{message}");
        }

        public void Debug(string message, Exception ex)
        {
            Debug($"{message}:{ex.Message}\r\n{ex.StackTrace}");
        }

        public void Info(object obj)
        {
            Info(obj.ToString());
        }

        public void Info(string message)
        {
            if (LogLevel.Info > _level) return;
            Console.WriteLine($"{RightNow}\tINFO\t{message}");
        }

        public void Info(string message, Exception ex)
        {
            Info($"{message}:{ex.Message}\r\n{ex.StackTrace}");
        }

        public void Warn(object obj)
        {
            Warn(obj.ToString());
        }

        public void Warn(string message)
        {
            if (LogLevel.Warn > _level) return;
            Console.WriteLine($"{RightNow}\tWARN\t{message}");
        }

        public void Warn(string message, Exception ex)
        {
            Warn($"{message}:{ex.Message}\r\n{ex.StackTrace}");
        }

        public void Error(object obj)
        {
            Error(obj.ToString());
        }

        public void Error(string message)
        {
            if (LogLevel.Error > _level) return;
            Console.WriteLine($"{RightNow}\tERROR\t{message}");
        }

        public void Error(string message, Exception ex)
        {
            Error($"{message}:{ex.Message}\r\n{ex.StackTrace}");
        }

        public void Fatal(object obj)
        {
            Fatal(obj.ToString());
        }

        public void Fatal(string message)
        {
            if (LogLevel.Fatal > _level) return;
            Console.WriteLine($"{RightNow}\tFATAL\t{message}");
        }

        public void Fatal(string message, Exception ex)
        {
            Fatal($"{message}:{ex.Message}\r\n{ex.StackTrace}");
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