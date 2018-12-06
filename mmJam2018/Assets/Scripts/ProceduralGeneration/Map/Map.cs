using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine;

using ProceduralGeneration.Graph;
using ProceduralGeneration.Node;

namespace ProceduralGeneration.Map
{
    // Note: Expose the serialized settings below to editor
    public class Map : Graph.Graph
    {
        private int mapWidth;
        private int mapHeight;

        // Same as exposed generator, this assumes tiles are square
        [SerializeField]
        private int tileSize = 10;

        private readonly float horizontalMapEdge;
        private readonly float verticalMapEdge;

        [Range(0, 1)]
        [SerializeField]
        public float OceanThreshold = 0.85f;

        [SerializeField]
        private double elevationIncreaseRate;

        [SerializeField]
        private double elevationIncreaseMultiplier = 7.5;

        [SerializeField]
        private int spawningIterations = 2;

        // some sort of noise map(?)
        // e.g. from C++ source:
        //noise::module::Perlin *noiseMap;

        [SerializeField]
        private string rngSeed;

        private System.Random rng;

        public Map(int width, int height, int tile, string seed)
        {
            mapWidth = width;
            mapHeight = height;
            tileSize = tile;
            rngSeed = seed;
            rng = new System.Random(rngSeed.GetHashCode());


            horizontalMapEdge = (mapWidth / tileSize) - 1;
            verticalMapEdge = (mapHeight / tileSize) - 1;


            // FIXME:   Define an actual formula for this eventually
            //          (+ the above is mostly due to the calculation saving 0 always if not done this way - investigate)
            double widthInTiles = mapWidth / tileSize;
            double coeff = (tileSize / widthInTiles) * elevationIncreaseMultiplier;
            elevationIncreaseRate = coeff;
        }


        // FIXME:   This uses the enums to create instances of objects
        //          need a more intelligent solution for this. Good ole dict of objects or types?
        private Dictionary<int, Biome.BiomeType> BiomeConditions = new Dictionary<int, Biome.BiomeType>()
        {
            { 5, Biome.BiomeType.BeachBiome},
            { 10, Biome.BiomeType.GrasslandBiome},
            { 30, Biome.BiomeType.ForestBiome},
            { 100, Biome.BiomeType.SwampBiome}, // lol somehow this is the tallest biome
        };

        private Biome.IBiome CreateBiomeByName(Biome.BiomeType name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullName = "GraphsDemo.Map.Biome." + name.ToString();

            System.Object obj = assembly.CreateInstance(fullName);
            Biome.IBiome b = obj as Biome.IBiome;

            return b;
        }

        private bool IsOnMapEdge(INode node)
        {
            return (node.Position.x == 0 || node.Position.x == horizontalMapEdge) || (node.Position.y == 0 || node.Position.y == verticalMapEdge);
        }

        private bool IsValidPosition(Vector2 pos)
        {
            return (pos.x >= 0 && pos.x < mapWidth / tileSize) && (pos.y >= 0 && pos.y < mapHeight / tileSize);
        }

        private bool IsOceanTile(Vector2 pos)
        {
            return (pos.x > horizontalMapEdge * OceanThreshold || pos.x < horizontalMapEdge * (1 - OceanThreshold))
                || (pos.y > horizontalMapEdge * OceanThreshold || pos.y < horizontalMapEdge * (1 - OceanThreshold));
        }


        // Possible member methods

        //bool IsIsland(Vec2 position);
        //void AssignOceanCoastLand();
        //void AssignCornerElevation();
        //void RedistributeElevations();
        //void AssignPolygonElevations();
        //void CalculateDownslopes();
        //void GenerateRivers();
        //void AssignCornerMoisture();
        //void RedistributeMoisture();
        //void AssignPolygonMoisture();
        //void AssignBiomes();

        //void GeneratePoints();    //Voronoi
        //void LloydRelaxation();
        //void Triangulate(vector<del::vertex> puntos);  //Delaunay
        //void FinishInfo();

        //void AddCenter(center* c);
        //center* GetCenter(Vec2 position);
        //void OrderPoints(vector<corner*> &corners);

        //vector<corner*> GetLandCorners();
        //vector<corner*> GetLakeCorners();

        //static unsigned int HashString(string seed);
        //string CreateSeed(int length);


