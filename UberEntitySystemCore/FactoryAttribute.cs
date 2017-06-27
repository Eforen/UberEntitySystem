using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace UberEntitySystemCore
{
    public class FactoryAttribute : Attribute
    {
        /// <summary>
        /// The type we want to declare this factory as.
        /// </summary>
        public Type factory;
        public Type target;

        public FactoryAttribute(Type factory, Type target)
        {
            //Store the vars incase we want them.
            this.factory = factory;
            this.target = target;
        }

        public void register()
        {
            //Effectively do Factory.RegisterFactory<factory, target>() using reflection
            typeof(Factory).GetMethod("RegisterFactory") //get reference to the Method itself
                .MakeGenericMethod(factory, target) //Make generic version of it using our types
                .Invoke(null, null); //finally invoke (Call) our generic with a static context
        }
    }
}
