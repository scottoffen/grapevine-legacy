using System;
using Grapevine.Server;

namespace Grapevine.Util.Logging
{
    public class LogToConsole : Grapevine.Util.Logging.ILog
    {
        public LogToConsole ()
        {
        }

        public void Log(string l)
        {
            System.Console.WriteLine (l);
        }

        public void Log(Exception ex)
        {
            System.Console.WriteLine (ex);
        }
    }
}