        //public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        //{
        //    from.Neighbours.Add(to);
        //    from.Costs.Add(cost);

        //    to.Neighbours.Add(from);
        //    to.Costs.Add(cost);
        //}

        //public bool Contains(T value)
        //{
        //    return nodeSet.FindByValue(value) != null;
        //}


        // FIXME:   I still don't like this, its sub-optimal
        //          Research options in terms of graphs (some sort of BFS creation of a graph) 
        //          and just look at all available options in C# land 
        private List<List<Center>> CreateDefaultNodes()
        {
            List<List<Center>> nodesMap = new List<List<Center>>();
            Root = new Center(true, true, false, new Biome.Biome(rng), 0, 0, 0, new Vector2(0, 0));

            int horizontalTiles = mapWidth / tileSize;
            int verticalTiles = mapHeight / tileSize;

            for (int i = 0; i < horizontalTiles; i++)
            {
                List<Center> row = new List<Center>();

                for (int j = 0; j < verticalTiles; j++)
                {
                    if (i == 0 && j == 0) {
                        row.Add(Root as Center);
                        continue;
                    }

                    Center node = new Center(false, false, false, new Biome.Biome(rng), 0, 0, Convert.ToUInt32(i + j), new Vector2(i, j));

                    if (IsOnMapEdge(node)) {
                        node.Water = true;
                        node.Ocean = true;
                    }

                    row.Add(node);
                }

                nodesMap.Add(row);
            }

            return nodesMap;
        }


        private void CreateGraph(List<List<Center>> nodes)
        {
            // FIXME: This seems sub-optimal. Can we assign neghbours during default creation?
            foreach (List<Center> row in nodes)
            {
                foreach (Center node in row)
                {
                    // Edges are only adjacent tiles, no diagonals
                    Vector2 currentPos = node.Position;

                    Vector2 topNeighbour = new Vector2(node.Position.x, node.Position.y + 1);
                    if (IsValidPosition(topNeighbour))
                    {
                        Center top = nodes[Convert.ToInt32(topNeighbour.x)][Convert.ToInt32(topNeighbour.y)];

                        // FIXME:   Index creation does not create unique values
                        node.Edges.Add(new Edge(node, top, (node.Index + top.Index)));
                    }

                    Vector2 botNeighbour = new Vector2(node.Position.x, node.Position.y - 1);
                    if (IsValidPosition(botNeighbour))
                    {
                        Center bot = nodes[Convert.ToInt32(botNeighbour.x)][Convert.ToInt32(botNeighbour.y)];
                        node.Edges.Add(new Edge(node, bot, (node.Index + bot.Index)));
                    }

                    Vector2 leftNeighbour = new Vector2(node.Position.x - 1, node.Position.y);
                    if (IsValidPosition(leftNeighbour))
                    {
                        Center left = nodes[Convert.ToInt32(leftNeighbour.x)][Convert.ToInt32(leftNeighbour.y)];
                        node.Edges.Add(new Edge(node, left, (node.Index + left.Index)));
                    }

                    Vector2 rightNeighbour = new Vector2(node.Position.x + 1, node.Position.y);
                    if (IsValidPosition(rightNeighbour))
                    {
                        Center right = (Center)nodes[Convert.ToInt32(rightNeighbour.x)][Convert.ToInt32(rightNeighbour.y)];
                        node.Edges.Add(new Edge(node, right, (node.Index + right.Index)));
                    }
                }
            }
        }


        private HashSet<Center> TraverseDFS(Center root)
        {
            HashSet<Center> visited = new HashSet<Center>();
            Stack<Center> stack = new Stack<Center>();

            stack.Push(root);

            while (stack.Count > 0)
            {
                Center node = stack.Pop();

                if (visited.Contains(node))
                {
                    continue;
                }

                visited.Add(node);

                foreach (Center neighbour in node.Centers)
                {
                    if (!visited.Contains(neighbour))
                    {
                        stack.Push(neighbour);
                    }
                }
            }

            return visited;
        }

        // private HashSet<Center> TraverseBFS(Center root) 
        // {
        // }

