using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class MarshBiome : BiomeBase
    {
        private float bushSpawnProbability;

        public MarshBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent, IContainSpawnData spawnData)
            : base(seedBasedRng, 
                  tileLookup.WaterPrefab, 
                  new List<GameObject> {
                      tileLookup.SingleGreenBushPrefab,
                      tileLookup.GreenBushGroupPrefab
                  }, 
                  parent)
        {
            HasSpawned = false;

            bushSpawnProbability = (spawnData as MarshProbabilities).bushSpawn;
        }

        public override void SpawnMembers(Center tile)
        {
            if (HasSpawned) {
                return;
            }

            if (tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome)))
            {
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
