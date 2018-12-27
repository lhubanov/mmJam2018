using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Tile Lookup Table")]
    public class TileLookup : ScriptableObject
    {
        // Tile prefabs used for procedurally generated region
        public GameObject WaterPrefab;
        public GameObject SandPrefab;
        public GameObject GrassTilePrefab;

        public GameObject FullColumnPrefab;
        public GameObject HalfColumnPrefab;

        public GameObject SingleStonePrefab;
        public GameObject CrackedStonePrefab;

        public GameObject SingleGreenBushPrefab;
        public GameObject GreenBushGroupPrefab;

        public GameObject SinglePurpleBushPrefab;
        public GameObject PurpleBushGroupPrefab;

        public GameObject SingleFlowerPrefab;
        public GameObject FlowerPatchPrefab;
        public GameObject FlowerFarmPrefab;

        public GameObject TreePrefab;

        // Resource lookup table; 

        // FIXME:   I hate these string comparison operations- investigate
        //          if can be done by type somehow
        public Dictionary<string, string> DeadSprites = new Dictionary<string, string>()
        {
            { "grass pattern", "grass pattern dead"},
            { "tree regular", "tree dead"},
            { "bushes_0", "bushes dead"},
            { "bushes_1", "bushes dead"},
            { "bushes_2", "bushes dead"},
            { "bushes_3", "bushes dead"},
            { "flowers_0", "flowers dead"},
            { "flowers_1", "flowers dead"},
            { "flowers_2", "flowers dead"},
            { "flowers_3", "flowers dead"}
        };
    }
}
