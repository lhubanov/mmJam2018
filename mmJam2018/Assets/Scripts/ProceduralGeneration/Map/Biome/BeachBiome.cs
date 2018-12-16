using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class BeachBiome : BiomeBase
    {
        public BeachBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent)
            : base(seedBasedRng, tileLookup.SandPrefab, null, parent)
        {
            HasSpawned = false;
            biomeType = BiomeType.BeachBiome;
        }

        public override void SpawnMembers(Center tile)
        {
            HasSpawned = true;
        }

        // FIXME:   This is generic and reusable- move to base class,
        //          alongside the sprite as a protected (property).
        //          Then just copy exact implementation and just set sprite
        //          in sub-biome.
        //          This returns a GameObject, so that eventual parent can be assigned
        //          in Map/RegionGenerator MonoBehaviour object.
        public override void SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(baseSprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            obj.transform.parent = parentGameObject;
        }
    }
}
