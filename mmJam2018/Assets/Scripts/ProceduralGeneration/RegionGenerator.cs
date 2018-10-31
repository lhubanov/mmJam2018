using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class RegionGenerator : MonoBehaviour
{
    public Transform    RegionTopLeft;
    public Transform    RegionBotRight;

    // This assumes a tile is square - introduce two constants where it isn't
    public float        SizeOfTile;

    public string       Seed;

    [Range(0,100)]
    public int          FillPercent;

    public TileLookup   TileLookup;
    public bool         UseRandomSeed;

    
	private void Start ()
    {
        GenerateRegion();
	}

    private void GenerateRegion()
    {
        if (UseRandomSeed) {
            Seed = Random.Range(0, 1000).ToString();
        }

        System.Random randomNumberGenerator = new System.Random(Seed.GetHashCode());

        for (float x = RegionTopLeft.position.x; x < RegionBotRight.position.x; x += SizeOfTile)
        {
            for(float y = RegionTopLeft.position.y; y > RegionBotRight.position.y; y -= SizeOfTile)
            {
                GameObject prefabToCreate = TileLookup.GrassTilePrefab;
                Vector3 pos = new Vector3(x, y, 0);

                if (IsAtRegionEdge(pos)) {
                    prefabToCreate = TileLookup.SeveralPurpleBushTilePrefab;
                }

                // Make these editor-manipulatble constants, if necessary
                else if (randomNumberGenerator.Next(0,100) < FillPercent) {
                    prefabToCreate = TileLookup.SeveralPurpleBushTilePrefab;
                }

                GameObject obj = Instantiate(prefabToCreate, pos, Quaternion.identity);
                obj.transform.parent = this.transform;
            }
        }
    }



    // Utility methods for basic limit checking
    private bool IsAtRegionEdge(Vector3 position)
    {
        return (IsAtXEdge(position.x) || IsAtYEdge(position.y));
    }

    private bool IsAtXEdge(float x)
    {
        return (x == RegionTopLeft.position.x || x == RegionBotRight.position.x - SizeOfTile);
    }

    private bool IsAtYEdge(float y)
    {
        return (y == RegionTopLeft.position.y || y == RegionBotRight.position.y + SizeOfTile);
    }
}
