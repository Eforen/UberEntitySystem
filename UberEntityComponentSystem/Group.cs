using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class Group
    {
        private List<Handle> entities = new List<Handle>();
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
            if (signature == h.entity && entities.Contains(h) == false) entities.Add(h);
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


    }
}
