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

        private Transform parentGameObject;

        public BiomeFactory(System.Random rng, TileLookup lookup, Transform parent)
        {
            randomNumberGen = rng;
            tileLookup = lookup;
            parentGameObject = parent;
        }

        public IBiome CreateBiome(BiomeType biomeType)
        {
            switch(biomeType)
            {
                case BiomeType.ForestBiome:
                    return new ForestBiome(randomNumberGen, tileLookup, parentGameObject);
                case BiomeType.BeachBiome:
                    return new BeachBiome(randomNumberGen, tileLookup, parentGameObject);
                case BiomeType.GrasslandBiome:
                    return new GrasslandBiome(randomNumberGen, tileLookup, parentGameObject);
                case BiomeType.SwampBiome:
                    return new SwampBiome(randomNumberGen, tileLookup, parentGameObject);
                case BiomeType.OceanBiome:
                    return new OceanBiome(randomNumberGen, tileLookup, parentGameObject);

                default:
                    return new Biome(randomNumberGen, tileLookup, parentGameObject);
            }
        }
    }
}
