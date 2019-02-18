using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class ForestBiome : BiomeBase
    {
        // Probabilities
        [Range(0, 100)]
        private float treeSpawnProbability;

        public ForestBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent, IContainSpawnData spawnData)
            : base(seedBasedRng, 
                  tileLookup.GrassTilePrefab, 
                  new List<GameObject>{
                      tileLookup.TreePrefab,
                      tileLookup.SingleGreenBushPrefab,
                      tileLookup.GreenBushGroupPrefab,
                  }, 
                  parent)
        {
            HasSpawned = false;

            treeSpawnProbability = (spawnData as ForestProbabilities).GetTreeSpawn();
        }

        public override void SpawnMembers(Center tile)
        {
            if (HasSpawned) {
                return;
            }

            if (tile.HasNeighbourWithSpawnedMembers(typeof(ForestBiome))) {
                float increase = treeSpawnProbability / 2;
                treeSpawnProbability += increase;
            }

            if(randomNumberGen.Next(0, 100) < treeSpawnProbability)
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
