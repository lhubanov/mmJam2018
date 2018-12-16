using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public abstract class BiomeBase : IBiome
    {
        protected System.Random randomNumberGen;
        protected TileLookup tileLookup;

        protected GameObject baseSprite;

        // FIXME: Change to some list of gameobjects(members) to spawn once I've reordered the prefabs
        protected GameObject spriteWithMembers;
        protected Transform parentGameObject;

        public BiomeType biomeType { get; protected set; }
        public bool HasSpawned { get; protected set; }

        public BiomeBase(System.Random rng,
                        GameObject sprite, 
                        GameObject spriteWMembers,
                        Transform parent)
        {
            randomNumberGen = rng;

            baseSprite = sprite;
            spriteWithMembers = spriteWMembers;

            parentGameObject = parent;
        }

        public abstract void SpawnSprite(Center tile); 
        public abstract void SpawnMembers(Center tile);

    }
}
