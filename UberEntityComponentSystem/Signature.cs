using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    [Factory(typeof(SignatureFactory), typeof(Signature))]
    public class Signature
    {
        private static readonly Type TAG_TYPE = typeof(ITag);
        private static readonly Type COMP_TYPE = typeof(Component);
        private Func<Entity, bool> filter;
        private List<Type> tags;
        private List<Type> components;
        public int phase;

        public void clear()
        {
            phase++;
            filter = null;
            tags.Clear();
            components.Clear();
        }

        public Signature()
        {
            filter = null;
            tags = new List<Type>(0);
            components = new List<Type>(0);
        }

        public Signature(params Type[] types)
        {
            filter = null;
            setTypes(types);
        }

        public Signature(Func<Entity, bool> filter, params Type[] types)
        {
            this.filter = filter;

            setTypes(types);
        }

        private void setTypes(params Type[] types)
        {
            if (tags == null) tags = new List<Type>(types.Length / 2);
            if (components == null) components = new List<Type>(types.Length / 2);
            // List<Type> tags = new List<Type>(types.Length);
            // List<Type> components = new List<Type>(types.Length);

            foreach (Type t in types)
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

            //this.tags = tags;
            //this.components = components.ToArray();
        }

        public static Signature Get(params Type[] types)
        {
            Signature sig = Factory.Get<Signature>();
            sig.setTypes(types);
            return sig;
        }

        public static Signature Get(Func<Entity, bool> filter, params Type[] types)
        {
            Signature sig = Factory.Get<Signature>();
            sig.filter = filter;
            sig.setTypes(types);
            return sig;
        }

        #region Compair Signature and Entity
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
        #endregion //compare

        #region Compair Signatures

        public static bool operator ==(Signature a, Signature b)
        {
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            //Require exact match
            if (a.tags.Count != b.tags.Count || a.components.Count != b.components.Count) return false;

            foreach (Type tag in a.tags)
            {
                //Check all tags
                if (b.tags.Contains(tag) == false) return false;
            }

            foreach (Type comp in a.components)
            {
                //check all components
                if (b.components.Contains(comp) == false) return false;
            }

            //check if using the same filter
            return a.filter == b.filter;
        }

        public static bool operator !=(Signature a, Signature b)
        {
            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return true;
            }

            foreach (Type tag in a.tags)
            {
                if (b.tags.Contains(tag) == false) return true;
            }

            foreach (Type comp in a.components)
            {
                if (b.components.Contains(comp) == false) return true;
            }
            
            return a.filter != b.filter;
        }

        //private bool signatureEquals
        #endregion //compare

        public override bool Equals(object obj)
        {
            if ((obj as Entity) != null)
            {
                return this == obj as Entity;
            }
            if ((obj as Signature) != (Signature)null) //the cast of Null is for the compiler to tell it which operator method to use.
            {
                return this == obj as Signature;
            }
            return base.Equals(obj);
        }

        public bool includes<T>(T c)
        {
            return tags.Contains(c.GetType()) || components.Contains(c.GetType());
        }

        public bool includes(Type t)
        {
            return tags.Contains(t) || components.Contains(t);
        }
    }
}
