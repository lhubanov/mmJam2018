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
        private Vector2 position;
        private BiomeFactory BiomeFactory;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool Ocean { get; private set; }
        public bool Coast { get; private set; }

        public Biome.IBiome Biome { get; private set; }

        // The only two publicly settable, as there's an elevetion calculation
        // happening on a Map-level and the Water-PerlinNoise calculation uses several
        // editor-settable values, available to map, but not to center. 
        // Leaving as is for now- there is a neater way to do it without exposing these. 
        // Property should at least introduce some basic decoupling, for now, between centers and map.
        public bool     Water       { get; set; }
        public double   Elevation   { get; set; }


        private TileLookup lookup;

        private HashSet<INode> neighbours;
        public HashSet<INode> Neighbours
        {
            get
            {
                if (neighbours == null) {
                    neighbours = new HashSet<INode>();
                }

                return neighbours;
            }

            set
            {
                neighbours = value;
            }
        }

        private HashSet<Edge> edges;
        public HashSet<Edge> Edges
        {
            get
            {
                if (edges == null) {
                    edges = new HashSet<Edge>();
                }

                return edges;
            }

            set
            {
                edges = value;
            }
        }

        public Center(bool water, 
                    bool ocean, 
                    bool coast, 
                    Biome.Biome biome, 
                    double elevation,
                    Vector2 pos,
                    TileLookup tileLookup,
                    BiomeFactory biomeFactory)
        {
            Water = water;
            Ocean = ocean;
            Coast = coast;
            Biome = biome;
            Elevation = elevation;

            lookup = tileLookup;

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
            foreach(Edge edge in edges)
            {
                if(edge.D0 == center || edge.D1 == center) {
                    return edge;
                }
            }

            return null;
        }

        // Assigns first-pass biome/conditions
        public void Initialize()
        {
            bool hasOceanNeighbours = HasOceanNeighbours();
            bool hasLandNeighbours = HasLandNeighbours();

            if ((Water && hasOceanNeighbours) || IsStrayIslandTile())
            {
                Ocean = true;
                Biome = BiomeFactory.CreateBiome(BiomeType.OceanBiome);
                Elevation = 0;
            }
            else
            {
                Water = false;

                if (hasLandNeighbours && hasOceanNeighbours) {
                    Biome = BiomeFactory.CreateBiome(BiomeType.BeachBiome);
                    Elevation = 1;
                    Coast = true;
                } else {
                    Elevation = 0;
                    Coast = false;
                }
            }
        }


        // Note:    The foreach loops on "Neighbours", below
        //          do an internal explicit conversion, which, currently, is safe,
        //          as there is only one type of INode - Center. If ever different
        //          INodes are introduced, this kind of operation will start throwing
        //          exceptions. However, currently this is quicker than doing a soft cast
        //          on each INode, so I'm leaving it as is for the time being.

        // IsStrayIslandTile if all neighbours are of ocean type
        private bool IsStrayIslandTile()
        {
            bool val = true;
            foreach(Center center in Neighbours) {
                val &= center.Ocean;
            }

            return val;
        }

        private bool HasLandNeighbours()
        {
            foreach(Center center in Neighbours) {
                if(!center.Ocean || !center.Water) {
                    return true;
                }
            }

            return false;
        }

        private bool HasOceanNeighbours()
        {
            foreach(Center center in Neighbours) {
                if (center.Ocean) {
                    return true;
                }
            }

            return false;
        }

        public bool HasNeighbourWithSpawnedMembers(Type biomeType)
        {
            foreach(Center c in Neighbours)
            {
                if((c.Biome.GetType() == biomeType) && c.Biome.HasSpawned) {
                    return true;
                }
            }

            return false;
        }

        public void SetBiomeBasedOnElevation()
        { 
            Biome = BiomeFactory.CreateBiomeFromElevation(Elevation);
        }

        public void SetToMarshTileIfWater()
        {
            if (Water)
            {
                Biome = BiomeFactory.CreateBiome(BiomeType.MarshBiome);
                Elevation = 0;
            }
        }

        public void SetToOceanTile()
        {
            Water = true;
            Ocean = true;
        }

        public void SpawnMembers()
        {
            Biome.SpawnMembers(this);
        }

        public void SpawnSpriteWrtBiome()
        {
            if (Biome == null) {
                return;
            }

            Biome.SpawnSprite(this);
        }      
    }
}
