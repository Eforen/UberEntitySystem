using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class TopologicalSort
    {
        /*
        public enum SortType
        {
            ObjRef,
            TypeRef
        }
        */
        private class SortWrapper<T>
        {

            public SortWrapper(T item, int dependencies)
            {
                this.item = item;
                this.Dependences = dependencies;
            }
            public T item;
            //public int Depents; // Things that depend on this
            public int Dependences; // Things that this depends on
        }

        public static IList<T> Sort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        {
            List<SortWrapper<T>> waiting = new List<SortWrapper<T>>();

            //Loop Items
            foreach (var i in source)
            {
                int wrapperDeps = 0;
                //Loop already added Wrappers
                foreach (var t in waiting)
                {
                    //Loop Items own dependencies
                    var deps = getDependencies(i);
                    foreach (var d in deps)
                    {
                        if (t.item.Equals(d))
                        {
                            //Target is a dependency of Item
                            wrapperDeps++;
                        }
                    }

                    //Loop Dependencies of added Wrappers
                    deps = getDependencies(t.item);
                    foreach (var d in deps)
                    {
                        if (i.Equals(d))
                        {
                            //Item is a dependency of Target
                            t.Dependences++;
                        }
                    }
                }

                waiting.Add(new SortWrapper<T>(i, wrapperDeps));
            }


            List<T> output = new List<T>();

            //Loop until their are no waiting elements
            while (waiting.Count > 0)
            {
                SortWrapper<T> targetToRemove = null;
                //Loop all waiting elements
                foreach (var target in waiting)
                {
                    //Find first target with no dependencies
                    if (target.Dependences == 0)
                    {
                        //Loop all remaining items
                        foreach (var remainer in waiting)
                        {
                            //Loop all dependencies of remaining items
                            var deps = getDependencies(remainer.item);
                            foreach (var d in deps)
                            {
                                if (d.Equals(target.item))
                                {
                                    remainer.Dependences--;
                                    break;
                                }
                            }
                        }
                        //Add the unwrapped item to the output
                        output.Add(target.item);
                        //Set item to remove
                        targetToRemove = target;
                        //exit loop to reset to the beginning of the remaining items.
                        break;
                    }
                }
                waiting.Remove(targetToRemove);
            }

            return output;
        }
    }
}
