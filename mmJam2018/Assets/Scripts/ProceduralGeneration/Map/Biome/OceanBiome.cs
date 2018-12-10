using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class OceanBiome : IBiome
    {
        private System.Random rng;

        [SerializeField]
        private GameObject sprite;

        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        public OceanBiome(System.Random seedBasedRng, TileLookup tileLookup)
        {
            rng = seedBasedRng;
            sprite = tileLookup.WaterPrefab;

            HasSpawned = false;
            biomeType = BiomeType.Ocean;
        }

        public bool SpawnMembers(Center tile)
        {
            HasSpawned = true;
            return true;
        }

        public GameObject SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(sprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            return obj;
        }
    }
}
