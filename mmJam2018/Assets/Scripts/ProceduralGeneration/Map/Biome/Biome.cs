using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    // Equivalent to BiomeType.None
    public class Biome : BiomeBase
    {
        public Biome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent, IContainSpawnData spawnData)
            : base(seedBasedRng, null, null, parent)
        {
            HasSpawned = false;
        }

        public override void SpawnMembers(Center tile)
        {
            HasSpawned = true;
        }

        public override void SpawnSprite(Center tile)
        {
            // * crickets *
        }
    }
}
