﻿using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class BeachBiome : BiomeBase
    {
        public BeachBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent, IContainSpawnData spawnData)
            : base(seedBasedRng, tileLookup.SandPrefab, null, parent)
        {
            HasSpawned = false;
        }

        public override void SpawnMembers(Center tile)
        {
            HasSpawned = true;
        }
    }
}
