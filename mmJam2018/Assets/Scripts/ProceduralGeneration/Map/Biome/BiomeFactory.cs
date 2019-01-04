using System.Linq;
using UnityEngine;
using Assets.Scripts;

namespace ProceduralGeneration.Biome
{
    public enum BiomeType
    {
        None,
        ForestBiome,
        GrasslandBiome,
        RosepatchBiome,
        BeachBiome,
        OceanBiome,
        MarshBiome
    }

    public class BiomeFactory
    {
        private TileLookup tileLookup;
        private System.Random randomNumberGen;
        private BiomeConditions conditions;
        private Transform parentGameObject;

        public BiomeFactory(System.Random rng, TileLookup lookup, BiomeConditions biomeConditions, Transform parent)
        {
            randomNumberGen = rng;
            tileLookup = lookup;
            conditions = biomeConditions;
            parentGameObject = parent;
        }

        public IBiome CreateBiome(BiomeType biomeType)
        {
            switch(biomeType)
            {
                case BiomeType.ForestBiome:
                    return new ForestBiome(randomNumberGen, tileLookup, parentGameObject, conditions.ForestProbabilities);
                case BiomeType.BeachBiome:
                    return new BeachBiome(randomNumberGen, tileLookup, parentGameObject, null);
                case BiomeType.GrasslandBiome:
                    return new GrasslandBiome(randomNumberGen, tileLookup, parentGameObject, conditions.GrasslandProbabilities);
                case BiomeType.RosepatchBiome:
                    return new RosepatchBiome(randomNumberGen, tileLookup, parentGameObject, conditions.RosepatchProbabilities);
                case BiomeType.OceanBiome:
                    return new OceanBiome(randomNumberGen, tileLookup, parentGameObject, null);
                case BiomeType.MarshBiome:
                    return new MarshBiome(randomNumberGen, tileLookup, parentGameObject, conditions.MarshProbabilities);

                default:
                    return new Biome(randomNumberGen, tileLookup, parentGameObject, null);
            }
        }

        public IBiome CreateBiomeFromElevation(double elevation)
        {
            BiomeType type = conditions.Elevations.Where(x => x.Key > elevation).First().Value;
            return CreateBiome(type);
        }
    }
}
