using System.Collections.Generic;
using System.Linq;

using ProceduralGeneration.Node;

namespace ProceduralGeneration.Graph
{
    public enum GRAPH_TRAVERSAL_METHOD
    {
        BFS = 0,
        DFS = 1
    }
    public class Graph
    {
        private INode root;

        public Graph() : this(null) { }
        public Graph(INode Root)
        {
            root = Root;
        }

        public INode Root
        {
            get { return root; }
            set { root = value; }
        }



        // Potential additions/ general useful graph methods

        private delegate INode TraversalGetter();
        private delegate void TraversalSetter(INode center);

        // This seems a bit over-engineered for what I intended to do, review later
        protected HashSet<INode> TraverseGraph(INode root, GRAPH_TRAVERSAL_METHOD method)
        {
            IEnumerable<INode> collection;
            TraversalGetter traversalGetter;
            TraversalSetter traversalSetter;

            if (method == GRAPH_TRAVERSAL_METHOD.BFS)
            {
                collection = new Queue<INode>();
                traversalGetter = new TraversalGetter((collection as Queue<INode>).Dequeue);
                traversalSetter = new TraversalSetter((collection as Queue<INode>).Enqueue);
            }
            else
            {
                collection = new Stack<INode>();
                traversalGetter = new TraversalGetter((collection as Stack<INode>).Pop);
                traversalSetter = new TraversalSetter((collection as Stack<INode>).Push);
            }

            var visited = new HashSet<INode>();
            traversalSetter(root);

            while (collection.Count() > 0)
            {
                INode node = traversalGetter();

                if (visited.Contains(node))
                {
                    continue;
                }

                visited.Add(node);
                foreach (INode neighbour in node.Neighbours)
                {
                    if (!visited.Contains(neighbour))
                    {
                        traversalSetter(neighbour);
                    }
                }
            }

            return visited;
        }
    }
}
