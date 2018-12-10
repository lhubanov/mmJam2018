using UnityEngine;

namespace ProceduralGeneration.Node
{
    public interface INode
    {
        int Index { get; set; }
        Vector2 Position { get; set; }
    }
}
