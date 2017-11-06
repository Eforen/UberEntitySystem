using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UberEntityComponentSystem
{
    public class Systems
    {
        private class SystemSortNode
        {
            public Type type;
            public PoolSystem Item { get; set; }
            public SystemSortNode[] Dependencies { get; set; }

            public SystemSortNode(PoolSystem item, params SystemSortNode[] dependencies)
            {
                Item = item;
                type = item.GetType();
                Dependencies = dependencies;
            }

            public SystemSortNode changeDependencyMappings(params SystemSortNode[] dependencies)
            {
                Dependencies = dependencies;
                return this;
            }
        }

        private Queue<PoolSystem> unNodedSystems = new Queue<PoolSystem>();

        private List<SystemSortNode> nodes = new List<SystemSortNode>();

        private void nodeup()
        {
            bool newNodes = false;
            while (unNodedSystems.Count > 0)
            {
                newNodes = true;
                SystemSortNode node = new SystemSortNode(unNodedSystems.Dequeue());

                List<SystemSortNode> depends = new List<SystemSortNode>();

                //Assign any new refs to this node and map its dependences to nodes
                foreach(SystemSortNode n in nodes)
                {
                    //Check if this is a dependency of that
                    if (n.Item.dependencies != null)
                        foreach (Type dependencey in n.Item.dependencies)
                        {
                            if (dependencey == node.type)
                            {
                                List<SystemSortNode> tempDep = new List<SystemSortNode>(n.Dependencies); // Make that's dependency list malleable
                                tempDep.Capacity = n.Dependencies.Length + 1; // Expand the Capacity of that dependency list
                                tempDep.Add(node); // Add this node to that

                                n.Dependencies = tempDep.ToArray(); //Set new Dependencies
                                break; //Quit looking because each node can only be depended once
                            }
                        }

                    //check if that is a dependency of this
                    if(node.Item.dependencies != null)
                        foreach (Type dependencey in node.Item.dependencies)
                        {
                            if (dependencey == n.type)
                            {
                                depends.Add(n); // Add that node to this
                            
                                break; //Quit looking because each node can only be depended once
                            }
                        }
                }

                //output final dependency array
                node.Dependencies = depends.ToArray();

                //add new node to the pool of nodes
                nodes.Add(node);
            }

            if (newNodes)
            {
                //Rework Topological Sort
                SystemSortNode[] arr = nodes.ToArray();
                systems = TopologicalSort.Sort(nodes, x => x.Dependencies).ToArray();
            }
        }

        public Pool pool { get; private set; }


        public Systems(Pool pool)
        {
            this.pool = pool;
        }

        public Systems add(params PoolSystem[] systems)
        {
            foreach (PoolSystem sys in systems)
            {
                sys.pool = pool;
                sys.Setup();

                unNodedSystems.Enqueue(sys);
            }

            return this;
        }

        SystemSortNode[] systems = null;

        public void Execute()
        {
            nodeup();

            if(systems != null)
            {
                foreach (SystemSortNode sys in systems)
                {
                    sys.Item.Execute();
                }
            }
        }
    }
}
