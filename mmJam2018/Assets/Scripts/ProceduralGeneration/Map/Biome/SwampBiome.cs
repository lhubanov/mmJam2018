using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class SwampBiome : BiomeBase
    {
        // Probabilities
        private float bushSpawnProbability;

        public SwampBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent, IContainSpawnData spawnData)
            : base(seedBasedRng, tileLookup.GrassTilePrefab, tileLookup.OnePurpleBushTilePrefab, parent)
        {
            biomeType = BiomeType.SwampBiome;
            HasSpawned = false;

            bushSpawnProbability = (spawnData as SwampProbabilities).bushSpawn;
        }

        public override void SpawnMembers(Center tile)
        {
            if (HasSpawned) {
                return;
            }

            if (tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome))) {
                float increase = bushSpawnProbability / 2;
                bushSpawnProbability += increase;
            }

            if (randomNumberGen.Next(0, 100) < bushSpawnProbability) {
                GameObject obj = Object.Instantiate(spriteWithMembers, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
                obj.transform.parent = parentGameObject;
                HasSpawned = true;
            }
        }
    }
}
