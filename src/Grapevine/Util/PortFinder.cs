using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

namespace Grapevine.Util
{
    public class PortFinder
    {
        public static int FirstPort { get; } = 1;
        public static int LastPort { get; } = 65535;
        public static string OutOfRangeMsg { get; } = $"must be an integer between {FirstPort} and {LastPort}.";

        public event Action<string> PortFound;
        public event Action PortNotFound;

        private int _startIndex;
        private int _endIndex;

        public PortFinder() : this(FirstPort, LastPort){}

        public PortFinder(int startIndex) : this(startIndex, LastPort) { }

        public PortFinder(int startIndex, int endIndex)
        {
            _startIndex = startIndex;
            _endIndex = endIndex;
            PortFound = s => { };
            PortNotFound = () => { };
        }

        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
            set
            {
                if (value > LastPort || value < FirstPort)
                    throw new ArgumentOutOfRangeException(nameof(value), OutOfRangeMsg);
                _startIndex = value;
            }
        }

        public int EndIndex
        {
            get
            {
                return _endIndex;
            }
            set
            {
                if (value > LastPort || value < FirstPort)
                    throw new ArgumentOutOfRangeException(nameof(value), OutOfRangeMsg);
                _endIndex = value;
            }
        }

        public string Run()
        {
            return FindNextLocalOpenPort(StartIndex, EndIndex);
        }

        public void RunAsync()
        {
            var t = new Thread(() =>
            {
                var port = Run();
                if (port != null)
                {
                    PortFound?.Invoke(port);
                }
                else
                {
                    PortNotFound?.Invoke();
                }
            });
            t.Start();
        }

        public static string FindNextLocalOpenPort()
        {
            return FindNextLocalOpenPort(FirstPort, LastPort);
        }

        public static string FindNextLocalOpenPort(int startIndex)
        {
            if (startIndex < FirstPort || startIndex > LastPort)
                throw new ArgumentOutOfRangeException(nameof(startIndex), OutOfRangeMsg);
            return FindNextLocalOpenPort(startIndex, LastPort);
        }

        public static string FindNextLocalOpenPort(int startIndex, int endIndex)
        {
            if (startIndex < FirstPort || startIndex > LastPort)
                throw new ArgumentOutOfRangeException(nameof(startIndex), OutOfRangeMsg);

            if (endIndex < FirstPort || endIndex > LastPort)
                throw new ArgumentOutOfRangeException(nameof(endIndex), OutOfRangeMsg);

            var min = (endIndex > startIndex) ? startIndex : endIndex;
            var max = (endIndex > startIndex) ? endIndex : startIndex;

            var inUse = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
                .Select(l => l.Port).ToList();

            for (var port = min; port < max; port++)
            {
                if (inUse.Contains(port)) continue;
                return port.ToString();
            }

            return null;
        }
    }
}
