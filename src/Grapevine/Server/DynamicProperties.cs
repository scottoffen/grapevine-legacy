using System.Dynamic;

namespace Grapevine.Server
{
    public interface IDynamicProperties
    {
        /// <summary>
        /// Gets a dynamic object available for adding dynamic properties at run-time
        /// </summary>
        dynamic Properties { get; }
    }

    public abstract class DynamicProperties : IDynamicProperties
    {
        private ExpandoObject _properties;

        public dynamic Properties
        {
            get
            {
                if (_properties != null) return _properties;
                _properties = new ExpandoObject();
                return _properties;
            }
        }
    }
}
