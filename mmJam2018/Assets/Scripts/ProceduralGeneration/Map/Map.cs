using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Biome;
using ProceduralGeneration.Node;
using ProceduralGeneration.Map.MapSettings;

namespace ProceduralGeneration.Map
{
    public class Map : Graph.Graph
    {
        private float mapWidth;
        private float mapHeight;

        private readonly Vector2 mapBotLeft;
        private readonly Vector2 mapTopRight;

        private float tileSize;

        private float oceanThreshold;
        private float chanceIslandTileIsWater;

        private double elevationIncreaseRate;
        private int spawningIterations;

        private float noiseScale;
        private int maxNoiseOffset;

        private string rngSeed;
        private System.Random rng;
        private TileLookup tileLookup;

        private Transform parentGameObject;

        private Graph.GRAPH_TRAVERSAL_METHOD graphTraversalMethod;

        private BiomeFactory biomeFactory;
        private BiomeConditions conditions;


        public Map(MapSettingsContainer mapSettings)
        {
            mapBotLeft = mapSettings.MapBotLeft;
            mapTopRight = mapSettings.MapTopRight;

            oceanThreshold = mapSettings.OceanThreshold;
            chanceIslandTileIsWater = mapSettings.ChanceIslandTileIsWater;

            elevationIncreaseRate = mapSettings.ElevationIncreaseRate;

            spawningIterations = mapSettings.MemberSpawningIterations;

            noiseScale = mapSettings.NoiseScale;
            maxNoiseOffset = mapSettings.MaxNoiseOffset;

            tileLookup = mapSettings.TileLookup;
            parentGameObject = mapSettings.ParentGameObject;
            conditions = mapSettings.Conditions;

            tileSize = mapSettings.TileSize;
            graphTraversalMethod = mapSettings.GraphTraversalMethod;

            rngSeed = mapSettings.RngSeed;
            InitConstants();
        }

        private void InitConstants()
        {
            rng = new System.Random(rngSeed.GetHashCode());

            biomeFactory = new BiomeFactory(rng, tileLookup, conditions, parentGameObject);

            mapWidth = Mathf.Abs(mapTopRight.x - mapBotLeft.x);
            mapHeight = Mathf.Abs(mapTopRight.y - mapBotLeft.y);
        }

        private bool IsOnMapEdge(INode node)
        {
            return (node.Position.x == mapBotLeft.x || node.Position.x == mapTopRight.x);
        }

        private bool IsAroundMapEdge(Vector2 pos)
        {
            var xMaxThreshold = mapTopRight.x  - (mapWidth * (1 - oceanThreshold));
            var xMinThreshold = mapBotLeft.x + (mapWidth * (1 - oceanThreshold));

            return (pos.x > xMaxThreshold || pos.x < xMinThreshold);
        }

        public void Generate()
        {
            CreateDefaultNodes();
            HashSet<INode> centers = TraverseGraph(Root as Center, graphTraversalMethod);

            HashSet<Center> islandTiles = GenerateOcean(centers);
            GenerateIsland(islandTiles);

            HashSet<Center> coastalTiles = InitializeTiles(centers);
            GenerateMarsh(islandTiles);

            SetElevation(coastalTiles);
            AssignBiomes(islandTiles);

            SpawnSprites(centers);
            SpawnMembers(centers, spawningIterations);
        }

