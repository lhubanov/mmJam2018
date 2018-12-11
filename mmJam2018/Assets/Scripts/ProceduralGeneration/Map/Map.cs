using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Node;

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
        private readonly float mapWidth;
        private readonly float mapHeight;

        [SerializeField]
        private int tileSize = 10;

        private readonly Vector2 mapBotLeft;
        private readonly Vector2 mapTopRight;


        [Range(0, 1)]
        [SerializeField]
        public float OceanThreshold = 0.85f;

        [Range(0, 1)]
        [SerializeField]
        private float chanceIslandTileIsWater = 0.5f;

        [SerializeField]
        private double elevationIncreaseRate;

        [SerializeField]
        private double elevationIncreaseMultiplier = 12;

        [SerializeField]
        private int spawningIterations = 2;

        [SerializeField]
        [Range(0, 1)]
        private float waterSpawnProbability = 0.5f;

        [SerializeField]
        private float noiseScale = 1f;

        [SerializeField]
        private int maxNoiseOffset = 100;

        [SerializeField]
        private string rngSeed;
        private System.Random rng;
        private TileLookup tileLookup;

        [SerializeField]
        private GRAPH_TRAVERSAL_METHOD graphTraversalMethod = GRAPH_TRAVERSAL_METHOD.BFS;


        public Map(int tilesize, string seed, Vector3 regionTopLeft, Vector3 regionBotRight, TileLookup lookup)
        {
            // FIXME:   If this strange conversion needs to happen, probably need to just change the input Vector3s
            mapBotLeft = new Vector2(regionTopLeft.x, regionBotRight.y);
            mapTopRight = new Vector2(regionBotRight.x, regionTopLeft.y);

            mapWidth = Mathf.Abs(mapTopRight.x - mapBotLeft.x);
            mapHeight = Mathf.Abs(mapTopRight.y - mapBotLeft.y);

            tileSize = tilesize;
            tileLookup = lookup;
            rngSeed = seed;
            rng = new System.Random(rngSeed.GetHashCode());


            // FIXME:   Define an actual formula for this eventually
            //          (+ the above is mostly due to the calculation saving 0 always if not done this way - investigate)
            double widthInTiles = mapWidth / tileSize;
            double coeff = (tileSize / widthInTiles) * elevationIncreaseMultiplier;
            elevationIncreaseRate = coeff;
        }


        // FIXME: Using these enum type is a bit archaic
        private Dictionary<int, Biome.BiomeType> BiomeConditions = new Dictionary<int, Biome.BiomeType>()
        {
            { 5, Biome.BiomeType.BeachBiome},
            { 10, Biome.BiomeType.GrasslandBiome},
            { 30, Biome.BiomeType.ForestBiome},
            { 50, Biome.BiomeType.SwampBiome}, // lol somehow this is the tallest biome
        };

        private Biome.IBiome CreateBiomeByName(Biome.BiomeType name)
        {
            Type type = Type.GetType("ProceduralGeneration.Biome." + name.ToString());
            System.Object obj = Activator.CreateInstance(type, rng, tileLookup);

            return obj as Biome.IBiome;
        }


        private bool IsOnMapEdge(INode node)
        {
            return (node.Position.x == mapBotLeft.x || node.Position.x == mapTopRight.x) 
                || (node.Position.y == mapBotLeft.y || node.Position.y == mapTopRight.y);
        }

        // FIXME: This might still be broken - try the commented out below
        private bool IsOceanTile(Vector2 pos)
        {
            //var xMaxThreshold = mapTopRight.x  - (mapWidth * OceanThreshold);
            var xMaxThreshold = mapTopRight.x * OceanThreshold;
            var xMinThreshold = mapBotLeft.x + (mapWidth * (1 - OceanThreshold));

            //var yMaxThreshold = mapTopRight.y  - (mapHeight * OceanThreshold);
            var yMaxThreshold = mapTopRight.y * OceanThreshold;
            var yMinThreshold = mapBotLeft.y + (mapHeight * (1 - OceanThreshold));

            return (pos.x > xMaxThreshold || pos.x < xMinThreshold)
                || (pos.y > yMaxThreshold || pos.y < yMinThreshold);
        }

        public void Generate()
        {
            List<List<Center>> nodes = CreateDefaultNodes();
            CreateGraph(nodes);

            //HashSet<Center> centers = TraverseDFS(Root as Center);
            HashSet<Center> centers = TraverseGraph(Root as Center, graphTraversalMethod);

            HashSet<Center> islandTiles = GenerateOcean(centers);
            GenerateIsland(islandTiles);

            // FIXME: Refactor this, to also use noise function!
            HashSet<Center> coastalTiles = InitializeTiles(centers);

            SetElevation(coastalTiles);
            AssignBiomes(islandTiles);

            SpawnSprites(centers);

            // Post processing runs to spawn tile/biome respective members (bushes, ruins etc.)
            // Note: This just calls the tile.Biome.Spawn(), so can be done on all tiles,
            //       if e.g. stuff needs to spawn in-ocean. Feeding it only island tiles for now
            SpawnMembers(islandTiles, spawningIterations);
        }

        // FIXME:   I still don't like this, its sub-optimal
        //          Research options in terms of graphs (some sort of BFS creation of a graph) 
        //          and just look at all available options in C# land 
        private List<List<Center>> CreateDefaultNodes()
        {
            List<List<Center>> nodesMap = new List<List<Center>>();
            Root = new Center(true, true, false, new Biome.Biome(rng), 0, 0, 0, new Vector2(mapBotLeft.x, mapBotLeft.y), tileLookup);

            float x = mapBotLeft.x;
            float y = mapBotLeft.y;

            int mapWidthInTiles = Convert.ToInt32(mapWidth / tileSize);
            int mapHeightInTiles = Convert.ToInt32(mapHeight / tileSize);

            for (float i = 0 ; i < mapWidthInTiles; i++)
            {
                List<Center> row = new List<Center>();
                y = mapBotLeft.y;

                for (float j = 0; j < mapHeightInTiles; j++)
                {
                    if (i == 0 && j == 0) {
                        row.Add(Root as Center);
                        continue;
                    }

                    Center node = new Center(false, false, false, new Biome.Biome(rng), 0, 0, Convert.ToInt32(x + y), new Vector2(x, y), tileLookup);

                    if (IsOnMapEdge(node)) {
                        node.Water = true;
                        node.Ocean = true;
                    }

                    row.Add(node);
                    y += tileSize;
                }

                nodesMap.Add(row);
                x += tileSize;
            }

            return nodesMap;
        }

        // FIXME: I've hacked this about to get it working, so refactor later pls
        private void CreateGraph(List<List<Center>> nodes)
        {
            // FIXME: This seems sub-optimal. Can we assign neghbours during the centers' creation in the loops above?
            //        Or loop more inteligently?
            int rows = nodes.Count;
            int columns = nodes.First().Count;

            foreach (List<Center> row in nodes)
            {
                int rowNumber = nodes.IndexOf(row);

                foreach (Center node in row)
                {
                    int columnNumber = row.IndexOf(node);

                    // Edges are only adjacent tiles, no diagonals

                    int top = rowNumber + 1;
                    if (top >= 0 && top < rows)
                    {
                        Center topNeigbhour = nodes[top][columnNumber];
                        // FIXME:   Index creation does not create unique values
                        node.Edges.Add(new Edge(node, topNeigbhour, (node.Index + topNeigbhour.Index)));
                    }

                    int bot = rowNumber - 1;
                    if (bot >= 0 && bot < rows)
                    {
                        Center botNeighbour = nodes[bot][columnNumber];
                        node.Edges.Add(new Edge(node, botNeighbour, (node.Index + botNeighbour.Index)));
                    }

                    int left = columnNumber - 1;
                    if (left >= 0 && left < columns)
                    {
                        Center leftNeighbour = nodes[rowNumber][left];
                        node.Edges.Add(new Edge(node, leftNeighbour, (node.Index + leftNeighbour.Index)));
                    }

                    int right = columnNumber + 1;
                    if (right >= 0 && right < columns)
                    {
                        Center rightNeighbour = (Center)nodes[rowNumber][right];
                        node.Edges.Add(new Edge(node, rightNeighbour, (node.Index + rightNeighbour.Index)));
                    }
                }
            }
        }

        private delegate Center TraversalGetter();
        private delegate void TraversalSetter(Center center);

        // Naturally, this needs testing
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

        //private HashSet<Center> TraverseDFS(Center root)
        //{
        //    var visited = new HashSet<Center>();
        //    var stack = new Stack<Center>();

        //    stack.Push(root);

        //    while (stack.Count > 0)
        //    {
        //        Center node = stack.Pop();

        //        if (visited.Contains(node))
        //        {
        //            continue;
        //        }

        //        visited.Add(node);

        //        foreach (Center neighbour in node.Centers)
        //        {
        //            if (!visited.Contains(neighbour))
        //            {
        //                stack.Push(neighbour);
        //            }
        //        }
        //    }

        //    return visited;
        //}

        //private HashSet<Center> TraverseBFS(Center root)
        //{
        //    var visited = new HashSet<Center>();
        //    var queue = new Queue<Center>();

        //    queue.Enqueue(root);

        //    while(queue.Count > 0)
        //    {
        //        Center node = queue.Dequeue();

        //        if (visited.Contains(node)) {
        //            continue;
        //        }

        //        visited.Add(node);

        //        foreach(Center neighbour in node.Centers)
        //        {
        //            if (!visited.Contains(neighbour)){
        //                queue.Enqueue(neighbour);
        //            }
        //        }
        //    }

        //    return visited;
        //}


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

        private void GenerateIsland(HashSet<Center> islandTiles)
        {
            // Note: If this is to be used on whole map, add check if tile is ocean before setting center.Water to anything
            foreach (Center center in islandTiles) {
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
