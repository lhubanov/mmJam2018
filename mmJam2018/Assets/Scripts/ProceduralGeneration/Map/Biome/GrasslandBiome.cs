using System.Collections.Generic;

using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class GrasslandBiome : BiomeBase
    {
        // Probabilities
        private float ruinSpawnProbability;

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

            ruinSpawnProbability = (spawnData as GrasslandProbabilities).ruinSpawn;
        }

        public override void SpawnMembers(Center tile)
        {
            if(HasSpawned) {
                return;
            }

            if(tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome))) {
                float increase = ruinSpawnProbability / 2;
                ruinSpawnProbability += increase;
            }

            if(randomNumberGen.Next(0,100) < ruinSpawnProbability)
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
