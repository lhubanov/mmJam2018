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

        protected List<GameObject> members;
        protected Transform parentGameObject;
        protected Transform tileGameObject;

        public bool HasSpawned { get; protected set; }

        protected BiomeBase(System.Random rng,
                        GameObject sprite, 
                        List<GameObject> spriteWMembers,
                        Transform parent)
        {
            randomNumberGen = rng;

            baseSprite = sprite;
            members = spriteWMembers;

            parentGameObject = parent;
        }

        public virtual void SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(baseSprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            obj.transform.parent = parentGameObject;
            tileGameObject = obj.transform;
        } 

        public abstract void SpawnMembers(Center tile);
    }
}
