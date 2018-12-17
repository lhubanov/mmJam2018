using System.Collections.Generic;
using ProceduralGeneration.Biome;

using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName =  "")]
    public class BiomeConditions : ScriptableObject
    {
        public Dictionary<int, BiomeType> Elevations = new Dictionary<int, BiomeType>()
        {
            { 3, BiomeType.BeachBiome},
            { 10, BiomeType.GrasslandBiome},
            { 20, BiomeType.ForestBiome},
            { 200, BiomeType.SwampBiome}, // lol somehow this is the tallest biome
        };

        // Note: All the probabilities can be held here btw- makes it hella easy to change on the fly
        //       But need a bit more refactoring to be carry the reference
    }
}
