using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class OceanBiome : IBiome
    {
        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        public OceanBiome()
        {
            HasSpawned = false;
            biomeType = BiomeType.Ocean;
        }

        public bool SpawnMembers(Center tile)
        {
            HasSpawned = true;
            return true;
        }
    }
}
