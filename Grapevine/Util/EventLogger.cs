using System;
using System.Diagnostics;
using System.Text;

namespace Grapevine
{
    /// <summary>
    /// Delegate returns a custom string representation of an exception object.
    /// </summary>
    public delegate string ExceptionFormatter(Exception e);

    /// <summary>
    /// Static class for recording logs in an EventLog. The EventLog property must be set or nothing will be logged.
    /// </summary>
    public static class EventLogger
    {
        /// <summary>
        /// Changes the default return value of ExceptionToString(Exception) to be the result of this method
        /// </summary>
        public static ExceptionFormatter FormatException { get; set; }

        /// <summary>
        /// The event log to use in writing entries
        /// </summary>
        public static EventLog EventLog { get; set; }

        /// <summary>
        /// Set to true to enable the logging of execeptions via the Log method; defaults to false
        /// </summary>
        public static Boolean LogExceptions = false;

        /// <summary>
        /// Converts an Exception to a verbose string
        /// </summary>
        public static string ExceptionToString(Exception e)
        {
            string message;

            if (object.ReferenceEquals(FormatException, null))
            {
                var trace = new StackTrace(e, true);
                var frame = trace.GetFrame(2);
                var file = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                StringBuilder sb = new StringBuilder(String.Format("{0} line {1}", file, line));
                sb.AppendLine("");
                sb.AppendLine(String.Format("[Source] {0} : [Message] {1}", e.Source, e.Message));
                sb.AppendLine("");
                sb.AppendLine(e.ToString());

                message = sb.ToString();
            }
            else
            {
                message = FormatException(e);
            }

            return message;
        }

        /// <summary>
        /// If LogExceptions is true, writes the result of ExceptionToString(e) to the event log
        /// </summary>
        public static void Log(Exception e)
        {
            if (LogExceptions)
            {
                Log(ExceptionToString(e));
            }
        }

        /// <summary>
        /// Logs the message to the EventLog
        /// </summary>
        public static void Log(String message)
        {
            if (!object.ReferenceEquals(EventLog, null))
            {
                EventLog.WriteEntry(message);
            }
        }
    }
}
