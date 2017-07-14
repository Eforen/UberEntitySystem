using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UberEntityComponentSystem
{
    public static class Factory
    {
        private static Dictionary<Type, Queue<object>> caches = new Dictionary<Type, Queue<object>>();

        #region Helper Methods

        public static void RegisterFactory<T, V>()
            where T : ObjectFactory<V>, new()
            where V : class, new()
        {
            throw new NotImplementedException();
        }

        public static bool IsRegistered<V>()
        {
            throw new NotImplementedException();
        }

        public static bool IsRegistered(Type type)
        {
            throw new NotImplementedException();
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
            if (caches.ContainsKey(typeof(V)) &&
                caches[typeof(V)].Count > 0)
                return caches[typeof(V)].Dequeue() as V;

            return new V();
        }

        public static object Get(Type type)
        {
            if (caches.ContainsKey(type) &&
                caches[type].Count > 0)
                return caches[type].Dequeue();

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
            return target;
        }
        
        public static void Cache<V>(params V[] targets) where V : class, new()
        {
            if (targets.Count() == 0) return;

            if (caches.ContainsKey(typeof(V)) == false) //There is currently no cache for this type of obj
                caches.Add(typeof(V), new Queue<object>()); //Create cache for this type of OBJ

            foreach (V value in targets)
            {
                caches[typeof(V)].Enqueue(value);
            }

            //TODO: Profile with more types to see which is faster
            /*
            try
            {
                _counts[typeof(V)]++;
            }
            catch (KeyNotFoundException)
            {
                _counts.Add(typeof(V), 1);
            }
            */

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
            if (caches.ContainsKey(typeof(V)) == false)
                return 0;
            return caches[typeof(V)].Count;

            //TODO: Profile with more types to see which is faster
            /*
            try
            {
                return _counts[typeof(V)];
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }
            */
        }

        public static int Count(params Type[] types)
        {
            int count = 0; //Setup Count Storage
            foreach (Type type in types) // Loop through the types
                if (caches.ContainsKey(type)) //Cache has the requested type setup
                    count += caches[type].Count; //The count of Cache for the object type
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
            if (caches.ContainsKey(typeof(V))) //Cache has the requested type setup
                caches[typeof(V)].Clear();
        }

        /// <summary>
        /// Clears the Cache for type.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        public static void Reset(Type type)
        {
            if (caches.ContainsKey(type)) //Cache has the requested type setup
                caches[type].Clear();
        }
        #endregion // Debug
    }
}
