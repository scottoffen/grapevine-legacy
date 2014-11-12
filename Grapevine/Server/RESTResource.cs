namespace Grapevine.Server
{
    /// <summary>
    /// Class to be searched for RESTRoutes, extends Responder
    /// </summary>
    public abstract class RESTResource : Responder
    {
        private RESTServer _server;

        /// <summary>
        /// The RESTServer that spawned this instance of the RESTResource
        /// </summary>
        public RESTServer Server
        {
            get
            {
                return this._server;
            }
            set
            {
                if (this.IsNot<RESTServer>())
                {
                    this._server = value;
                }
            }
        }
    }
}