        public void Generate()
        {
            List<List<Center>> nodes = CreateDefaultNodes();
            CreateGraph(nodes);

            // Try using BFS to compare map shapes etc.
            HashSet<Center> centers = TraverseDFS(Root as Center);

            HashSet<Center> islandTiles = GenerateOcean(centers);
            GenerateIsland(islandTiles);

            //HashSet<Center> coastalTiles = new HashSet<Center>();
            HashSet<Center> coastalTiles = InitializeTiles(centers);

            SetElevation(coastalTiles);
            AssignBiomes(islandTiles);

            // Post processing runs to spawn tile/biome respective members (bushes, ruins etc.)
            // Note: This just calls the tile.Biome.Spawn(), so can be done on all tiles,
            //       if e.g. stuff needs to spawn in-ocean. Feeding it only island tiles for now
            SpawnMembers(islandTiles, spawningIterations);

            // FIXME: Spawn actual sprites here - use RegionGenerator for inspiration
            SaveMapAsText(centers);
        }


        // Returns Island tiles
        private HashSet<Center> GenerateOcean(HashSet<Center> centers)
        {
            Debug.Log(String.Format("Generating ocean. Ocean Threshold X: lowerRange {0} - {1}", 0, horizontalMapEdge * (1 - OceanThreshold)));
            Debug.Log(String.Format("Generating ocean. Ocean Threshold X: upperRange {0} - {1}", horizontalMapEdge * OceanThreshold, horizontalMapEdge));
            Debug.Log(String.Format("Generating ocean. Ocean Threshold Y: lowerRange {0} - {1}", 0, verticalMapEdge * (1 - OceanThreshold)));
            Debug.Log(String.Format("Generating ocean. Ocean Threshold Y: upperRange {0} - {1}", verticalMapEdge * OceanThreshold, verticalMapEdge));

            HashSet<Center> islandTiles = new HashSet<Center>();

            foreach (Center center in centers)
            {
                if (IsOceanTile(center.Position))
                {
                    Console.WriteLine(String.Format("Setting tile to ocean pos(x/y): {0} / {1}", center.Position.x, center.Position.y));

                    center.Water = true;
                    center.Ocean = true;
                } 
                else
                {
                    islandTiles.Add(center);
                }
            }

            return islandTiles;
        }


        private void GenerateIsland(HashSet<Center> islandTiles)
        {
            Debug.Log(String.Format("Generating island. Island Range X: {0} - {1}", horizontalMapEdge * (1 - OceanThreshold), horizontalMapEdge));
            Debug.Log(String.Format("Generating island. Island Range Y: {0} - {1}", verticalMapEdge * (1 - OceanThreshold), verticalMapEdge));

            foreach (Center center in islandTiles)
            {
                //if (IsOceanTile(center.Position)) {
                //    continue;
                //}

                // FIXME: Use noise function here instead
                center.Water = rng.Next(0, 100) < 50;
            }
        }


        // FIXME: Refactor this, no need to use random- use noise function/seed instead
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

            if (visitedTiles.Count > 5)
            {
                SetElevation(visitedTiles);
            }
        }


        private void AssignBiomes(HashSet<Center> islandTiles)
        {
            foreach (Center tile in islandTiles)
            {
                if (tile.Biome.biomeType == Biome.BiomeType.None)
                {
                    tile.Biome = CreateBiomeByName(BiomeConditions.Where(x => x.Key > tile.Elevation).First().Value);
                }
            }
        }


        private void SpawnMembers(HashSet<Center> tiles, int iterations)
        {
            SaveMapAsText(tiles, "intermediate.txt");

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


        private void SaveMapAsText(HashSet<Center> map, string fileName = "demo.txt")
        {
            string[,] table = new string[mapWidth / tileSize, mapHeight / tileSize];

            foreach (Center center in map)
            {
                string symbol;
                if (center.Biome.biomeType == Biome.BiomeType.Ocean)
                {
                    symbol = @"*";
                }
                else
                {
                    int val = (int)center.Biome.biomeType;
                    symbol = val.ToString();
                }

                table[Convert.ToInt32(center.Position.x), Convert.ToInt32(center.Position.y)] = symbol;
            }

            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
            {
                for (int r = 0; r < mapWidth / tileSize; r++)
                {
                    for (int c = 0; c < mapHeight / tileSize; c++)
                    {
                        sw.Write(table[r, c]);
                    }

                    sw.Write(Environment.NewLine);
                }
            }

            Console.WriteLine("Map saved as text...");
        }
    }
}
