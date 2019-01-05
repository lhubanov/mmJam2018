using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ProximityScanner : MonoBehaviour
{
    // FIXME: make these necessary with that unity attribute
    private SphereCollider          tileScanner;
    private PlayerAnimationManager  playerAnimationManager;
    private PlayerSFXManager        playerSFXManager;
    private HeldEnergyManager       heldEnergyManager;

    public float EnergyGainRateTile = 0.5f;
    public float EnergyGainRateEnemy = 2f;

    void Start()
    {
        tileScanner             = GetComponent<SphereCollider>();
        playerAnimationManager  = GetComponentInParent<PlayerAnimationManager>();
        playerSFXManager        = GetComponentInParent<PlayerSFXManager>();
        heldEnergyManager       = GetComponentInParent<HeldEnergyManager>();
    }
	
	void Update()
    {
        if (Input.GetButton("Fire1")) {
            DrainLife();
        }
    }

    private void DrainLife()
    {
        playerAnimationManager.AnimateLifeDrain();
        playerSFXManager.PlayLifeDrainSound();

        Collider[] objectsInProximity = GetAllObjectsInProximity();
        for(int i = 0; i < objectsInProximity.Length; i++)
        {
            if(objectsInProximity[i].GetComponent<Tile>() != null)
            { 
                switch (objectsInProximity[i].tag)
                {
                    case "Terrain":
                        objectsInProximity[i].GetComponent<Tile>().DrainTile();
                        heldEnergyManager.IncreaseHeldEnergy(EnergyGainRateTile);
                        break;
                    case "Enemy":
                        objectsInProximity[i].GetComponentInChildren<EnemyAnimationManager>().PlayDeathAnimation();
                        heldEnergyManager.IncreaseHeldEnergy(EnergyGainRateEnemy);
                        break;
                }
            }
        }
    }

    private Collider[] GetAllObjectsInProximity()
    {
        return Physics.OverlapSphere(tileScanner.transform.position, tileScanner.radius);
    }
}
