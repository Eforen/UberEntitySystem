using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UberEntitySystemCore
{
    public static class Factory
    {
        private static Dictionary<Type, object> factories = new Dictionary<Type, object>();

        private static Dictionary<Type, Queue<object>> caches = new Dictionary<Type, Queue<object>>();

        #region Helper Methods
        
        /*Use if Attr Constructor does not work. */
        private static bool initialized; //If the factories have been scanned yet.

        private static void checkSetup()
        {
            if (initialized) return;
            initialized = true;

            MethodInfo registerMethod = typeof(Factory).GetMethod("RegisterFactory"); //get reference to the Method itself

            //Scan Everything for factories
            //Get all active assemblies and loop each one.
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                //Get and loop through all Types in current assembly
                foreach (Type type in assembly.GetTypes())
                {
                    //Get list of any/all Factory Attributes on the type
                    var attribs = type.GetCustomAttributes(typeof(FactoryAttribute), false);
                    if (attribs == null || attribs.Length == 0) //No Factory attribute found
                        continue; //Skip the rest of this loop and go to next type.

                    //Clearly has a Factory so get the Factory marker and register it.
                    FactoryAttribute fattr = attribs[0] as FactoryAttribute;

                    //Effectively do Factory.RegisterFactory<factory, target>() using reflection
                    registerMethod
                       .MakeGenericMethod(fattr.factory, fattr.target) //Make generic version of it using our types
                       .Invoke(null, null); //finally invoke (Call) our generic with a static context
                }
            }
        }

        public static void RegisterFactory<T, V>()
            where T : ObjectFactory<V>, new()
            where V : class, new()
        {
            if (IsRegistered<V>() == false)
            {
                factories.Add(typeof(V), new T());
            }
        }

        public static bool IsRegistered<V>()
        {
            checkSetup();
            return factories.ContainsKey(typeof(V));
        }

        public static bool IsRegistered(Type type)
        {
            checkSetup();
            return factories.ContainsKey(type);
        }
        #endregion // Helper Methods

        #region Core Functionality
        /// <summary>
        /// Gets a cached object of type or returns a new() of it if none are available.
        /// </summary>
        /// <typeparam name="V">Type of object to get</typeparam>
        /// <returns></returns>
        public static V Get<V>() where V : class, new()
        {
            if (caches.ContainsKey(typeof(V))) //Cache has the  requested type setup
            {
                if(caches[typeof(V)].Count > 0) //Cache contains at least one cleaned copy of the object type
                {
                    return caches[typeof(V)].Dequeue() as V; //Return the pre-cleaned object
                }
            }

            //No prepared objects available for what ever reason so we need to instantiate it.

            if (IsRegistered<V>()) //The Type has a factory registered so it is safe to use it
                return (factories[typeof(V)] as ObjectFactory<V>).CreateNew(); //Return new object out of factory

            //Must not have factory so run standard new
            return new V();
        }

        public static object Get(Type type)
        {
            if (caches.ContainsKey(type)) //Cache has the  requested type setup
            {
                if (caches[type].Count > 0) //Cache contains at least one cleaned copy of the object type
                {
                    return caches[type].Dequeue(); //Return the pre-cleaned object
                }
            }

            if (IsRegistered(type)) //The Type has a factory registered so it is safe to use it
                return (factories[type] as ObjectFactoryBase).ObjCreateNew(); //Return new object out of factory

            try
            {
                return type.GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static V Clean<V>(V target) where V : class, new()
        {
            if (IsRegistered<V>()) //The Type has a factory registered so it is safe to use it
                return (factories[typeof(V)] as ObjectFactory<V>).CleanForReuse(target); //Clean the obj using its factory

            //TODO: Autonomous object cleaning using reflection
            PropertyInfo[] props = target.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object test = prop.GetValue(target, null);
            }
            return target;
        }

        public static void Cache<V>(params V[] targets) where V : class, new()
        {
            if (targets.Count() == 0) return;
            if (caches.ContainsKey(typeof(V)) == false) //There is currently no cache for this type of obj
                caches.Add(typeof(V), new Queue<object>()); //Create cache for this type of OBJ
            
            foreach(V value in targets)
            {
                caches[typeof(V)].Enqueue(Clean<V>(value)); //Clean the object and then put it into the cache
            }
        }
        #endregion //Core Functionality

        #region Debug
        /// <summary>
        /// Returns the amount of target objects currently cached
        /// </summary>
        /// <typeparam name="V">Target Object Type</typeparam>
        /// <returns>Amount of objects cached</returns>
        public static int Count<V>()
        {
            if (caches.ContainsKey(typeof(V))) //Cache has the  requested type setup
            {
                return caches[typeof(V)].Count; //The count of Cache for the object type
            }
            return 0;
        }

        public static int Count(params Type[] types)
        {
            int count = 0;
            foreach(Type type in types)
            {
                if (caches.ContainsKey(type)) //Cache has the  requested type setup
                {
                    count += caches[type].Count; //The count of Cache for the object type
                }
            }
            return count;

        }

        /// <summary>
        /// Clears all Caches.
        /// </summary>
        public static void Reset()
        {
            caches.Clear();
        }

        /// <summary>
        /// Clears the Cache for type.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        public static void Reset<V>()
        {
            try
            {
                caches[typeof(V)].Clear();
            }
            catch (Exception) { }
        }
        #endregion // Debug
    }
}
