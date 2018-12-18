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
        SwampBiome,
        BeachBiome,
        OceanBiome,
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
                    return new ForestBiome(randomNumberGen, tileLookup, parentGameObject, conditions.forestProbabilities);
                case BiomeType.BeachBiome:
                    return new BeachBiome(randomNumberGen, tileLookup, parentGameObject, null);
                case BiomeType.GrasslandBiome:
                    return new GrasslandBiome(randomNumberGen, tileLookup, parentGameObject, conditions.grasslandProbabilities);
                case BiomeType.SwampBiome:
                    return new SwampBiome(randomNumberGen, tileLookup, parentGameObject, conditions.swampProbabilities);
                case BiomeType.OceanBiome:
                    return new OceanBiome(randomNumberGen, tileLookup, parentGameObject, null);

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
