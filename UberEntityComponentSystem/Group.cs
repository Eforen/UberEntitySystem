using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class Group
    {
        private List<Handle> entities = new List<Handle>();
        public List<Handle> replaced { get; private set; } = new List<Handle>();
        public List<Handle> added { get; private set; } = new List<Handle>();
        public List<Handle> removed { get; private set; } = new List<Handle>();

        public Pool pool { get; private set; }

        public Group(Pool pool, Signature sig)
        {
            this.pool = pool;
            this.signature = sig;
            for (int i = 0; i<pool.Count; i++)
            {
                fit(pool[i]);
            }
        }

        public Signature signature { get; private set; }

        public void fit(Handle h)
        {
            if (entities.Contains(h))
            {
                //Entity is already in the group
                if(signature != h.entity)
                {
                    //Entity does not match the group anymore
                    entities.Remove(h);
                    if (added.Contains(h)) added.Remove(h);
                    else removed.Add(h);
                }
                else
                {
                    //Entity does still match check for replace
                    //Todo: Add code to check for replace
                    foreach(Type t in h.entity.replaced)
                    {
                        if (signature.includes(t))
                        {
                            replaced.Add(h);
                            break;
                        }
                    }
                }
            }
            else
            {
                //Entity is not in the group
                if (signature == h.entity)
                {
                    //Entity matches the group
                    entities.Add(h);
                    if (removed.Contains(h)) removed.Remove(h);
                    else added.Add(h);
                }
            }
        }

        public int count {
            get
            {
                return entities.Count;
            }
        }

        public bool contains(Handle h)
        {
            return entities.Contains(h);
        }

        public bool contains(Entity e)
        {
            return entities.Contains(e.handle);
        }

        public Handle this[int key]
        {
            get
            {
                return entities[key];
            }
        }

        internal void increment()
        {
            removed.Clear();
            added.Clear();
            replaced.Clear();
        }
    }
}
