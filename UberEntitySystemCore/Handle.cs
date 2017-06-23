using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntitySystemCore
{
    /// <summary>
    /// Stores a reference to an entity that is irretrievable if the entity has gone out of phase with this handle.
    /// </summary>
    public class Handle
    {
        /// <summary>
        /// Creates a handle based on the entity
        /// </summary>
        /// <param name="entity">Entity to create handle for.</param>
        public Handle(Entity entity)
        {
            this._entity = entity;
            this.phase = entity.phase;
        }
        protected Entity _entity;

        /// <summary>
        /// The <i>readonly</i> entity stored in this handle. <b>Will be Null if out of phase.</b>
        /// </summary>
        public Entity entity {
            get {
                if (_entity.phase != phase) return null;
                else return _entity;
            }
        }

        /// <summary>
        /// The <i>readonly</i> phase of this handle.
        /// </summary>
        public int phase { get; protected set; }
    }
}
