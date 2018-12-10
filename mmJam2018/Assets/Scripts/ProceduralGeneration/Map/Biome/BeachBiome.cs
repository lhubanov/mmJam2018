using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class BeachBiome : IBiome
    {
        private System.Random rng;

        // TODO:    This is statically asigned in constructor atm.
        //          Might be better, to expose in Unity Editor and
        //          assign from there.
        [SerializeField]
        private GameObject sprite;

        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        public BeachBiome(System.Random seedBasedRng, TileLookup tileLookup)
        {
            rng = seedBasedRng;
            sprite = tileLookup.SandPrefab;

            HasSpawned = false;
            biomeType = BiomeType.BeachBiome;
        }

        public bool SpawnMembers(Center tile)
        {
            HasSpawned = true;
            return true;
        }

        // FIXME:   This is generic and reusable- move to base class,
        //          alongside the sprite as a protected (property).
        //          Then just copy exact implementation and just set sprite
        //          in sub-biome.
        //          This returns a GameObject, so that eventual parent can be assigned
        //          in Map/RegionGenerator MonoBehaviour object.
        public GameObject SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(sprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            return obj;
        }
    }
}
