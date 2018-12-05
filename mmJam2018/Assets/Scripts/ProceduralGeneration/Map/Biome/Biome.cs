using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public enum BiomeType
    {
        None,
        ForestBiome,
        ForestWithSpawnedTrees,
        GrasslandBiome,
        GrasslandWithRuins,
        SwampBiome,
        SwampWithBushes,
        BeachBiome,
        Ocean,
    }

    // Equivalent to BiomeType.None
    public class Biome : IBiome
    {
        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        public Biome()
        {
            HasSpawned = false;
            biomeType = BiomeType.None;
        }

        public bool SpawnMembers(Center tile)
        {
            HasSpawned = true;
            return true;
        }
    }
}
