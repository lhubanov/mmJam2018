using System;
using UnityEngine;

using ProceduralGeneration.Node;

namespace ProceduralGeneration.Map
{
    public class Edge
    {
        // Polygons/tiles connected by the Delaunay edge
        public Center D0, D1;

        public Edge()
        {
            D0 = null;
            D1 = null;
        }

        public Edge (Center d0, Center d1)
        {
            D0 = d0;
            D1 = d1;

            AddUndirectedEdge(D0, D1);
        }

        private void AddUndirectedEdge(Center from, Center to)
        {
            from.Neighbours.Add(to);
            to.Neighbours.Add(from);

            from.Edges.Add(this);
            to.Edges.Add(this);
        }

        // TODO: Test this (I'm not sure if indexless edges won't cause issues with hashset storage)

        //public override int GetHashCode()
        //{
        //    return Convert.ToInt32(Index);
        //}

        //public override bool Equals(object obj)
        //{
        //    return (Index == (obj as Edge).Index);
        //}
    }
}
