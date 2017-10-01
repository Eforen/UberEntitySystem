using System;
using System.Collections.Generic;
using System.Text;

namespace UberEntityComponentSystem
{
    /// <summary>
    /// stores all the components (data) for an entity
    /// </summary>
    [Factory(typeof(EntityFactory), typeof(Entity))]
    public class Entity
    {
        public void resetToDefaults()
        {
            _handle = null;
            components.Clear();
        }

        #region Handle
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
        public Pool pool { get; protected set; }
        public Entity setPool(Pool p)
        {
            if (pool != null) pool.remove(this);
            pool = p;
            return this;
        }

        /// <summary>
        /// Increments the phase up one and causes it to go totally out of phase with any handles floating around.
        /// </summary>
        public void phaseUp()
        {
            phase++;
        }

        #endregion //Handle

        #region Component

        private Dictionary<Type, Component> components = new Dictionary<Type, Component>();

        public T getComponent<T>() where T : Component
        {
            try
            {
                return (T)components[typeof(T)];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool hasComponent<T>(T component = null) where T : Component
        {
            if (component != null)
                return components.ContainsKey(component.GetType());
            return components.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="component"></param>
        /// <returns></returns>
        public T addComponent<T>(T component = null) where T : Component, new()
        {
            try
            {
                if (component == null)
                    component = Factory.Get<T>();
                component.owner = this;
                components.Add(typeof(T), component);
                return component;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("A component with that type already exists in the Entity.");
            }
            throw new Exception("Unknown Error: addComponent<T>");
        }

        /// <summary>
        /// Removes a type of component from this entity.
        /// </summary>
        /// <typeparam name="T">The type to remove from this Entity.</typeparam>
        /// <param name="component">Does not remove this exact component nessesaraly. This is just an alternate way to specify the type to remove</param>
        /// <returns>The component of T type that was removed from entity or null if no component available.</returns>
        public virtual T removeComponent<T>(T component = null, bool recycleComponent = true) where T : Component, new()
        {
            try
            {
                T t = (T)components[typeof(T)];
                components.Remove(typeof(T));
                t.owner = null;
                if (recycleComponent) Factory.Cache(t);
                return t;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion //Component
    }
}
