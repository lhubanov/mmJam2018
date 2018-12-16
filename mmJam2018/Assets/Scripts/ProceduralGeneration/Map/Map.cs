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
    public enum GRAPH_TRAVERSAL_METHOD
    {
        BFS = 0,
        DFS = 1
    }

    // Note: Expose the serialized settings below to editor
    public class Map : Graph.Graph
    {
        // FIXME:   Find best constructional pattern to use w/ this many parameters;
        //          Add defaults that currently get set via a default constructor.
        private float mapWidth;
        private float mapHeight;

        private readonly Vector2 mapBotLeft;
        private readonly Vector2 mapTopRight;

        private float tileSize; // = 10f;

        private float oceanThreshold; // = 0.85f;
        private float chanceIslandTileIsWater; // = 0.5f;

        private double elevationIncreaseRate;
        private double elevationIncreaseMultiplier; // = 12;
        private int spawningIterations; // = 2;

        private float noiseScale; // = 1f;
        private int maxNoiseOffset; // = 100;

        private string rngSeed;
        private System.Random rng;
        private TileLookup tileLookup;

        private Transform parentGameObject;

        private GRAPH_TRAVERSAL_METHOD graphTraversalMethod; //= GRAPH_TRAVERSAL_METHOD.BFS;


        private BiomeFactory biomeFactory;


        public Map(MapSettingsContainer mapSettings)
        {
            mapBotLeft = mapSettings.MapBotLeft;
            mapTopRight = mapSettings.MapTopRight;

            oceanThreshold = mapSettings.OceanThreshold;
            chanceIslandTileIsWater = mapSettings.ChanceIslandTileIsWater;

            elevationIncreaseMultiplier = mapSettings.ElevationIncreaseMultiplier;

            spawningIterations = mapSettings.MemberSpawningIterations;

            noiseScale = mapSettings.NoiseScale;
            maxNoiseOffset = mapSettings.MaxNoiseOffset;

            tileLookup = mapSettings.TileLookup;
            parentGameObject = mapSettings.ParentGameObject;

            tileSize = mapSettings.TileSize;
            graphTraversalMethod = mapSettings.GraphTraversalMethod;

            rngSeed = mapSettings.RngSeed;
            InitConstants();
        }

        // Maybe just deprecate this?
        public Map(float tilesize, string seed, Vector3 regionTopLeft, Vector3 regionBotRight, TileLookup lookup)
        {
            // FIXME:   If this strange conversion needs to happen, probably need to just change the input Vector3s
            mapBotLeft = new Vector2(regionTopLeft.x, regionBotRight.y);
            mapTopRight = new Vector2(regionBotRight.x, regionTopLeft.y);

            mapWidth = Mathf.Abs(mapTopRight.x - mapBotLeft.x);
            mapHeight = Mathf.Abs(mapTopRight.y - mapBotLeft.y);

            tileSize = tilesize;
            tileLookup = lookup;

            InitConstants();
        }

        private void InitConstants()
        {
            biomeFactory = new BiomeFactory(rng, tileLookup, parentGameObject);

            rng = new System.Random(rngSeed.GetHashCode());

            mapWidth = Mathf.Abs(mapTopRight.x - mapBotLeft.x);
            mapHeight = Mathf.Abs(mapTopRight.y - mapBotLeft.y);

            // FIXME:   Define an actual formula for this eventually
            //          (+ the above is mostly due to the calculation saving 0 always if not done this way - investigate)
            double widthInTiles = mapWidth / tileSize;
            double coeff = (tileSize / widthInTiles) * elevationIncreaseMultiplier;
            elevationIncreaseRate = coeff;
        }

        // FIXME: Using these enum type is a bit archaic
        private Dictionary<int, Biome.BiomeType> BiomeConditions = new Dictionary<int, Biome.BiomeType>()
        {
            { 3, Biome.BiomeType.BeachBiome},
            { 10, Biome.BiomeType.GrasslandBiome},
            { 20, Biome.BiomeType.ForestBiome},
            { 200, Biome.BiomeType.SwampBiome}, // lol somehow this is the tallest biome
        };

        private Biome.IBiome CreateBiomeByName(Biome.BiomeType type)
        {
            // FIXME: Assign parent in BiomeFactory ( parent passed in via BiomeFactory constructor)
            return biomeFactory.CreateBiome(type);
        }


        private bool IsOnMapEdge(INode node)
        {
            return (node.Position.x == mapBotLeft.x || node.Position.x == mapTopRight.x) 
                || (node.Position.y == mapBotLeft.y || node.Position.y == mapTopRight.y);
        }

        // FIXME: Still broken - debug what the values are below
        private bool IsOceanTile(Vector2 pos)
        {
            var xMaxThreshold = mapTopRight.x  - (mapWidth * (1 - oceanThreshold));
            //var xMaxThreshold = mapTopRight.x * OceanThreshold;
            var xMinThreshold = mapBotLeft.x + (mapWidth * (1 - oceanThreshold));

            var yMaxThreshold = mapTopRight.y  - (mapHeight * (1- oceanThreshold));
            //var yMaxThreshold = mapTopRight.y * OceanThreshold;
            var yMinThreshold = mapBotLeft.y + (mapHeight * (1 - oceanThreshold));

            return (pos.x > xMaxThreshold || pos.x < xMinThreshold)
                || (pos.y > yMaxThreshold || pos.y < yMinThreshold);
        }

        public void Generate()
        {
            List<List<Center>> nodes = CreateDefaultNodes();
            //CreateGraph(nodes);

            //HashSet<Center> centers = TraverseDFS(Root as Center);
            HashSet<Center> centers = TraverseGraph(Root as Center, graphTraversalMethod);

            HashSet<Center> islandTiles = GenerateOcean(centers);
            GenerateIsland(islandTiles);

            // FIXME: Refactor this, to also use noise function!
            //        Also, maybe structurally refactor as well, it's
            //        a bit weird, how some proc gen map stuff happens within Center class  
            HashSet<Center> coastalTiles = InitializeTiles(centers);

            SetElevation(coastalTiles);
            AssignBiomes(islandTiles);

            SpawnSprites(centers);

            // Post processing runs to spawn tile/biome respective members (bushes, ruins etc.)
            // Note: This just calls the tile.Biome.Spawn(), so can be done on all tiles,
            //       if e.g. stuff needs to spawn in-ocean. Feeding it only island tiles for now
            SpawnMembers(islandTiles, spawningIterations);

            StrayIslandPostProcessing(centers);
        }

        // FIXME:   Can use some clean up;
        //          Does not need to return anything actually
        private List<List<Center>> CreateDefaultNodes()
        {
            List<List<Center>> nodesMap = new List<List<Center>>();
            Root = new Center(true, true, false, new Biome.Biome(rng, tileLookup), 0, 0, 0, new Vector2(mapBotLeft.x, mapBotLeft.y), tileLookup);

            float x = mapBotLeft.x;
            float y = mapBotLeft.y;

            int mapWidthInTiles = Convert.ToInt32(mapWidth / tileSize);
            int mapHeightInTiles = Convert.ToInt32(mapHeight / tileSize);

            for (int i = 0 ; i < mapHeightInTiles; i++)
            {
                List<Center> row = new List<Center>();
                x = mapBotLeft.x;

                for (int j = 0; j < mapWidthInTiles; j++)
                {
                    if (i == 0 && j == 0) {
                        row.Add(Root as Center);
                        continue;
                    }

                    Center node = new Center(false, false, false, new Biome.Biome(rng, tileLookup), 0, 0, Convert.ToInt32(x + y), new Vector2(x, y), tileLookup);

                    if (IsOnMapEdge(node)) {
                        node.Water = true;
                        node.Ocean = true;
                    }

                    if (j - 1 >= 0) {
                        node.Edges.Add(new Edge(node, row[j - 1], (node.Index + row[j - 1].Index)));
                    }

                    if (i - 1 >= 0) {
                        node.Edges.Add(new Edge(node, nodesMap[i - 1][j], (node.Index + nodesMap[i - 1][j].Index)));
                    }

                    row.Add(node);
                    x += tileSize;
                }

                nodesMap.Add(row);
                y += tileSize;
            }

            return nodesMap;
        }

        private void StrayIslandPostProcessing(HashSet<Center> tiles)
        {
            foreach(Center node in tiles)
            {
                node.StrayIslandTilePostProcess(rng, tileLookup);
            }
        }


        private delegate Center TraversalGetter();
        private delegate void TraversalSetter(Center center);

        // This seems a bit over-complicated for what I intended to do, review later
        private HashSet<Center> TraverseGraph(Center root, GRAPH_TRAVERSAL_METHOD method)
        {
            IEnumerable<Center> collection;
            TraversalGetter traversalGetter;
            TraversalSetter traversalSetter;

            if (method == GRAPH_TRAVERSAL_METHOD.BFS) {
                collection = new Queue<Center>();
                traversalGetter = new TraversalGetter((collection as Queue<Center>).Dequeue);
                traversalSetter = new TraversalSetter((collection as Queue<Center>).Enqueue);
            } else {
                collection = new Stack<Center>();
                traversalGetter = new TraversalGetter((collection as Stack<Center>).Pop);
                traversalSetter = new TraversalSetter((collection as Stack<Center>).Push);
            }

            var visited = new HashSet<Center>();
            traversalSetter(root);

            while(collection.Count() > 0)
            {
                Center node = traversalGetter();

                if (visited.Contains(node)) {
                    continue;
                }

                visited.Add(node);
                foreach(Center neighbour in node.Centers)
                {
                    if (!visited.Contains(neighbour)) {
                        traversalSetter(neighbour);
                    }
                }
            }

            return visited;
        }
        
        // Returns Island tiles
        private HashSet<Center> GenerateOcean(HashSet<Center> centers)
        {
            HashSet<Center> islandTiles = new HashSet<Center>();

            foreach (Center center in centers)
            {
                if (IsOceanTile(center.Position)) {
                    center.Water = true;
                    center.Ocean = true;
                }  else {
                    islandTiles.Add(center);
                }
            }

            return islandTiles;
        }

        private void GenerateIsland(HashSet<Center> tiles)
        {
            foreach (Center center in tiles)
            {
                if (center.Ocean) {
                    continue;
                }
                center.Water = (GetNoiseValueBasedOnPosition(center.Position.x, center.Position.y)) < chanceIslandTileIsWater;
            }
        }


        private float GetNoiseValueBasedOnPosition(float x, float y)
        {
            float offsetX = x + rng.Next(0, maxNoiseOffset);
            float offsetY = y + rng.Next(0, maxNoiseOffset);

            float xPos = (offsetX / (mapWidth / tileSize)) * noiseScale;
            float yPos = (offsetY / (mapHeight / tileSize)) * noiseScale;

            return Mathf.PerlinNoise(xPos, yPos);
        }


        // FIXME: Refactor this, to also use noise function!
        private HashSet<Center> InitializeTiles(HashSet<Center> centers)
        {
            HashSet<Center> coast = new HashSet<Center>();

            foreach (Center c in centers)
            {
                c.Initialize(rng);

                if (c.Coast) {
                    coast.Add(c);
                }

                Console.WriteLine(String.Format("Visited: Index {0}, posX {1}, posY {2}, Biome {3}", c.Index, c.Position.x, c.Position.y, c.Biome.ToString()));
            }
            return coast;
        }


        private void SetElevation(HashSet<Center> outerTiles)
        {
            HashSet<Center> visitedTiles = new HashSet<Center>();

            foreach (Center tile in outerTiles)
            {
                if (!visitedTiles.Contains(tile))
                {
                    foreach (Center neighbour in tile.Centers)
                    {
                        if (neighbour.Elevation == 0)
                        {
                            neighbour.Elevation = tile.Elevation + elevationIncreaseRate;
                            visitedTiles.Add(neighbour);

                            // As there should be only one of these, so we can skip the rest
                            break;
                        }
                    }
                }
            }

            // TODO: Review this, as the 5 value was selected arbitrarily
            if (visitedTiles.Count > 5) {
                SetElevation(visitedTiles);
            }
        }


        private void AssignBiomes(HashSet<Center> islandTiles)
        {
            foreach (Center tile in islandTiles)
            {
                if (tile.Biome.biomeType == Biome.BiomeType.None) {
                    tile.Biome = CreateBiomeByName(BiomeConditions.Where(x => x.Key > tile.Elevation).First().Value);
                    tile.SpawnMembers();
                }
            }
        }


        private void SpawnMembers(HashSet<Center> tiles, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                foreach (Center center in tiles)
                {

                    // FIXME: having this in actually changes the RNG!
                    //        Hence, without it, either no spawning or full spawning happens
                    if (!(center.Biome is Biome.OceanBiome))
                    {
                        Console.WriteLine("Debuggiiiing...");
                    }

                    center.SpawnMembers();
                }
            }
        }

        private void SpawnSprites(HashSet<Center> map)
        {
            foreach (Center center in map)
            {
                center.SpawnSprite();
            }
        }
    }
}
