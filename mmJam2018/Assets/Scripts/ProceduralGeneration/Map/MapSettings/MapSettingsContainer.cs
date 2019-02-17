using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Graph;

namespace ProceduralGeneration.Map.MapSettings
{

    public class MapSettingsContainer
    {
        public Vector2 MapBotLeft { get; private set; }
        public Vector2 MapTopRight { get; private set; }

        public float TileSize { get; private set; }

        public float OceanThreshold { get; private set; }
        public float ChanceIslandTileIsWater { get; private set; }

        public double ElevationIncreaseRate { get; private set; }

        public int MemberSpawningIterations { get; private set; }

        public float NoiseScale { get; private set; }
        public int MaxNoiseOffset { get; private set; }

        public string RngSeed { get; private set; }
        public TileLookup TileLookup { get; private set; }
        public BiomeConditions Conditions { get; private set; }
        public Transform ParentGameObject { get; private set; }

        public GRAPH_TRAVERSAL_METHOD GraphTraversalMethod;

        public MapSettingsContainer()
        {
            MapBotLeft = new Vector2(0, 0);
            MapTopRight = new Vector2(0, 0);
            TileSize = 0;
            OceanThreshold = 0;
            ChanceIslandTileIsWater = 0;
            ElevationIncreaseRate = 0;
            MemberSpawningIterations = 0;
            NoiseScale = 0;
            MaxNoiseOffset = 0;
            RngSeed = "";
            TileLookup = null;
            Conditions = null;
            ParentGameObject = null;
            GraphTraversalMethod = GRAPH_TRAVERSAL_METHOD.BFS;
        }

        public MapSettingsContainer(
            Vector2 mapBotLeft,
            Vector2 mapTopRight,
            float tileSize,
            float oceanThreshold,
            float chanceIslandTileIsWater,
            double elevationIncreaseRate,
            int memberSpawningIterations,
            float noiseScale,
            int maxNoiseOffset,
            string rngSeed,
            TileLookup tileLookup,
            BiomeConditions biomeConditions,
            Transform parent,
            GRAPH_TRAVERSAL_METHOD graphTraversalMethod
            )
        {
            MapBotLeft = mapBotLeft;
            MapTopRight = mapTopRight;
            TileSize = tileSize;
            OceanThreshold = oceanThreshold;
            ChanceIslandTileIsWater = chanceIslandTileIsWater;
            ElevationIncreaseRate = elevationIncreaseRate;
            MemberSpawningIterations = memberSpawningIterations;
            NoiseScale = noiseScale;
            MaxNoiseOffset = maxNoiseOffset;
            RngSeed = rngSeed;
            TileLookup = tileLookup;
            Conditions = biomeConditions;
            ParentGameObject = parent;
            GraphTraversalMethod = graphTraversalMethod;
        }
    }
}
