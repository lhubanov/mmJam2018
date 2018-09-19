using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ProximityScanner : MonoBehaviour
{
    private SphereCollider tileScanner = null;

    void Start()
    {
        tileScanner = GetComponent<SphereCollider>();
    }
	
	void Update()
    {
        if (Input.GetButton("Fire1")) {
            DrainLife();
        }
    }

    // This can include enemies as well
    private void DrainLife()
    {
        GetComponentInParent<PlayerAnimationManager>().AnimateLifeDrain();
        GetComponentInParent<PlayerSFXManager>().PlayLifeDrainSound();

        Collider[] objectsInProximity = GetAllObjectsInProximity();
        for(int i = 0; i < objectsInProximity.Length; i++)
        {
            switch (objectsInProximity[i].tag)
            {
                case "Terrain":
                    objectsInProximity[i].GetComponent<Tile>().DrainTile();
                    break;
                case "Enemy":
                    objectsInProximity[i].GetComponentInChildren<EnemyAnimationManager>().PlayDeathAnimation();
                    break;
            }
        }
    }

    private Collider[] GetAllObjectsInProximity()
    {
        return Physics.OverlapSphere(tileScanner.transform.position, tileScanner.radius);
    }

    private int GetResourceID(string spriteName)
    {
        char resourceID = spriteName[spriteName.Length - 1];
        return (char.GetNumericValue(resourceID) != -1) ? (int)char.GetNumericValue(resourceID) : 0;
    }
}
