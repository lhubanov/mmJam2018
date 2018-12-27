using System.Collections.Generic;

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
            : base(seedBasedRng, 
                  tileLookup.GrassTilePrefab, 
                  new List<GameObject> {
                      tileLookup.SinglePurpleBushPrefab,
                      tileLookup.PurpleBushGroupPrefab
                  }, 
                  parent)
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

            if (randomNumberGen.Next(0, 100) < bushSpawnProbability)
            {
                // Randomly select member to spawn from list of members
                int member = randomNumberGen.Next(Members.Count);

                GameObject obj = Object.Instantiate(Members[member], new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
                obj.transform.parent = parentGameObject;
                HasSpawned = true;
            }
        }
    }
}
