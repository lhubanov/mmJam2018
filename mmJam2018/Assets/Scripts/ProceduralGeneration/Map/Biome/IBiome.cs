using UnityEngine;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public interface IBiome
    {
        bool HasSpawned { get; }

        void SpawnMembers(Center tile);
        void SpawnSprite(Center tile);
    }
}
