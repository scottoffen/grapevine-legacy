using System.Collections.Generic;

namespace Grapevine.Server
{
    /// <summary>
    /// Container class to store and manage starting and stopping multiple RestServer objects
    /// </summary>
    public class RestCluster
    {
        private readonly Dictionary<string, IRestServer> _servers = new Dictionary<string, IRestServer>();
        private bool _started;

        /// <summary>
        /// Adds a server to the cluster
        /// </summary>
        public void Add(string label, IRestServer server)
        {
            if (_started) server.Start();
            _servers.Add(label, server);
        }

        /// <summary>
        /// Starts each server in the cluster
        /// </summary>
        public void StartAll()
        {
            foreach (var server in _servers.Values)
            {
                if (!server.IsListening) server.Start();
            }

            _started = true;
        }

        /// <summary>
        /// Stops each server in the cluster
        /// </summary>
        public void StopAll()
        {
            foreach (var server in _servers.Values)
            {
                if (server.IsListening) server.Stop();
            }

            _started = false;
        }

        /// <summary>
        /// Retrieve an IRestServer from the cluster
        /// </summary>
        public IRestServer Get(string label)
        {
            return (_servers.ContainsKey(label)) ? _servers[label] : null;
        }

        /// <summary>
        /// Stop and remove a server from the cluster
        /// </summary>
        public bool Remove(string label)
        {
            if (!_servers.ContainsKey(label)) return true;
            _servers[label].Stop();
            return _servers.Remove(label);
        }
    }
}
