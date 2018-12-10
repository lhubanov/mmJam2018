using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class GrasslandBiome : IBiome
    {
        protected System.Random rng;

        [SerializeField]
        private GameObject sprite;

        // FIXME: This is due to the stupid way I've made the prefabs atm.
        //        Should actually just be references to the spawnable members (columns/ruins)
        //        not the whole tile.
        private GameObject spriteWithMembers;

        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        // Probabilities
        private float ruinSpawnProbability = 15;

        public GrasslandBiome(System.Random seedBasedRng, TileLookup tileLookup)
        {
            rng = seedBasedRng;
            sprite = tileLookup.GrassTilePrefab;
            spriteWithMembers = tileLookup.ColumnTilePrefab;

            biomeType = BiomeType.GrasslandBiome;
            HasSpawned = false;
        }

        public bool SpawnMembers(Center tile)
        {
            if(HasSpawned) {
                return true;
            }

            if(tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome))) {
                float increase = ruinSpawnProbability / 2;
                ruinSpawnProbability += increase;
            }

            if(rng.Next(0,100) < ruinSpawnProbability) {
                biomeType = BiomeType.GrasslandWithRuins;

                // FIXME:   rework so object is returned and assigned a parent,
                //          as now this will be spawned on the top-level of the hierarchy
                GameObject obj = Object.Instantiate(spriteWithMembers, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
                // return obj;

                HasSpawned = true;
                return true;
            }

            return false;
        }

        public GameObject SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(sprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            return obj;
        }
    }
}
