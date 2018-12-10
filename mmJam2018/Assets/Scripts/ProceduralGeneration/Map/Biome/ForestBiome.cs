using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class ForestBiome : IBiome
    {
        private System.Random rng;

        [SerializeField]
        private GameObject sprite;

        // FIXME: This is due to the stupid way I've made the prefabs atm.
        //        Should actually just be references to the spawnable members (columns/ruins)
        //        not the whole tile.
        private GameObject spriteWithMembers;


        public BiomeType biomeType { get; private set; }
        public bool HasSpawned { get; private set; }

        // Probabilities

        //Range[0,1]
        //private float treeSpawnProbability = 0.7f;

        //Range[0,100]
        private float treeSpawnProbability = 25;

        public ForestBiome(System.Random seedBasedRng, TileLookup tileLookup)
        {
            rng = seedBasedRng;
            sprite = tileLookup.GrassTilePrefab;
            spriteWithMembers = tileLookup.OneGreenBushTilePrefab;

            biomeType = BiomeType.ForestBiome;
            HasSpawned = false;
        }

        // FIXME:   In all of these cases, use the unity Random.Range and get a value between 0 and 1
        //          Although, maybe slower using floats instead of 0-100 vals?

        //          This usage of Center can be cleaned up (and maybe redesigned around after)
        public bool SpawnMembers(Center tile)
        {
            if (HasSpawned) {
                return true;
            }

            // TODO: This is copy-pasted around, but generifying this check
            //       seems difficult currently.
            if (tile.HasNeighbourOfExBiomeType(typeof(ForestBiome)))
            {
                float increase = treeSpawnProbability / 2;
                treeSpawnProbability += increase;
            }

            if(rng.Next(0, 100) < treeSpawnProbability)
            {
                biomeType = BiomeType.ForestWithSpawnedTrees;

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
