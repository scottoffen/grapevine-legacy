using System;
using System.Diagnostics;
using System.Text;
using Grapevine.Util.Logging;

namespace Grapevine.Util.Logging
{
	/// <summary>
	/// Delegate returns a custom string representation of an exception object.
	/// </summary>
	public delegate string ExceptionFormatter(Exception e);

	/// <summary>
	/// class for recording logs in an EventLog. The EventLog property must be set or nothing will be logged.
	/// </summary>
	public class LogToEventLog  : ILog
	{
		/// <summary>
		/// Changes the default return value of ExceptionToString(Exception) to be the result of this method
		/// </summary>
		public ExceptionFormatter FormatException { get; set; }

		/// <summary>
		/// The event log to use in writing entries
		/// </summary>
		public EventLog EventLog { get; set; }

		/// <summary>
		/// Set to true to enable the logging of execeptions via the Log method; defaults to false
		/// </summary>
		public Boolean LogExceptions = false;

		/// <summary>
		/// Converts an Exception to a verbose string
		/// </summary>
		public string ExceptionToString(Exception e)
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
		public void Log(Exception e)
		{
			if (LogExceptions)
			{
				Log(ExceptionToString(e));
			}
		}

		/// <summary>
		/// Logs the message to the EventLog
		/// </summary>
		public void Log(String message)
		{
			if (!object.ReferenceEquals(EventLog, null))
			{
				EventLog.WriteEntry(message);
			}
		}
	}
}
