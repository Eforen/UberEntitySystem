using System;
using System.Collections.Generic;
using System.Text;

namespace UberEntityComponentSystem
{
    /// <summary>
    /// stores entities in a scope
    /// </summary>
    public class Pool
    {
        #region Entities

        protected List<Entity> entities = new List<Entity>();

        public Entity newEntity {
            get {
                Entity e = Factory.Get<Entity>();
                e.setPool(this);
                entities.Add(e);
                return e;
            }
        }

        public Pool remove(params Entity[] entities)
        {
            foreach(Entity entity in entities)
            {
                this.entities.Remove(entity);
            }
            return this;
        }

        public int Count { get { return entities.Count; } }

        /// <summary>
        /// True if all provided entities are in the Pool
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool has(params Entity[] entities)
        {
            if(entities.Length == 1)
                return this.entities.Contains(entities[0]);

            int count = 0;
            foreach(Entity entity in entities)
            {
                if (this.entities.Contains(entity)) count++;
            }
            return count == entities.Length && count != 0;
        }

        #endregion //Entities

        #region Groups
        
        protected List<Group> groups = new List<Group>();

        protected void addToGroups(Handle h)
        {
            foreach (Group g in groups)
            {
                g.fit(h);
            }
        }

        public Group getGroup(Signature sig)
        {
            foreach (Group g in groups)
            {
                if (g.signature == sig)
                {
                    return g;
                }
            }
            Group group = new Group(this, sig);
            groups.Add(group);
            return group;
        }

        #endregion //Groups

        #region Indexing

        public Handle this[int key]
        {
            get
            {
                return entities[key].handle;
            }
        }

        internal void _updateEntity(Entity entity)
        {
            foreach (Group g in groups)
            {
                g.fit(entity.handle);
            }
        }

        #endregion //Indexing
    }
}
