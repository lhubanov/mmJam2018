using System;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class SwampBiome : IBiome
    {
        private System.Random rng;

        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        // Probabilities
        private float bushSpawnProbability = 40;

        public SwampBiome(System.Random seedBasedRng)
        {
            rng = seedBasedRng;
            biomeType = BiomeType.SwampBiome;
            HasSpawned = false;
        }

        public bool SpawnMembers(Center tile)
        {
            if (HasSpawned) {
                return true;
            }

            if (tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome)))
            {
                float increase = bushSpawnProbability / 2;
                bushSpawnProbability += increase;
            }

            if (rng.Next(0, 100) < bushSpawnProbability)
            {
                biomeType = BiomeType.SwampWithBushes;
                HasSpawned = true;
                return true;
            }

            return false;
        }

    }
}
