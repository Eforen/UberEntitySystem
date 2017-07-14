using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class FactoryAttribute : Attribute
    {

        /// <summary>
        /// The factory we want to declare makes and cleans this type.
        /// </summary>
        public Type factory;

        /// <summary>
        /// The type this is.
        /// </summary>
        public Type target;

        public FactoryAttribute(Type factory, Type target)
        {
            this.factory = factory;
            this.target = target;
        }
    }
}
