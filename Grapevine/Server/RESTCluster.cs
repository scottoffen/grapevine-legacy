using System.Collections.Generic;

namespace Grapevine.Server
{
    /// <summary>
    /// Container class to store and manage starting and stopping multiple RESTServer objects
    /// </summary>
    public class RESTCluster
    {
        private Dictionary<string, RESTServer> _servers = new Dictionary<string, RESTServer>();
        private bool _started = false;

        /// <summary>
        /// Add a RESTServer to the RESTCluster
        /// </summary>
        public void Add(string label, RESTServer server)
        {
            if (this._started)
            {
                server.Start();
            }

            this._servers.Add(label, server);
        }

        /// <summary>
        /// Calls the Start() method on each RESTServer in the RESTCluster
        /// </summary>
        public void StartAll()
        {
            foreach (KeyValuePair<string, RESTServer> server in this._servers)
            {
                server.Value.Start();
            }

            this._started = true;
        }

        /// <summary>
        /// Calls the Stop() method on each RESTServer in the RESTCluster
        /// </summary>
        public void StopAll()
        {
            foreach (KeyValuePair<string, RESTServer> server in this._servers)
            {
                server.Value.Stop();
            }

            this._started = false;
        }

        /// <summary>
        /// Retrieve a RESTServer from the RESTCluster
        /// </summary>
        public RESTServer Get(string label)
        {
            if (this._servers.ContainsKey(label))
            {
                return this._servers[label];
            }

            return null;
        }

        /// <summary>
        /// Stop and remove a RESTServer from the RESTCluster
        /// </summary>
        public bool Remove(string label)
        {
            if (this._servers.ContainsKey(label))
            {
                this._servers[label].Stop();
                return this._servers.Remove(label);
            }

            return false;
        }
    }
}
