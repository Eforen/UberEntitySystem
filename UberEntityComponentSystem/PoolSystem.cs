using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public abstract class PoolSystem
    {
        public Pool pool { get; internal set; }

        public virtual Type[] dependencies { get; private set; }

        public abstract void Execute();
        public virtual void Setup() { }
    }
}
