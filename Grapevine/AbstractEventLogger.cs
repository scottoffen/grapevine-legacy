using System;
using System.Diagnostics;
using System.Text;

namespace Grapevine
{
    public abstract class AbstractEventLogger
    {
        public EventLog EventLog { get; set; }
        public Boolean LogExceptions = false;

        protected void Log(Exception e)
        {
            if (LogExceptions)
            {
                var trace = new StackTrace(e, true);
                var frame = trace.GetFrame(0);
                var file = frame.GetFileName();
                var line = frame.GetFileLineNumber();

                StringBuilder sb = new StringBuilder(String.Format("{0} : {1}", file, line));
                sb.AppendLine(String.Format("[Source] {0} : [Message] {1}", e.Source, e.Message));
                sb.AppendLine("");
                sb.AppendLine(e.ToString());

                this.Log(sb.ToString());
            }
        }

        protected void Log(string message)
        {
            if (this.EventLog != null)
            {
                this.EventLog.WriteEntry(message);
            }
        }
    }
}
