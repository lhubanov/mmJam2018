using System;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class GrasslandBiome : IBiome
    {
        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        // Probabilities
        private float ruinSpawnProbability = 15;

        public GrasslandBiome()
        {
            biomeType = BiomeType.GrasslandBiome;
            HasSpawned = false;
        }

        public bool SpawnMembers(Center tile)
        {
            if(HasSpawned) {
                return true;
            }

            if(tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome))) {
                float increase = ruinSpawnProbability / 2;
                ruinSpawnProbability += increase;
            }

            Random r = new Random();
            if(r.Next(0,100) < ruinSpawnProbability) {
                biomeType = BiomeType.GrasslandWithRuins;
                HasSpawned = true;
                return true;
            }

            return false;
        }
    }
}
