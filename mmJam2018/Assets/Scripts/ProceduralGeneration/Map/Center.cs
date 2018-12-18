using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Node;
using ProceduralGeneration.Biome;

namespace ProceduralGeneration.Map
{
    public class Center : INode
    {
        private int index;
        private Vector2 position;

        private BiomeFactory BiomeFactory;

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

        // FIXME:   Review the accessors of all these (surely biomes need not be public)

        public bool Water;
        public bool Ocean;

        public bool Coast;
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
        //         Review if its necessary to be able to set all these actually!
        public Center(bool water, 
                    bool ocean, 
                    bool coast, 
                    Biome.Biome biome, 
                    double elevation, 
                    double moisture, 
                    int ind, 
                    Vector2 pos,
                    TileLookup tileLookup,
                    BiomeFactory biomeFactory)
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

            BiomeFactory = biomeFactory;
        }


        public bool RemoveEdge(Edge edge)
        {
            if(edge == null) {
                return false;
            }

            edges.Remove(edge);
            return true;
        }

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
                Biome = BiomeFactory.CreateBiome(BiomeType.OceanBiome);
                Elevation = 0;
            }
            else
            {
                if(hasLandNeighbours && hasOceanNeighbours)
                {
                    Biome = BiomeFactory.CreateBiome(BiomeType.BeachBiome);
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
        }

        // i.e. are all neighbours oceans
        private bool IsStrayIslandTile()
        {
            bool val = true;
            foreach(Center center in centers) {
                val &= center.Ocean;
            }

            return val;
        }

        public void StrayIslandTilePostProcess(System.Random seedBasedRng, TileLookup lookup)
        {
            if (IsStrayIslandTile()) {
                Biome = BiomeFactory.CreateBiome(BiomeType.OceanBiome);
                Elevation = 0;

                Biome.SpawnSprite(this);
            }
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

        public void SetBiomeBasedOnElevation()
        {
            BiomeFactory.CreateBiomeFromElevation(Elevation);
        }

        public void SpawnMembers()
        {
            Biome.SpawnMembers(this);
        }

        public void SpawnSprite()
        {
            if(Biome != null) { 
                Biome.SpawnSprite(this);
            }
        }      
    }
}
