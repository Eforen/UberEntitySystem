using System;

namespace UberEntityComponentSystem
{
    public abstract class ObjectFactory<T> : ObjectFactoryBase where T : class, new()
    {
        /// <summary>
        /// Creates a new instance of the type specified.
        /// </summary>
        /// <returns>New instance of type</returns>
        public abstract T CreateNew();
        public override object ObjCreateNew()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the same object passed in with all the instance specific stuff cleaned out.
        /// </summary>
        /// <param name="obj">the instance of the object that needs to be cleaned</param>
        /// <returns>The same object that was passed in for cleaning</returns>
        public abstract T CleanForReuse(T obj);
        public override object ObjCleanForReuse(object obj)
        {
            throw new NotImplementedException();
        }

    }

    public abstract class ObjectFactoryBase
    {
        /// <summary>
        /// Creates a new instance of the type specified.
        /// </summary>
        /// <returns>New instance of type</returns>
        public abstract object ObjCreateNew();

        /// <summary>
        /// Returns the same object passed in with all the instance specific stuff cleaned out.
        /// </summary>
        /// <param name="obj">the instance of the object that needs to be cleaned</param>
        /// <returns>The same object that was passed in for cleaning</returns>
        public abstract object ObjCleanForReuse(object obj);
    }
}