using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.SerializableGenerics;

namespace Assets.Scripts
{
    // Unity does not serialize generics, so resorting to hacks, until a better solution is found
    [System.Serializable]
    public class DictOfStringAndSprite : SerializableDictionary<string, Sprite> { }

    [CreateAssetMenu(menuName = "Tile Lookup Table")]
    public class TileLookup : ScriptableObject
    {
        // Tile prefabs used for procedurally generated region
        public GameObject WaterPrefab;
        public GameObject MarshWaterPrefab;

        public GameObject SandPrefab;
        public GameObject GrassTilePrefab;

        public GameObject FullColumnPrefab;
        public GameObject HalfColumnPrefab;

        public GameObject SingleStonePrefab;
        public GameObject CrackedStonePrefab;

        public GameObject SingleGreenBushPrefab;
        public GameObject GreenBushGroupPrefab;

        public GameObject SingleMarshBushPrefab;
        public GameObject MarshBushGroupPrefab;

        public GameObject SinglePurpleBushPrefab;
        public GameObject PurpleBushGroupPrefab;

        public GameObject SingleFlowerPrefab;
        public GameObject FlowerPatchPrefab;
        public GameObject FlowerFarmPrefab;

        public GameObject TreePrefab;

        // Resource lookup table; 
        public DictOfStringAndSprite DeadSprites = new DictOfStringAndSprite();

        // Add string/Sprite pairs in editor later
        //{
        //    { "grass pattern_0", "grass pattern dead"},
        //    { "tree regular", "tree dead"},
        //    { "bushes_0", "bushes dead"},
        //    { "bushes_1", "bushes dead"},
        //    { "bushes_2", "bushes dead"},
        //    { "bushes_3", "bushes dead"},
        //    { "flowers_0", "flowers dead"},
        //    { "flowers_1", "flowers dead"},
        //    { "flowers_2", "flowers dead"},
        //    { "flowers_3", "flowers dead"}
        //};
    }
}
