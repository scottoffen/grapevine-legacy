using System;
using Grapevine.Interfaces.Shared;

namespace Grapevine.Shared.Loggers
{
    /// <summary>
    /// No-op implementation of IGrapevineLogger; this class cannot be inherited
    /// </summary>
    public sealed class NullLogger : IGrapevineLogger
    {
        private static NullLogger _logger;

        private NullLogger() { }

        public static NullLogger GetInstance()
        {
            if (_logger != null) return _logger;
            _logger = new NullLogger();
            return _logger;
        }

        /// <inheritdoc/>
        public LogLevel Level => LogLevel.Trace;

        /// <inheritdoc/>
        public void Log(LogEvent evt) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Debug(string message) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Debug(object obj) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Debug(string message, Exception ex) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Error(string message) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Error(object obj) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Error(string message, Exception ex) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Fatal(string message) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Fatal(object obj) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Fatal(string message, Exception ex) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Info(string message) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Info(object obj) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Info(string message, Exception ex) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Trace(string message) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Trace(object obj) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Trace(string message, Exception ex) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Warn(string message) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Warn(object obj) { /* This method is intentionally left empty */ }

        /// <inheritdoc/>
        public void Warn(string message, Exception ex) { /* This method is intentionally left empty */ }
    }
}