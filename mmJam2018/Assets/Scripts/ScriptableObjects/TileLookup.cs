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

        // Dead tile Sprites
        // Note: Never got serializable generics to quite work properly in-editor
        //       so stuck with this for the time being. Will look into this again in the future.
        public Sprite DeadGrass;
        public Sprite DeadTree;
        public Sprite DeadBush1;
        public Sprite DeadBush2;
        public Sprite DeadBush3;
        public Sprite DeadBush4;
        public Sprite DeadFlower1;
        public Sprite DeadFlower2;
        public Sprite DeadFlower3;
        public Sprite DeadFlower4;

        // Dead sprite lookup table used by Tile.Die()
        [SerializeField]
        public Dictionary<string, Sprite> DeadSprites = new Dictionary<string, Sprite>() { };

        public void Initialize()
        {
            DeadSprites.Add("grass pattern_0", DeadGrass);
            DeadSprites.Add("tree regular", DeadTree);

            DeadSprites.Add("bushes_0", DeadBush1);
            DeadSprites.Add("bushes_1", DeadBush2);
            DeadSprites.Add("bushes_2", DeadBush3);
            DeadSprites.Add("bushes_3", DeadBush4);

            DeadSprites.Add("flowers_0", DeadFlower1);
            DeadSprites.Add("flowers_1", DeadFlower2);
            DeadSprites.Add("flowers_2", DeadFlower3);
            DeadSprites.Add("flowers_3", DeadFlower4);
        }
    }
}
