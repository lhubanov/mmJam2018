using UnityEngine;
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

        public override void SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(baseSprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            obj.transform.parent = parentGameObject;
        }
    }
}
