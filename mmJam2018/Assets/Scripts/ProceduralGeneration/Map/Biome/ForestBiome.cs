﻿using System.Collections.Generic;

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
            : base(seedBasedRng, tileLookup.GrassTilePrefab, new List<GameObject>{ tileLookup.OneGreenBushTilePrefab }, parent)
        {
            biomeType = BiomeType.ForestBiome;
            HasSpawned = false;

            treeSpawnProbability = (spawnData as ForestProbabilities).treeSpawn;
        }

        public override void SpawnMembers(Center tile)
        {
            if (HasSpawned) {
                return;
            }

            if (tile.HasNeighbourOfExBiomeType(typeof(ForestBiome))) {
                float increase = treeSpawnProbability / 2;
                treeSpawnProbability += increase;
            }

            if(randomNumberGen.Next(0, 100) < treeSpawnProbability)
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
