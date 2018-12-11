using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

using ProceduralGeneration.Map;

public class RegionGenerator : MonoBehaviour
{
    public Transform    RegionTopLeft;
    public Transform    RegionBotRight;

    public TileLookup   TileLookup;

    // Does this show up in Unity?
    public bool         UseRandomSeed;
    public string       Seed;

    // Note: This assumes a tile is square;
    [SerializeField]
    private float tileSize;

    [SerializeField]
    private int mapWidth;

    [SerializeField]
    private int mapHeight;


    private void Start()
    {
        GenerateRegion();
	}

    private void GenerateRegion()
    {
        if (UseRandomSeed) {
            Seed = Random.Range(0, 1000).ToString();
        }

        // FIXME: Change this to Unity RNG
        System.Random randomNumberGenerator = new System.Random(Seed.GetHashCode());


        Map map = new Map(tileSize, Seed, RegionTopLeft.position, RegionBotRight.position, TileLookup);
        map.Generate();


        //for (float x = RegionTopLeft.position.x; x < RegionBotRight.position.x; x += SizeOfTile)
        //{
        //    for(float y = RegionTopLeft.position.y; y > RegionBotRight.position.y; y -= SizeOfTile)
        //    {
        //        GameObject prefabToCreate = TileLookup.GrassTilePrefab;
        //        Vector3 pos = new Vector3(x, y, 0);

        //        if (IsAtRegionEdge(pos)) {
        //            prefabToCreate = TileLookup.SeveralPurpleBushTilePrefab;
        //        }

        //        // Make these editor-manipulatble constants, if necessary
        //        else if (randomNumberGenerator.Next(0,100) < FillPercent) {
        //            prefabToCreate = TileLookup.SeveralPurpleBushTilePrefab;
        //        }

        //        GameObject obj = Instantiate(prefabToCreate, pos, Quaternion.identity);
        //        obj.transform.parent = this.transform;
        //    }
        //}
    }
}
