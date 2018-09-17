using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Dead Tile Lookup Table")]
    public class DeadTileLookup : ScriptableObject
    {
        // Resource lookup table; probably a more optimal way to do this
        public Dictionary<string, string> deadSprites = new Dictionary<string, string>()
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
