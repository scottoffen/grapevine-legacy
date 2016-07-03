using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;

namespace Grapevine.Util
{
    /// <summary>
    /// Provides synchronous and asynchronous methods to detect available ports
    /// </summary>
    public interface IPortFinder
    {
        /// <summary>
        /// Synchronously scans for an avilable port in the specified range and returns to result as a string
        /// </summary>
        /// <returns>stringo</returns>
        string Run();

        /// <summary>
        /// Begins an asynchronous scan for an available port in the specified range; will fire the PortFound event when one is detected, or the PortNotFound event if one is not detected
        /// </summary>
        void RunAsync();
    }

    /// <summary>
    /// Provides synchronous and asynchronous methods to detect available ports
    /// </summary>
    public class PortFinder : IPortFinder
    {
        /// <summary>
        /// Represents the smallest port number possible
        /// </summary>
        public static int FirstPort { get; } = 1;

        /// <summary>
        /// Represents the largest port number possible
        /// </summary>
        public static int LastPort { get; } = 65535;

        private static string OutOfRangeMsg { get; } = $"must be an integer between {FirstPort} and {LastPort}.";

        /// <summary>
        /// An event that can be used to notify a client when an available port is detected
        /// </summary>
        public event Action<string> PortFound;

        /// <summary>
        /// An event that can be used to notify a client when no available port can be found in the specified range
        /// </summary>
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

        /// <summary>
        /// Gets or sets the integer at which to start scanning for ports
        /// </summary>
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

        /// <summary>
        /// Gets or sets the integer at which to stop scanning for ports
        /// </summary>
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

        /// <summary>
        /// Returns a string representation of the next available port
        /// </summary>
        /// <returns>string</returns>
        public static string FindNextLocalOpenPort()
        {
            return FindNextLocalOpenPort(FirstPort, LastPort);
        }

        /// <summary>
        /// Returns a string representation of the next available port after the start index
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns>string</returns>
        public static string FindNextLocalOpenPort(int startIndex)
        {
            if (startIndex < FirstPort || startIndex > LastPort)
                throw new ArgumentOutOfRangeException(nameof(startIndex), OutOfRangeMsg);
            return FindNextLocalOpenPort(startIndex, LastPort);
        }

        /// <summary>
        /// Returns a string representation of the next available port between the start index and end index
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns>string</returns>
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
