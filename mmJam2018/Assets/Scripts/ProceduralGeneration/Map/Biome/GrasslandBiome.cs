using UnityEngine;
using Assets.Scripts;
using ProceduralGeneration.Map;

namespace ProceduralGeneration.Biome
{
    public class GrasslandBiome : BiomeBase
    {
        // Probabilities
        private float ruinSpawnProbability = 15;

        public GrasslandBiome(System.Random seedBasedRng, TileLookup tileLookup, Transform parent)
            : base(seedBasedRng, tileLookup.GrassTilePrefab, tileLookup.ColumnTilePrefab, parent)
        {
            biomeType = BiomeType.GrasslandBiome;
            HasSpawned = false;
        }

        public override void SpawnMembers(Center tile)
        {
            if(HasSpawned) {
                return;
            }

            if(tile.HasNeighbourOfExBiomeType(typeof(GrasslandBiome))) {
                float increase = ruinSpawnProbability / 2;
                ruinSpawnProbability += increase;
            }

            if(randomNumberGen.Next(0,100) < ruinSpawnProbability) {
                GameObject obj = Object.Instantiate(spriteWithMembers, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
                obj.transform.parent = parentGameObject;

                HasSpawned = true;
            }
        }

        public override void SpawnSprite(Center tile)
        {
            GameObject obj = Object.Instantiate(baseSprite, new Vector3(tile.Position.x, tile.Position.y, 0), Quaternion.identity);
            obj.transform.parent = parentGameObject;
        }
    }
}
