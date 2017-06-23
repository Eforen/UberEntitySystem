using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntitySystemCore
{
    /// <summary>
    /// Stores a reference to an entity that is irretrievable if the entity has gone out of phase with this handle.
    /// </summary>
    public class Handle : IEquatable<Handle>
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false; //Because paranoia
            if (ReferenceEquals(this, obj)) return true; //Because why bother
            if (obj.GetType() != GetType()) return false; //Because WRONG!!!
            
            //Actually compare the values
            Handle h = obj as Handle;
            return this.phase == h.phase && this._entity == h._entity && this._entity != null;
        }

        public bool Equals(Handle other)
        {
            return this.phase == other.phase && this._entity == other._entity && this._entity != null;
        }

        // Because we should never override equals without overriding GetHash AND our data is immutable
        // Our class is not totally immutable because _entity is mutable
        /* public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 13;
                hashCode = (hashCode * 397) ^ phase;
                var entityHashCode =
                    ReferenceEquals(null, _entity) ?
                        _entity.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ entityHashCode;
                return hashCode;
            }
        }*/

        public static bool operator ==(Handle a, Handle b)
        {
            if (ReferenceEquals(null, a) && ReferenceEquals(null, b)) return true; //null is null
            if (ReferenceEquals(null, a) || ReferenceEquals(null, b)) return false; //Always not equal if one is null
            return a.phase == b.phase && a._entity == b._entity && a._entity != null;
        }

        public static bool operator !=(Handle a, Handle b)
        {
            if (ReferenceEquals(null, a) && ReferenceEquals(null, b)) return false; //null == null
            if (ReferenceEquals(null, a) || ReferenceEquals(null, b)) return true; //Always not equal if one is null
            return a.phase != b.phase || a._entity != b._entity || a._entity == null;
        }
    }
}
