using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class GrasslandBiome : BiomeBase
    {
        // Probabilities
        private float memberSpawnProbability;

        public GrasslandBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent, IContainSpawnData spawnData)
            : base(seedBasedRng, 
                  tileLookup.GrassTilePrefab, 
                  new List<GameObject> {
                      tileLookup.FullColumnPrefab,
                      tileLookup.HalfColumnPrefab,
                      tileLookup.SingleStonePrefab,
                      tileLookup.CrackedStonePrefab,
                      tileLookup.SingleFlowerPrefab,
                      tileLookup.FlowerFarmPrefab,
                      tileLookup.FlowerPatchPrefab,
                      tileLookup.SingleGreenBushPrefab,
                      tileLookup.GreenBushGroupPrefab
                  }, 
                  parent)
        {
            HasSpawned = false;

            memberSpawnProbability = (spawnData as GrasslandProbabilities).memberSpawn;
        }

        public override void SpawnMembers(Center tile)
        {
            if(HasSpawned) {
                return;
            }

            if(tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome))) {
                float increase = memberSpawnProbability / 2;
                memberSpawnProbability += increase;
            }

            if(randomNumberGen.Next(0,100) < memberSpawnProbability)
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
