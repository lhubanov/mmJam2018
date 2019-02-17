using UnityEngine;
using System.Collections.Generic;

namespace ProceduralGeneration.Node
{
    public interface INode
    {
        Vector2 Position { get; set; }
        HashSet<INode> Neighbours { get; set; }
    }
}
