using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class BeachBiome : IBiome
    {
        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        public BeachBiome()
        {
            HasSpawned = false;
            biomeType = BiomeType.BeachBiome;
        }

        public bool SpawnMembers(Center tile)
        {
            HasSpawned = true;
            return true;
        }
    }
}
