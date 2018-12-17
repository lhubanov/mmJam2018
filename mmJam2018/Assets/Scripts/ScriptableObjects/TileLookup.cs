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
        public GameObject ColumnTilePrefab;
        public GameObject OneGreenBushTilePrefab;
        public GameObject OnePurpleBushTilePrefab;
        public GameObject SeveralPurpleBushTilePrefab;
        public GameObject TreePrefab;

        // Resource lookup table; probably a more optimal way to do this
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