        // Creates map of tiles withe default values/settings
        private void CreateDefaultNodes()
        {
            List<List<Center>> nodesMap = new List<List<Center>>();
            Root = new Center(true, true, false, biomeFactory.CreateBiome(BiomeType.None) as Biome.Biome, 0, new Vector2(mapBotLeft.x, mapBotLeft.y), tileLookup, biomeFactory);

            float x = mapBotLeft.x;
            float y = mapBotLeft.y;

            int mapWidthInTiles = Convert.ToInt32(mapWidth / tileSize);
            int mapHeightInTiles = Convert.ToInt32(mapHeight / tileSize);

            for (int heightIndex = 0 ; heightIndex < mapHeightInTiles; heightIndex++)
            {
                List<Center> row = new List<Center>();
                x = mapBotLeft.x;

                for (int widthIndex = 0; widthIndex < mapWidthInTiles; widthIndex++)
                {
                    if (heightIndex == 0 && widthIndex == 0) {
                        row.Add(Root as Center);
                        continue;
                    }

                    Center node = new Center(false, false, false, biomeFactory.CreateBiome(BiomeType.None) as Biome.Biome, 0, new Vector2(x, y), tileLookup, biomeFactory);

                    if (IsOnMapEdge(node)) {
                        node.SetToOceanTile();
                    }

                    // If nodes on the left and bottom are not outside the map (i.e. >= 0),
                    // create edges between current node and them. This makes sure, while
                    // traversing to create defaults, the neighbour relationship between the
                    // neighbouring tiles is established.
                    if (widthIndex - 1 >= 0) {
                        node.Edges.Add(new Edge(node, row[widthIndex - 1]));
                    }

                    if (heightIndex - 1 >= 0) {
                        node.Edges.Add(new Edge(node, nodesMap[heightIndex - 1][widthIndex]));
                    }

                    row.Add(node);
                    x += tileSize;
                }

                nodesMap.Add(row);
                y += tileSize;
            }
        }

        
        // Returns Island tiles
        private HashSet<Center> GenerateOcean(IEnumerable<INode> centers)
        {
            HashSet<Center> islandTiles = new HashSet<Center>();

            foreach (Center center in centers)
            {
                if (IsAroundMapEdge(center.Position)) {
                    center.SetToOceanTile();
                }  else {
                    islandTiles.Add(center);
                }
            }

            return islandTiles;
        }

        private void GenerateIsland(IEnumerable<Center> tiles)
        {
            foreach (Center center in tiles)
            {
                if (center.Ocean) {
                    continue;
                }

                center.Water = (GetNoiseValFromPosition(center.Position.x, center.Position.y)) < chanceIslandTileIsWater;
            }
        }


        private void GenerateMarsh(IEnumerable<Center> nonOceanTiles)
        {
            foreach(Center center in nonOceanTiles) {
                center.SetToMarshTileIfWater();
            }
        }


        private float GetNoiseValFromPosition(float x, float y)
        {
            float offsetX = x + rng.Next(0, maxNoiseOffset);
            float offsetY = y + rng.Next(0, maxNoiseOffset);

            float xPos = (offsetX / (mapWidth / tileSize)) * noiseScale;
            float yPos = (offsetY / (mapHeight / tileSize)) * noiseScale;

            return Mathf.PerlinNoise(xPos, yPos);
        }


        private HashSet<Center> InitializeTiles(IEnumerable<INode> centers)
        {
            HashSet<Center> coast = new HashSet<Center>();

            foreach (Center c in centers)
            {
                c.Initialize();

                if (c.Coast) {
                    coast.Add(c);
                }
            }
            return coast;
        }


        private void SetElevation(IEnumerable<Center> outerTiles)
        {
            HashSet<Center> visitedTiles = new HashSet<Center>();

            foreach (Center tile in outerTiles)
            {
                if (!visitedTiles.Contains(tile))
                {
                    foreach (Center neighbour in tile.Neighbours)
                    {
                        if (neighbour.Elevation == 0)
                        {
                            neighbour.Elevation = tile.Elevation + elevationIncreaseRate;
                            visitedTiles.Add(neighbour);
                        }
                    }
                }
            }

            // TODO: Review this, as the 5 value was selected arbitrarily; or move to constant
            if (visitedTiles.Count > 5) {
                SetElevation(visitedTiles);
            }
        }


        private void AssignBiomes(IEnumerable<Center> islandTiles)
        {
            foreach (Center tile in islandTiles)
            {
                if (tile.Biome is Biome.Biome) {
                    tile.SetBiomeBasedOnElevation();
                }
            }
        }


        private void SpawnMembers(IEnumerable<INode> tiles, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                foreach (Center center in tiles) {
                    center.SpawnMembers();
                }
            }
        }

        private void SpawnSprites(IEnumerable<INode> map)
        {
            foreach (Center center in map) {
                center.SpawnSpriteWrtBiome();
            }
        }
    }
}
