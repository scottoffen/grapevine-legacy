using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Grapevine.Shared
{
    /// <summary>
    /// Provides synchronous and asynchronous methods to detect available ports
    /// </summary>
    public class PortFinder
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

        private int _startIndex;
        private int _endIndex;

        public PortFinder() : this(FirstPort, LastPort){}

        public PortFinder(int startIndex) : this(startIndex, LastPort) { }

        public PortFinder(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
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

        /// <summary>
        /// Synchronously scans for an avilable port in the specified range and returns to result as a string
        /// </summary>
        /// <returns>string</returns>
        public string Run()
        {
            return FindNextLocalOpenPort(StartIndex, EndIndex);
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

            var min = endIndex > startIndex ? startIndex : endIndex;
            var max = endIndex <= startIndex ? startIndex : endIndex;

            var inUse = IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners()
                .Select(l => l.Port).ToList();

            int port = 0;
            for (var i = min; i <= max; i++)
            {
                if (inUse.Contains(i)) continue;
                port = i;
                break;
            }

            return port.ToString();
        }

        /// <summary>
        /// A list that contains all the ports currently in use
        /// </summary>
        public static List<int> PortsInUse
        {
            get
            {
                return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Select(l => l.Port).ToList();
            }
        }
    }
}
