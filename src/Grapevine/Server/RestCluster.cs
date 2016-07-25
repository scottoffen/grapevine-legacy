using System;
using System.Collections.Generic;
using System.Linq;

namespace Grapevine.Server
{
    /// <summary>
    /// Container class to store and manage starting and stopping multiple RestServer objects
    /// </summary>
    public class RestCluster
    {
        protected readonly Dictionary<string, IRestServer> Servers = new Dictionary<string, IRestServer>();
        protected bool Started;

        /// <summary>
        /// Gets or sets the Action that will be executed immediately before attempting to start the collection of servers
        /// </summary>
        public Action OnBeforeStartAll { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately after attempting to start the collection of servers
        /// </summary>
        public Action OnAfterStartAll { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately before attempting to stop the collection of servers
        /// </summary>
        public Action OnBeforeStopAll { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately after attempting to stop the collection of servers
        /// </summary>
        public Action OnAfterStopAll { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately before attempting to start each server
        /// </summary>
        public Action OnBeforeStartEach { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately after attempting to start each server
        /// </summary>
        public Action OnAfterStartEach { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately before attempting to stop each server
        /// </summary>
        public Action OnBeforeStopEach { get; set; }

        /// <summary>
        /// Gets or sets the Action that will be executed immediately after attempting to stop each server
        /// </summary>
        public Action OnAfterStopEach { get; set; }

        /// <summary>
        /// Convenience indexer; synonym for Add and Get methods
        /// </summary>
        /// <param name="label"></param>
        /// <returns>Get or set the instance of IRestServer assigned to the label</returns>
        public IRestServer this[string label]
        {
            get { return Get(label); }
            set { Add(label, value); }
        }

        /// <summary>
        /// Adds a server to the cluster
        /// </summary>
        /// <param name="server"></param>
        /// <returns>Assigned labled of the server that was addded</returns>
        public string Add(IRestServer server)
        {
            Add(server.Origin, server);
            return server.Origin;
        }

        /// <summary>
        /// Adds a server to the cluster
        /// </summary>
        public void Add(string label, IRestServer server)
        {
            if (Started) server.Start();
            Servers.Add(label, server);
        }

        /// <summary>
        /// Retrieve an IRestServer from the cluster
        /// </summary>
        public IRestServer Get(string label)
        {
            return Servers.ContainsKey(label) ? Servers[label] : null;
        }

        /// <summary>
        /// Stop and remove a server from the cluster
        /// </summary>
        public bool Remove(string label)
        {
            if (!Servers.ContainsKey(label)) return true;

            OnBeforeStopEach?.Invoke();
            Servers[label].Stop();
            OnAfterStopEach?.Invoke();

            return Servers.Remove(label);
        }

        /// <summary>
        /// Starts each server in the cluster
        /// </summary>
        public void StartAll()
        {
            OnBeforeStartAll?.Invoke();

            foreach (var server in Servers.Values.Where(server => !server.IsListening))
            {
                OnBeforeStartEach?.Invoke();
                server.Start();
                OnAfterStartEach?.Invoke();
            }

            OnAfterStartAll?.Invoke();
            Started = true;
        }

        /// <summary>
        /// Stops each server in the cluster
        /// </summary>
        public void StopAll()
        {
            OnBeforeStopAll?.Invoke();

            foreach (var server in Servers.Values.Where(server => server.IsListening))
            {
                OnBeforeStopEach?.Invoke();
                server.Stop();
                OnAfterStopEach?.Invoke();
            }

            OnAfterStopAll?.Invoke();
            Started = false;
        }
    }
}
