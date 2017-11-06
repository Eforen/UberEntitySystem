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
            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();


            // Loop through items and mark things that depend on this 
            foreach (var item in source)
            {
                Visit(item, getDependencies, sorted, visited);
            }

            return sorted;
        }

        public static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[item] = true;

                var dependencies = getDependencies(item);
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        Visit(dependency, getDependencies, sorted, visited);
                    }
                }

                visited[item] = false;
                sorted.Add(item);
            }
        }
    }
}
