using System;
using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Node;
using ProceduralGeneration.Biome;

namespace ProceduralGeneration.Map
{
    //Center.neighbours = adjacent polygons/tiles
    //Center.borders = bordering edges
    //Center.corners = polygon corners

    //Edge.d0/d1 = polygons connected by the Delaunay edge
    //Edge.v0/v1 = corners connected by the Voronoi edge

    //Corner.touches = set of polygons touching this corner
    //Corner.portrudes = set of edges touching the corner
    //Corner.adjacent = set of corners connected to this one

    public class Center : INode
    {
        private int index;
        private Vector2 position;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        // FIXME:   Shouldn't be public probably, 
        //          one ok alternative would be to
        //          add specific setters (e.g. SetToOcean() or IsOcean(bool b) etc.)

        public bool Water;
        public bool Ocean;

        public bool Coast;

        //bool border;
        public Biome.IBiome Biome;
        public double Elevation;
        public double Moisture;

        private TileLookup lookup;

        // Note: these could be HashSets?
        private HashSet<Edge> edges;
        private HashSet<Center> centers;

        //private List<Corner> corners;
        //public List<Corner> Corners
        //{
        //    get
        //    {
        //        if(corners == null) {
        //            corners = new List<Corner>();
        //        }
        //        return corners;
        //    }

        //    set => corners = value;
        //}


        public HashSet<Edge> Edges
        {
            get
            {
                if (edges == null)
                {
                    edges = new HashSet<Edge>();
                }
                return edges;
            }

            set {
                edges = value;
            }
        }

        public HashSet<Center> Centers
        {
            get
            {
                if (centers == null)
                {
                    centers = new HashSet<Center>();
                }

                return centers;
            }

            set {
                centers = value;
            }
        }


        //  FIXME: I hate this constructor now
        public Center(bool water, 
                    bool ocean, 
                    bool coast, 
                    Biome.Biome biome, 
                    double elevation, 
                    double moisture, 
                    int ind, 
                    Vector2 pos,
                    TileLookup tileLookup)
        {
            Water = water;
            Ocean = ocean;
            Coast = coast;
            Biome = biome;
            Elevation = elevation;
            Moisture = moisture;

            lookup = tileLookup;

            index = ind;
            position = pos;
        }


        public bool RemoveEdge(Edge edge)
        {
            if(edge == null) {
                return false;
            }

            edges.Remove(edge);
            return true;
        }

        //bool RemoveCorner(Corner corner)
        //{
        //    if (corner == null) {
        //        return false;
        //    }

        //    corners.Remove(corner);
        //    return true;
        //}

        public Edge GetEdgeWith(Center center)
        {
            foreach(Edge edge in edges) {
                if(edge.D0 == center || edge.D1 == center) {
                    return edge;
                }
            }

            return null;
        }

        public void Initialize(System.Random seedBasedRng)
        {
            // Assign biome here
            bool hasOceanNeighbours = HasOceanNeighbours();
            bool hasLandNeighbours = HasLandNeighbours();

            if (Water)
            {
                if (hasOceanNeighbours) {
                    Ocean = true;
                }
            }

            if (Water && Ocean || IsStrayIslandTile())
            {
                Biome = new Biome.OceanBiome(seedBasedRng, lookup);
                Elevation = 0;
            }
            else
            {
                if(hasLandNeighbours && hasOceanNeighbours)
                {
                    Biome = new Biome.BeachBiome(seedBasedRng, lookup);
                    Elevation = 1;

                    Water = false;
                    Coast = true;
                }
                else
                {
                    Water = false;
                    Coast = false;

                    Elevation = 0;
                }
            }

            // TODO:    Get & instantiate tile prefab here
            //          Note: Potentially, this could be done at some
            //          post-processing stage, depending on how many iterations
            //          through the graph need to be made (based on initial Biome assignments)
        }

        // i.e. are all neighbours oceans
        private bool IsStrayIslandTile()
        {
            bool val = true;
            foreach(Center center in centers) {
                val &= (center.Water && center.Ocean);
            }

            return val;
        }

        private bool HasLandNeighbours()
        {
            foreach(Center center in centers) {
                if(!center.Ocean || !center.Water) {
                    return true;
                }
            }

            return false;
        }

        private bool HasOceanNeighbours()
        {
            foreach(Center center in centers) {
                if (center.Ocean) {
                    return true;
                }
            }

            return false;
        }

        // FIXME: The fact the name of this function is weird/long
        //        probably just means it needs to be refactored/redesigned.
        public bool HasNeighbourOfExBiomeType(Type biomeType)
        {
            foreach(Center c in centers)
            {
                if((c.Biome.GetType() == biomeType) && c.Biome.HasSpawned) {
                    return true;
                }
            }

            return false;
        }

        public void SpawnMembers()
        {
            Biome.SpawnMembers(this);
        }

        public void SpawnSprite()
        {
            Biome.SpawnSprite(this);
        }
        
        // Potential member methods

        //void MakeBorder();
        //bool IsInsideBoundingBox(int width, int height);
        //bool Contains(Vec2 p_pos);
        //pair<Vec2, Vec2> GetBoundingBox();
        //void SortCorners();
        //bool GoesBefore(Vec2 p_a, Vec2 p_b);
    }
}
