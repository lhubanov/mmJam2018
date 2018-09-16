using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScanner : MonoBehaviour
{
    private SphereCollider tileScanner = null;

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
        Collider[] tiles = GetAllTiles();
        
        for(int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].gameObject.GetComponent<Tile>() != null) {
                tiles[i].gameObject.GetComponent<Tile>().DrainTile();
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
