using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class Signature
    {
        private static readonly Type TAG_TYPE = typeof(ITag);
        private static readonly Type COMP_TYPE = typeof(Component);
        private Func<Entity, bool> filter;
        private Type[] tags;
        private Type[] components;
        public Signature(Func<Entity, bool> filter = null, params Type[] types)
        {
            List<Type> tags = new List<Type>(types.Length);
            List<Type> components = new List<Type>(types.Length);

            this.filter = filter;
            foreach(Type t in types)
            {
                if (TAG_TYPE.IsAssignableFrom(t))
                {
                    tags.Add(t);
                    continue;
                }
                if (COMP_TYPE.IsAssignableFrom(t))
                {
                    components.Add(t);
                    continue;
                }
            }

            this.tags = tags.ToArray();
            this.components = components.ToArray();
        }

        public static bool operator ==(Entity a, Signature b)
        {
            return b == a;
        }

        public static bool operator !=(Entity a, Signature b)
        {
            return b != a;
        }

        public static bool operator ==(Signature a, Entity b)
        {
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            foreach (Type tag in a.tags)
            {
                if (b.hasTag(tag) == false) return false;
            }

            foreach (Type comp in a.components)
            {
                if (b.hasComponent(comp) == false) return false;
            }

            if (a.filter != null) return a.filter(b);
            return true;
        }

        public static bool operator !=(Signature a, Entity b)
        {
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return true;
            }

            foreach (Type tag in a.tags)
            {
                if (b.hasTag(tag) == false) return true;
            }

            foreach (Type comp in a.components)
            {
                if (b.hasComponent(comp) == false) return true;
            }

            if (a.filter != null) return a.filter(b) == false;
            return false;
        }
    }
}
