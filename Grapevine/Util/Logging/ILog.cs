using System;

namespace Grapevine.Util.Logging
{
	public interface ILog
	{
		void Log(string message);
		void Log(Exception ex);
	}
}

