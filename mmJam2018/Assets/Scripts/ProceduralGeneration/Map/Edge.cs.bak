using System;
using UnityEngine;

using ProceduralGeneration.Node;

namespace ProceduralGeneration.Map
{
    public class Edge : INode
    {
        public int Index { get; set; }
        public Vector2 Position { get; set; }

        // Polygons/tiles connected by the Delaunay edge
        public Center D0, D1;

        // Corners connected by the Voronoi edge
        //public Corner V0, V1;

        public Edge()
        {
            Index = 0;

            D0 = null;
            D1 = null;
        }

        public Edge (Center d0, Center d1, int index)
        {
            Index = index;

            D0 = d0;
            D1 = d1;

            AddUndirectedEdge(D0, D1);
        }

        private void AddUndirectedEdge(Center from, Center to)
        {
            from.Centers.Add(to);
            to.Centers.Add(from);

            from.Edges.Add(this);
            to.Edges.Add(this);
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(Index);
        }

        public override bool Equals(object obj)
        {
            return (Index == (obj as Edge).Index);
        }
    }
}
