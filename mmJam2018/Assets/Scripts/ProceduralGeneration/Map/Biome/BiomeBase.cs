using System.Collections.Generic;

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

        protected List<GameObject> Members;
        protected Transform parentGameObject;

        public BiomeType biomeType { get; protected set; }
        public bool HasSpawned { get; protected set; }

        protected BiomeBase(System.Random rng,
                        GameObject sprite, 
                        List<GameObject> spriteWMembers,
                        Transform parent)
        {
            randomNumberGen = rng;

            baseSprite = sprite;
            Members = spriteWMembers;

            parentGameObject = parent;
        }

        public virtual void SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(baseSprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            obj.transform.parent = parentGameObject;
        } 

        public abstract void SpawnMembers(Center tile);
    }
}
