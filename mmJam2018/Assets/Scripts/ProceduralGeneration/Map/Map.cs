using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Node;

namespace ProceduralGeneration.Map
{
    // Note: Expose the serialized settings below to editor
    public class Map : Graph.Graph
    {
        // Map size info
        private readonly float mapWidth;
        private readonly float mapHeight;

        [SerializeField]
        private int tileSize = 10;

        private readonly Vector2 mapBotLeft;
        private readonly Vector2 mapTopRight;


        [Range(0, 1)]
        [SerializeField]
        public float OceanThreshold = 0.85f;

        [SerializeField]
        private double elevationIncreaseRate;

        [SerializeField]
        private double elevationIncreaseMultiplier = 7.5;

        [SerializeField]
        private int spawningIterations = 2;

        [SerializeField]
        [Range(0, 1)]
        private float waterSpawnProbability = 0.5f;

        [SerializeField]
        private float noiseScale = 1f;

        [SerializeField]
        private int maxNoiseOffset = 100;

        // some sort of noise map(?)
        // e.g. from C++ source:
        //noise::module::Perlin *noiseMap;

        [SerializeField]
        private string rngSeed;
        private System.Random rng;

        private TileLookup tileLookup;

        public Map(int tilesize, string seed, Vector3 regionTopLeft, Vector3 regionBotRight, TileLookup lookup)
        {
            // FIXME:   If this strange conversion needs to happen, probably need to
            //          either just change the input Vector3s so can just be assigned
            //          or extend map edges to a more intellingent structure/class (overkill maybe?)
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
            string fullName = "ProceduralGeneration.Biome." + name.ToString();

            System.Object obj = assembly.CreateInstance(fullName);
            Biome.IBiome b = obj as Biome.IBiome;

            return b;
        }


        // FIXME: The map limit stuff here is somewhat unreadible, consider refactoring somehow
        private bool IsOnMapEdge(INode node)
        {
            return (node.Position.x == mapBotLeft.x || node.Position.x == mapTopRight.x) || (node.Position.y == mapBotLeft.y || node.Position.y == mapTopRight.y);
        }

        private bool IsValidPosition(Vector2 pos)
        {
            return (pos.x >= mapBotLeft.x && pos.x < mapTopRight.x) && (pos.y >= mapBotLeft.y && pos.y < mapTopRight.y);
        }

        // FIXME: I'm not sure this will work actually
        private bool IsOceanTile(Vector2 pos)
        {
            //return (pos.x > horizontalMapEdge * OceanThreshold || pos.x < horizontalMapEdge * (1 - OceanThreshold))
            //    || (pos.y > horizontalMapEdge * OceanThreshold || pos.y < horizontalMapEdge * (1 - OceanThreshold));

            var xMaxThreshold = mapTopRight.x * OceanThreshold;
            var xMinThreshold = mapBotLeft.x + (mapWidth * (1 - OceanThreshold));

            var yMaxThreshold = mapTopRight.y * OceanThreshold;
            var yMinThreshold = mapBotLeft.y + (mapHeight * (1 - OceanThreshold));

            return (pos.x > xMaxThreshold || pos.x < xMinThreshold)
                || (pos.y > yMaxThreshold || pos.y < yMinThreshold);
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
                x = mapBotLeft.x;

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
                    x += tileSize;
                }

                nodesMap.Add(row);
                y += tileSize;
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


        // Returns Island tiles
        private HashSet<Center> GenerateOcean(HashSet<Center> centers)
        {
            Debug.Log(String.Format("Generating ocean. Ocean Threshold X: lowerRange {0} - {1}", mapBotLeft.x, mapTopRight.x * (1 - OceanThreshold)));
            Debug.Log(String.Format("Generating ocean. Ocean Threshold X: upperRange {0} - {1}", mapTopRight.x * OceanThreshold, mapTopRight.x));
            Debug.Log(String.Format("Generating ocean. Ocean Threshold Y: lowerRange {0} - {1}", mapBotLeft.y, mapTopRight.y * (1 - OceanThreshold)));
            Debug.Log(String.Format("Generating ocean. Ocean Threshold Y: upperRange {0} - {1}", mapTopRight.y * OceanThreshold, mapTopRight.y));

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
            Debug.Log(String.Format("Generating island. Island Range X: {0} - {1}", mapTopRight.x * (1 - OceanThreshold), mapTopRight.x * OceanThreshold));
            Debug.Log(String.Format("Generating island. Island Range Y: {0} - {1}", mapTopRight.y * (1 - OceanThreshold), mapTopRight.y * OceanThreshold));

            foreach (Center center in islandTiles)
            {
                center.Water = (GetNoiseValueBasedOnPosition(center.Position.x, center.Position.y)) < 0.5f;
            }
        }

        // FIXME: This needs testing when offset works best and how big of an offset to use
        private float GetNoiseValueBasedOnPosition(float x, float y)
        {
            float offsetX = x + rng.Next(0, maxNoiseOffset);
            float offsetY = y + rng.Next(0, maxNoiseOffset);

            float xPos = (offsetX / (mapWidth / tileSize)) * noiseScale;
            float yPos = (offsetY / (mapHeight / tileSize)) * noiseScale;

            return Mathf.PerlinNoise(xPos, yPos);
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
            //SaveMapAsText(tiles, "intermediate.txt");

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


        //private void SaveMapAsText(HashSet<Center> map, string fileName = "demo.txt")
        //{
        //    string[,] table = new string[mapWidth / tileSize, mapHeight / tileSize];

        //    foreach (Center center in map)
        //    {
        //        string symbol;
        //        if (center.Biome.biomeType == Biome.BiomeType.Ocean)
        //        {
        //            symbol = @"*";
        //        }
        //        else
        //        {
        //            int val = (int)center.Biome.biomeType;
        //            symbol = val.ToString();
        //        }

        //        table[Convert.ToInt32(center.Position.x), Convert.ToInt32(center.Position.y)] = symbol;
        //    }

        //    using (StreamWriter sw = new StreamWriter(fileName, false, System.Text.Encoding.UTF8))
        //    {
        //        for (int r = 0; r < mapWidth / tileSize; r++)
        //        {
        //            for (int c = 0; c < mapHeight / tileSize; c++)
        //            {
        //                sw.Write(table[r, c]);
        //            }

        //            sw.Write(Environment.NewLine);
        //        }
        //    }

        //    Console.WriteLine("Map saved as text...");
        //}
    }
}
