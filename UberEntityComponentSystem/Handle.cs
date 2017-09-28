namespace UberEntityComponentSystem
{
    /// <summary>
    /// Stores a reference to an entity that is irretrievable if the entity has gone out of phase with this handle.
    /// </summary>
    public class Handle
    {
        /// <summary>
        /// The <i>readonly</i> phase of this handle.
        /// </summary>
        public int phase { get; protected set; }

        /// <summary>
        /// The local storage for entity ref.
        /// </summary>
        protected Entity _entity;
        /// <summary>
        /// The <i>readonly</i> entity stored in this handle. <b>Will be Null if out of phase.</b>
        /// </summary>
        public Entity entity
        {
            get {
                if (phase != _entity.phase) return null;
                return _entity;
            }
            protected set
            {
                this._entity = value;
            }
        }

        /// <summary>
        /// Creates a handle based on the entity
        /// </summary>
        /// <param name="entity">Entity to create handle for.</param>
        public Handle(Entity entity)
        {
            this.entity = entity;
            this.phase = entity.phase;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false; //Null should never match a set object.
            if (ReferenceEquals(this, obj)) return true; //Because something is always equal to itself
            if (obj.GetType() != GetType()) return false; //Because WRONG!!!!

            //compare the values
            Handle h = obj as Handle;
            return
                this.phase == h.phase && //In-phase
                this._entity == h._entity;
        }

        public static bool operator ==(Handle a, Handle b)
        {
            if (ReferenceEquals(null, a) && ReferenceEquals(null, b)) return true; //null is null
            if (ReferenceEquals(null, a) || ReferenceEquals(null, b)) return false; //null is never something
            return a.phase == b.phase && a._entity == b._entity;
        }

        public static bool operator !=(Handle a, Handle b)
        {
            if (ReferenceEquals(null, a) && ReferenceEquals(null, b)) return false; //null is null
            if (ReferenceEquals(null, a) || ReferenceEquals(null, b)) return true; //null is never something
            return a.phase != b.phase || a._entity != b._entity;
        }
    }
}