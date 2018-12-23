using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Map.MapSettings
{
    // Note: This is a bit basic in terms of the way its designed;
    //       Add interface and extend w/ Base, if different types of
    //       containers need to be used at some point;
    
    //       Maybe at least add constructor and make all private, so they can be set to defaults if invalid;
    //       Docu comments also.
    //       
    //       Separate into interfaces etc. also
    public class MapSettingsContainer
    {
        public Vector2 MapBotLeft;
        public Vector2 MapTopRight;

        public float TileSize;

        public float OceanThreshold;
        public float ChanceIslandTileIsWater;

        public double ElevationIncreaseMultiplier;

        public int MemberSpawningIterations;

        public float NoiseScale;
        public int MaxNoiseOffset;

        public string RngSeed;
        public TileLookup TileLookup;
        public BiomeConditions Conditions;
        public Transform ParentGameObject;

        public GRAPH_TRAVERSAL_METHOD GraphTraversalMethod;
    }
}
