using UnityEngine;

namespace ProceduralGeneration.Node
{
    public interface INode
    {
        uint Index { get; set; }
        Vector2 Position { get; set; }
    }
}
