﻿using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class OceanBiome : BiomeBase
    {
        public OceanBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent)
            : base(seedBasedRng, tileLookup.WaterPrefab, null, parent)
        {
            HasSpawned = false;
            biomeType = BiomeType.OceanBiome;
        }

        public override void SpawnMembers(Center tile)
        {
            HasSpawned = true;
        }
    }
}
