using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UberEntityComponentSystem
{
    public static class Factory
    {
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
            throw new NotImplementedException();
        }

        public static object Get(Type type)
        {
            throw new NotImplementedException();
        }

        public static V Clean<V>(V target) where V : class, new()
        {
            throw new NotImplementedException();
        }

        public static void Cache<V>(params V[] targets) where V : class, new()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public static int Count(params Type[] types)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all Caches.
        /// </summary>
        public static void Reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears the Cache for type.
        /// </summary>
        /// <typeparam name="V"></typeparam>
        public static void Reset<V>()
        {
            throw new NotImplementedException();
        }
        #endregion // Debug
    }
}
