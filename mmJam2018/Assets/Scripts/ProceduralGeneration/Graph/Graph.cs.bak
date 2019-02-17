using ProceduralGeneration.Node;

namespace ProceduralGeneration.Graph
{
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

        // Potential additions
        // public abstract HashSet<INode> Traverse (INode root, HashSet<INode> rootNeighbours);
        // TraverseDFS / TraverseBFS?

        // A bit of a design challenge, as INodes do
        // not contain a neighbours property; Probably should and then be somehow overwriten as Centers?
        //public HashSet<INode> TraverseDFS(INode root)
        //{
        //    HashSet<INode> visited = new HashSet<INode>();
        //    Stack<INode> stack = new Stack<INode>();

        //    stack.Push(root);

        //    while(stack.Count > 0)
        //    {
        //        INode node = stack.Pop();
        //        if (visited.Contains(node)) {
        //            continue;
        //        }

        //        visited.Add(node);

        //        foreach(INode neighbour in node.)
        //    }

        //}

        //public HashSet<INode> TraverseBFS()
        //{

        //}


        // Potential additions/ general useful graph methods

        //private NodeList<T> nodeSet;

        //public Graph(NodeList<T> nodeSet)
        //{
        //    if(nodeSet == null) {
        //        this.nodeSet = new NodeList<T>();
        //    } else {
        //        this.nodeSet = nodeSet;
        //    }
        //}

        //public NodeList<T> Nodes
        //{
        //    get { return nodeSet; }
        //}

        //public int Count
        //{
        //    get { return nodeSet.Count; }
        //}

        //public void AddNode(GraphNode<T> node)
        //{
        //    nodeSet.Add(node);
        //}

        //public void AddNode(T value)
        //{
        //    nodeSet.Add(new GraphNode<T>(value));
        //}

        //public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        //{
        //    from.Neighbours.Add(to);
        //    from.Costs.Add(cost);
        //}

        //public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        //{
        //    from.Neighbours.Add(to);
        //    from.Costs.Add(cost);

        //    to.Neighbours.Add(from);
        //    to.Costs.Add(cost);
        //}

        //public bool Contains(T value)
        //{
        //    return nodeSet.FindByValue(value) != null;
        //}

        //public bool Remove(T value)
        //{
        //    // first remove the node from the nodeset
        //    GraphNode<T> nodeToRemove = (GraphNode<T>)nodeSet.FindByValue(value);
        //    if (nodeToRemove == null) { 
        //        // node wasn't found
        //        return false;
        //    }

        //    // otherwise, the node was found
        //    nodeSet.Remove(nodeToRemove);

        //    // enumerate through each node in the nodeSet, removing edges to this node
        //    foreach (GraphNode<T> node in nodeSet)
        //    {
        //        int index = node.Neighbours.IndexOf(nodeToRemove);
        //        if (index != -1)
        //        {
        //            // remove the reference to the node and associated cost
        //            node.Neighbours.RemoveAt(index);
        //            node.Costs.RemoveAt(index);
        //        }
        //    }

        //    return true;
        //}
    }
}
