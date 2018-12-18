using System.Collections.Generic;
using ProceduralGeneration.Biome;

using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName =  "BiomeConditions")]
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

        public ForestProbabilities forestProbabilities;
        public GrasslandProbabilities grasslandProbabilities;
        public SwampProbabilities swampProbabilities;


        public void Awake()
        {
            Elevations = new Dictionary<int, BiomeType>();
            Elevations.Add(beachElevation, BiomeType.BeachBiome);
            Elevations.Add(grasslandElevation, BiomeType.GrasslandBiome);
            Elevations.Add(forestElevation, BiomeType.ForestBiome);
            Elevations.Add(swampElevation, BiomeType.SwampBiome);

            forestProbabilities = new ForestProbabilities();
            grasslandProbabilities = new GrasslandProbabilities();
            swampProbabilities = new SwampProbabilities();
        }
    }


    // FIXME: Add constructors etc.

    // Literally for the sake of encapsulation
    // Note: These could contain spawnable members etc. also?
    //       Or should that be delegated to 
    [System.Serializable]
    public class ForestProbabilities : IContainSpawnData
    {
        // FIXME: Consolidate these to either 0-100 or 0-1
        [Range(0, 100)]
        public float treeSpawn = 25;
    }

    [System.Serializable]
    public class GrasslandProbabilities : IContainSpawnData
    {
        [Range(0, 100)]
        public float ruinSpawn = 15;
    }

    public class SwampProbabilities : IContainSpawnData
    {
        [Range(0, 100)]
        public float bushSpawn = 40;
    }
}
