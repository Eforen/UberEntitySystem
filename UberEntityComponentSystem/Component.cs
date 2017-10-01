using System;
using System.Collections.Generic;
using System.Text;

namespace UberEntityComponentSystem
{
    /// <summary>
    /// Small Amount of data no methods
    /// </summary>
    public class Component
    {
        private Entity _owner = null;
        public Entity owner {
            get { return _owner; }
            set
            {
                if(_owner != null && _owner.hasComponent(this))
                    _owner.removeComponent(this);
                _owner = value;
            }
        }

        public Component()
        {

        }
    }
}
