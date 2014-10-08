using System;
using System.Diagnostics;
using System.Text;

namespace Grapevine
{
    public abstract class AbstractEventLogger
    {
        public EventLog EventLog { get; set; }

        protected void Log(Exception e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(e.ToString());
            sb.AppendLine("");
            sb.AppendLine("Source: " + e.Source);
            sb.AppendLine("");
            sb.AppendLine("Message: " + e.Message);

            this.Log(sb.ToString());
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
