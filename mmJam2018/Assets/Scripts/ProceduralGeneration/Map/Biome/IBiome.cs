using UnityEngine;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public interface IBiome
    {
        BiomeType biomeType { get; }
        bool HasSpawned { get; }

        void SpawnMembers(Center tile);
        void SpawnSprite(Center tile);
    }
}
