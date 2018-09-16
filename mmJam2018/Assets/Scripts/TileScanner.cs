using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScanner : MonoBehaviour
{
    private SphereCollider tileScanner = null;

    //// Resource lookup table; probably a more optimal way to do this
    //private Dictionary<string, string> deadSprites = new Dictionary<string, string>()
    //{
    //    { "grass pattern", "grass pattern dead"},
    //    { "tree regular", "tree dead"},
    //    { "bushes_0", "bushes dead"},
    //    { "bushes_1", "bushes dead"},
    //    { "bushes_2", "bushes dead"},
    //    { "bushes_3", "bushes dead"},
    //    { "flowers_0", "flowers dead"},
    //    { "flowers_1", "flowers dead"},
    //    { "flowers_2", "flowers dead"},
    //    { "flowers_3", "flowers dead"}
    //};

	void Start ()
    {
        tileScanner = GetComponent<SphereCollider>();
	}
	
	void Update ()
    {
        if (Input.GetButton("Fire1")) {
            DrainLife();
        }
    }

    // This can include enemies as well
    private void DrainLife()
    {
        AnimateLifeDrain();

        // Get all colliders within tile scanner sphere trigger
        Collider[] tiles = GetAllTiles();
        
        for(int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].gameObject.GetComponent<Tile>() != null)
            {
                tiles[i].gameObject.GetComponent<Tile>().DrainTile();
                //List<string> spriteNames = tiles[i].gameObject.GetComponent<Tile>().GetSpriteNames();

                //foreach(string spriteName in spriteNames) { 
                //    string deadSprite;
                //    if (deadSprites.TryGetValue(spriteName, out deadSprite)) {
                //        tiles[i].gameObject.GetComponent<Tile>().KillTile(deadSprite, GetResourceID(spriteName));
                //    }
                //}

            }
        }
    }

    private Collider[] GetAllTiles()
    {
        return Physics.OverlapSphere(tileScanner.transform.position, tileScanner.radius);
    }

    private int GetResourceID(string spriteName)
    {
        char resourceID = spriteName[spriteName.Length - 1];
        return (char.GetNumericValue(resourceID) != -1) ? (int)char.GetNumericValue(resourceID) : 0;
    }

    private void AnimateLifeDrain()
    {
        // Add player animation code
    }
}
