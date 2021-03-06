﻿using System.Collections.Generic;
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
        private int RosepatchElevation; //200

        public Dictionary<int, BiomeType> Elevations;

        public ForestProbabilities      ForestProbabilities;
        public GrasslandProbabilities   GrasslandProbabilities;
        public RosepatchProbabilities   RosepatchProbabilities;
        public MarshProbabilities       MarshProbabilities;


        public void OnEnable()
        {
            Elevations = new Dictionary<int, BiomeType>();
            Elevations.Add(beachElevation, BiomeType.BeachBiome);
            Elevations.Add(grasslandElevation, BiomeType.GrasslandBiome);
            Elevations.Add(forestElevation, BiomeType.ForestBiome);
            Elevations.Add(RosepatchElevation, BiomeType.RosepatchBiome);

            ForestProbabilities     = new ForestProbabilities();
            GrasslandProbabilities  = new GrasslandProbabilities();
            RosepatchProbabilities  = new RosepatchProbabilities();
            MarshProbabilities      = new MarshProbabilities();
        }
    }


    // Literally for the sake of encapsulation
    [System.Serializable]
    public class ForestProbabilities : IContainSpawnData
    {
        [Range(0, 100)]
        [SerializeField]
        private float treeSpawn = 20;

        // Note: Unity does not serialize C# properties, hence these
        public float GetTreeSpawn() { return treeSpawn; }
    }

    [System.Serializable]
    public class GrasslandProbabilities : IContainSpawnData
    {
        [Range(0, 100)]
        [SerializeField]
        private float memberSpawn = 15;

        public float GetMemberSpawn() { return memberSpawn; }
    }

    [System.Serializable]
    public class RosepatchProbabilities : IContainSpawnData
    {
        [Range(0, 100)]
        [SerializeField]
        private float bushSpawn = 30;

        public float GetBushSpawn() { return bushSpawn; }
    }

    [System.Serializable]
    public class MarshProbabilities : IContainSpawnData
    {
        [Range(0, 100)]
        [SerializeField]
        private float bushSpawn = 15;

        public float GetBushSpawn() { return bushSpawn; }
    }
}
