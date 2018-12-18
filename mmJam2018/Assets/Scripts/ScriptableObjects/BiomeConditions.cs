using System.Collections.Generic;
using ProceduralGeneration.Biome;

using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName =  "")]
    public class BiomeConditions : ScriptableObject
    {
        // Elevations as editor-settable members
        [SerializeField]
        private int beachElevation; //3

        [SerializeField]
        private int grasslandElevation; //10

        [SerializeField]
        private int forestElevation; //20

        [SerializeField]
        private int swampElevation; //200

        public Dictionary<int, BiomeType> Elevations;


        public void Awake()
        {
            Elevations = new Dictionary<int, BiomeType>();
            Elevations.Add(beachElevation, BiomeType.BeachBiome);
            Elevations.Add(grasslandElevation, BiomeType.GrasslandBiome);
            Elevations.Add(forestElevation, BiomeType.ForestBiome);
            Elevations.Add(swampElevation, BiomeType.SwampBiome);
        }

        // Note: All the probabilities can be held here btw- makes it hella easy to change on the fly
        //       But need a bit more refactoring to be carry the reference
    }
}
