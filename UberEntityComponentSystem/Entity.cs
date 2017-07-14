using System;
using System.Collections.Generic;
using System.Text;

namespace UberEntityComponentSystem
{
    /// <summary>
    /// stores all the components (data) for an entity
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Used to store the current handle so that we are not creating tons of garbage
        /// by creating tons of handles instead we give a ref to the same one out as long
        /// as its still in phase with this entity.
        /// </summary>
        private Handle _handle;

        /// <summary>
        /// Get current handle for this Entity.
        /// </summary>
        public Handle handle
        {
            get
            {
                if (_handle == null || _handle.phase != this.phase) //If handle has gone out of phase or is not yet set
                    this._handle = new Handle(this); //Create and store handle to this entity
                return _handle; //Return current handle
            }
        }

        /// <summary>
        /// Used to determine if a handle is out of phase with its entity.
        /// </summary>
        public int phase { get; protected set; }

        /// <summary>
        /// Increments the phase up one and causes it to go totally out of phase with any handles floating around.
        /// </summary>
        public void phaseUp()
        {
            phase++;
        }
    }
}
