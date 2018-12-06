using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class OceanBiome : IBiome
    {
        private System.Random rng;

        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        public OceanBiome(System.Random seedBasedRng)
        {
            rng = seedBasedRng;

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
