using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class RosepatchBiome : BiomeBase
    {
        // Probabilities
        private float bushSpawnProbability;

        public RosepatchBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent, IContainSpawnData spawnData)
            : base(seedBasedRng, 
                  tileLookup.GrassTilePrefab, 
                  new List<GameObject> {
                      tileLookup.SinglePurpleBushPrefab,
                      tileLookup.PurpleBushGroupPrefab
                  }, 
                  parent)
        {
            HasSpawned = false;

            bushSpawnProbability = (spawnData as RosepatchProbabilities).GetBushSpawn();
        }

        public override void SpawnMembers(Center tile)
        {
            if (HasSpawned) {
                return;
            }

            if (tile.HasNeighbourWithSpawnedMembers(typeof(GrasslandBiome))) {
                float increase = bushSpawnProbability / 2;
                bushSpawnProbability += increase;
            }

            if (randomNumberGen.Next(0, 100) < bushSpawnProbability)
            {
                // Randomly select member to spawn from list of members
                int member = randomNumberGen.Next(members.Count);

                GameObject obj = Object.Instantiate(members[member], new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
                obj.transform.parent = tileGameObject;
                HasSpawned = true;
            }
        }
    }
}
