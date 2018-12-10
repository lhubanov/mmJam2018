using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class SwampBiome : IBiome
    {
        private System.Random rng;


        [SerializeField]
        private GameObject sprite;
        private GameObject spriteWithMembers;

        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        // Probabilities
        private float bushSpawnProbability = 40;

        public SwampBiome(System.Random seedBasedRng, TileLookup tileLookup)
        {
            rng = seedBasedRng;

            sprite = tileLookup.GrassTilePrefab;
            spriteWithMembers = tileLookup.OnePurpleBushTilePrefab;

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
                
                // FIXME:   rework so object is returned and assigned a parent,
                //          as now this will be spawned on the top-level of the hierarchy
                GameObject obj = Object.Instantiate(spriteWithMembers, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
                // return obj;

                HasSpawned = true;
                return true;
            }

            return false;
        }

        public GameObject SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(sprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            return obj;
        }
    }
}
