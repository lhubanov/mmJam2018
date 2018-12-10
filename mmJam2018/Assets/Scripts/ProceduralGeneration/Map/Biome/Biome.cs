using UnityEngine;
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
        private System.Random rng;

        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        public Biome(System.Random seedBasedRng)
        {
            rng = seedBasedRng;

            HasSpawned = false;
            biomeType = BiomeType.None;
        }

        public bool SpawnMembers(Center tile)
        {
            HasSpawned = true;
            return true;
        }

        public GameObject SpawnSprite(Center tile)
        {
            //GameObject obj = Object.Instantiate(sprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            //return obj;
            return null;
        }
    }
}
