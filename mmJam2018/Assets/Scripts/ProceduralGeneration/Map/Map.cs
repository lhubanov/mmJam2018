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
    public class Map : Graph.Graph
    {
        private int mapWidth;
        private int mapHeight;

        // Tiles are square, so using this vertically and horizontally
        [SerializeField]
        private int tileSize = 10;

        private readonly int horizontalMapEdge;
        private readonly int verticalMapEdge;

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

        public string Seed;


        // FIXME: Porting to Unity will make this redundant
        private Dictionary<int, Biome.BiomeType> BiomeConditions = new Dictionary<int, Biome.BiomeType>()
        {
            { 5, Biome.BiomeType.BeachBiome},
            { 10, Biome.BiomeType.GrasslandBiome},
            { 30, Biome.BiomeType.ForestBiome},
            { 100, Biome.BiomeType.SwampBiome}, // lol somehow this is the tallest biome
        };

        // FIXME: Mostly exists due to the enum requirement thing, refactor/remove in Unity
        private Biome.IBiome CreateBiomeByName(Biome.BiomeType name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullName = "GraphsDemo.Map.Biome." + name.ToString();

            System.Object obj = assembly.CreateInstance(fullName);
            Biome.IBiome b = obj as Biome.IBiome;

            return b;
        }

        public Map(int width, int height, string seed)
        {
            mapWidth = width;
            mapHeight = height;

            // FIXME: use seed here to generate random/in noise func etc.

            horizontalMapEdge = (mapWidth / tileSize) - 1;
            verticalMapEdge = (mapHeight / tileSize) - 1;

            double widthInTiles = mapWidth / tileSize;
            double coeff = (tileSize / widthInTiles) * elevationIncreaseMultiplier;

            // FIXME:   maybe define an actual formula for this eventually
            //          (+ the above is mostly due to the calculation saving 0 always if not done this way - investigate)
            elevationIncreaseRate = coeff;
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

        public void Generate()
        {
            // Distribute the Voronoi points around here
            // Function turns out a bit simpler, as we're using tiles (for now)
            List<List<Center>> nodesMap = new List<List<Center>>();

            // basically the top left node
            Root = new Center(true, true, false, new Biome.Biome(), 0, 0, 0, new Vector2(0,0));
            
            int horizontalTiles = mapWidth / tileSize;
            int verticalTiles = mapHeight / tileSize;

            for(int i = 0; i < horizontalTiles; i++)
            {
                List<Center> row = new List<Center>();

                for (int j = 0; j < verticalTiles; j++)
                {
                    if (i == 0 && j ==0) {
                        row.Add(Root as Center);
                        continue;
                    }

                    Center node = new Center(false, false, false, new Biome.Biome(), 0, 0, Convert.ToUInt32(i + j), new Vector2(i, j));

                    // FIXME: Move to GenerateOcean() method
                    if (IsOnMapEdge(node)) {
                        node.Water = true;
                        node.Ocean = true;
                    }

                    row.Add(node);
                }

                nodesMap.Add(row);
            }


            // FIXME: Post-processing to attach nodes as neighbours to each other/create edges seems super sub-optimal
            foreach (List<Center> row in nodesMap)
            {
                foreach(Center node in row)
                {
                    // Edges are only adjacent tiles, no diagonals
                    Vector2 currentPos = node.Position;

                    // Maybe have one get function and return them as a list to iterate on?
                    // But it would need the list of list of INodes to be passed in as well
                    // x-1; x+1
                    // y-1; y+1

                    // FIXME: This is the most sub-optimal shit- refactor
                    Vector2 topNeighbour = new Vector2(node.Position.x, node.Position.y + 1);
                    if (IsValidPosition(topNeighbour))
                    {
                        Center top = nodesMap[Convert.ToInt32(topNeighbour.x)][Convert.ToInt32(topNeighbour.y)];

                        // FIXME:   Index creation does not create unique values
                        // NB!      Overriding GetHashCode() and Equals should mean that the new Edges don't get added over and over, but needs testing!
                        node.Edges.Add(new Edge(node, top, (node.Index + top.Index)));
                    }

                    Vector2 botNeighbour = new Vector2(node.Position.x, node.Position.y - 1);
                    if(IsValidPosition(botNeighbour))
                    {
                        Center bot = nodesMap[Convert.ToInt32(botNeighbour.x)][Convert.ToInt32(botNeighbour.y)];
                        node.Edges.Add(new Edge(node, bot, (node.Index + bot.Index)));
                    }

                    Vector2 leftNeighbour = new Vector2(node.Position.x - 1, node.Position.y);
                    if (IsValidPosition(leftNeighbour))
                    {
                        Center left = nodesMap[Convert.ToInt32(leftNeighbour.x)][Convert.ToInt32(leftNeighbour.y)];
                        node.Edges.Add(new Edge(node, left, (node.Index + left.Index)));
                    }

                    Vector2 rightNeighbour = new Vector2(node.Position.x + 1, node.Position.y);
                    if (IsValidPosition(rightNeighbour))
                    {
                        Center right = (Center)nodesMap[Convert.ToInt32(rightNeighbour.x)][Convert.ToInt32(rightNeighbour.y)];
                        node.Edges.Add(new Edge(node, right, (node.Index + right.Index)));
                    }
                }
            }

            // FIXME: Try using BFS to compare map shapes etc.
            //        Move to base class, also (will need to do type conversions from INodes to Centers btw)!
            var centers = TraverseDFS(Root as Center);

            GenerateOcean(centers);
            GenerateIsland(centers);

            HashSet<Center> coastalTiles = new HashSet<Center>();
            coastalTiles = InitializeTiles(centers);

            SetElevation(coastalTiles);
            AssignBiomes(centers);

            // Post processing runs to spawn tile/biome respective members (bushes, ruins etc.)
            SpawnMembers(centers, spawningIterations);

            SaveMapAsText(centers);
        }

        private bool IsOnMapEdge(INode node)
        {
            return (node.Position.x == 0 || node.Position.x == horizontalMapEdge) || (node.Position.y == 0 || node.Position.y == verticalMapEdge);
        }

        private bool IsValidPosition(Vector2 pos)
        {
            return (pos.x >= 0 && pos.x < mapWidth/tileSize) && (pos.y >= 0 && pos.y < mapHeight/tileSize);
        }

        private bool IsOceanTile(Vector2 pos)
        {
            return (pos.x > horizontalMapEdge * OceanThreshold || pos.x < horizontalMapEdge * (1 - OceanThreshold))
                || (pos.y > horizontalMapEdge * OceanThreshold || pos.y < horizontalMapEdge * (1 - OceanThreshold));
        }


        // Returns visited
        public HashSet<Center> TraverseDFS(Center root)
        {
            HashSet<Center> visited = new HashSet<Center>();
            Stack<Center> stack = new Stack<Center>();

            stack.Push(root);

            while(stack.Count > 0)
            {
                Center node = stack.Pop();

                if (visited.Contains(node)) {
                    continue;
                }

                visited.Add(node);

                foreach(Center neighbour in node.Centers)
                {
                    if (!visited.Contains(neighbour)) {
                        stack.Push(neighbour);
                    }
                }
            }

            return visited;
        }


        private void SetElevation(HashSet<Center> outerTiles)
        {
            HashSet<Center> visitedTiles = new HashSet<Center>();

            foreach(Center tile in outerTiles)
            {
                if (!visitedTiles.Contains(tile))
                { 
                    foreach(Center neighbour in tile.Centers)
                    {
                        if (neighbour.Elevation == 0) {
                            neighbour.Elevation = tile.Elevation + elevationIncreaseRate;
                            visitedTiles.Add(neighbour);

                            // As there should be only one of these, so we can skip the rest
                            break;
                        }
                    }
                }
            }

            if(visitedTiles.Count > 5) {
                SetElevation(visitedTiles);
            }
        }
        

        // FIXME:   The number of iterations here can be massively reduced if we only give
        //          the island tiles as input
        private void AssignBiomes(HashSet<Center> tiles)
        {
            foreach(Center tile in tiles) {
                if (tile.Biome.biomeType == Biome.BiomeType.None) {
                    tile.Biome = CreateBiomeByName(BiomeConditions.Where(x => x.Key > tile.Elevation).First().Value);
                }
            }
        }

        // FIXME: Reduce amount of tiles used here to only island tiles (gotten from GenerateIsland())
        private void SpawnMembers(HashSet<Center> tiles, int iterations)
        {
            SaveMapAsText(tiles, "intermediate.txt");

            for(int i = 0; i < iterations; i++)
            {
                foreach(Center center in tiles)
                {

                    // FIXME: having this in actually changes the RNG!
                    //        Hence, without it, either no spawning or full spawning happens
                    if(!(center.Biome is Biome.OceanBiome))
                    {
                        Console.WriteLine("Debuggiiiing...");
                    }

                    center.SpawnMembers();
                }
            }
        }


        private void SaveMapAsText(HashSet<Center> map, string fileName = "demo.txt")
        {
            string[,] table = new string[mapWidth/tileSize, mapHeight/tileSize];

            foreach(Center center in map)
            {
                string symbol;
                if(center.Biome.biomeType == Biome.BiomeType.Ocean) {
                    symbol = @"*";
                } else {
                    int val = (int)center.Biome.biomeType;
                    symbol = val.ToString();
                }

                table[Convert.ToInt32(center.Position.x),Convert.ToInt32(center.Position.y)] = symbol;
            }

            using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
            { 
                for (int r = 0; r < mapWidth / tileSize; r++) {
                    for (int c = 0; c < mapHeight / tileSize; c++) {
                        sw.Write(table[r, c]);
                    }

                    sw.Write(Environment.NewLine);
                }
            }

            Console.WriteLine("Map saved as text...");
        }

        private HashSet<Center> InitializeTiles(HashSet<Center> centers)
        {
            HashSet<Center> coast = new HashSet<Center>();

            System.Random r = new System.Random();
            foreach (Center c in centers)
            {
                c.Initialize(r);

                if (c.Coast) {
                    coast.Add(c);
                }

                Console.WriteLine(String.Format("Visited: Index {0}, posX {1}, posY {2}, Biome {3}", c.Index, c.Position.x, c.Position.y, c.Biome.ToString()));
            }

            return coast;
        }

        private void GenerateIsland(HashSet<Center> centers)
        {
            Console.WriteLine(String.Format("Generating island. Island Range X: {0} - {1}", horizontalMapEdge * (1 - OceanThreshold), horizontalMapEdge));
            Console.WriteLine(String.Format("Generating island. Island Range Y: {0} - {1}", verticalMapEdge * (1 - OceanThreshold), verticalMapEdge));

            System.Random rand = new System.Random();

            foreach (Center center in centers)
            {
                // FIXME: Again, can probably reduce amount of tiles in hashset after setting ocean tiles before this?
                if (IsOceanTile(center.Position)) {
                    continue;
                }

                // FIXME: Use noise function here instead
                center.Water = rand.Next(0, 100) < 50;
            }
        }

        private void GenerateOcean(HashSet<Center> centers)
        {
            Console.WriteLine(String.Format("Generating ocean. Ocean Threshold X: lowerRange {0} - {1}", 0, horizontalMapEdge*(1- OceanThreshold)));
            Console.WriteLine(String.Format("Generating ocean. Ocean Threshold X: upperRange {0} - {1}", horizontalMapEdge* OceanThreshold, horizontalMapEdge));
            Console.WriteLine(String.Format("Generating ocean. Ocean Threshold Y: lowerRange {0} - {1}", 0, verticalMapEdge * (1- OceanThreshold)));
            Console.WriteLine(String.Format("Generating ocean. Ocean Threshold Y: upperRange {0} - {1}", verticalMapEdge* OceanThreshold, verticalMapEdge));

            // FIXME: we probably shouldn't need to traverse all of these over and over again
            foreach (Center center in centers)
            {
                if (IsOceanTile(center.Position)) {
                    Console.WriteLine(String.Format("Setting tile to ocean pos(x/y): {0} / {1}", center.Position.x, center.Position.y));

                    center.Water = true;
                    center.Ocean = true;
                }
            }
        }
    }
}
