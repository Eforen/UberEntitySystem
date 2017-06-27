using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntitySystemCore
{
    public interface IObjectFactory<T> where T : class, new()
    {
        /// <summary>
        /// Creates a new instance of the type specified.
        /// </summary>
        /// <returns>New instance of type</returns>
        T CreateNew();

        /// <summary>
        /// Returns the same object passed in with all the instance specific stuff cleaned out.
        /// </summary>
        /// <param name="obj">the instance of the object that needs to be cleaned</param>
        /// <returns>The same object that was passed in for cleaning</returns>
        T CleanForReuse(T obj);
    }
}
