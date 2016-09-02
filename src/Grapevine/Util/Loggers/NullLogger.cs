using System;

namespace Grapevine.Util.Loggers
{
    /// <summary>
    /// No-op implementation of IGrapevineLogger; this class cannot be inherited
    /// </summary>
    public sealed class NullLogger : IGrapevineLogger
    {
        public LogLevel Level => LogLevel.Trace;

        public void Log(LogEvent evt) { /* This method is intentionally left empty */ }

        public void Debug(string message) { /* This method is intentionally left empty */ }

        public void Debug(object obj) { /* This method is intentionally left empty */ }

        public void Debug(string message, Exception ex) { /* This method is intentionally left empty */ }

        public void Error(string message) { /* This method is intentionally left empty */ }

        public void Error(object obj) { /* This method is intentionally left empty */ }

        public void Error(string message, Exception ex) { /* This method is intentionally left empty */ }

        public void Fatal(string message) { /* This method is intentionally left empty */ }

        public void Fatal(object obj) { /* This method is intentionally left empty */ }

        public void Fatal(string message, Exception ex) { /* This method is intentionally left empty */ }

        public void Info(string message) { /* This method is intentionally left empty */ }

        public void Info(object obj) { /* This method is intentionally left empty */ }

        public void Info(string message, Exception ex) { /* This method is intentionally left empty */ }

        public void Trace(string message) { /* This method is intentionally left empty */ }

        public void Trace(object obj) { /* This method is intentionally left empty */ }

        public void Trace(string message, Exception ex) { /* This method is intentionally left empty */ }

        public void Warn(string message) { /* This method is intentionally left empty */ }

        public void Warn(object obj) { /* This method is intentionally left empty */ }

        public void Warn(string message, Exception ex) { /* This method is intentionally left empty */ }
    }
}