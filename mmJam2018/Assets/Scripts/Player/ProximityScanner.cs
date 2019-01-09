using UnityEngine;
using Assets.Scripts;


public class ProximityScanner : MonoBehaviour
{
    private SphereCollider          tileScanner;
    private PlayerAnimationManager  playerAnimationManager;
    private PlayerSFXManager        playerSFXManager;
    private HeldEnergyManager       heldEnergyManager;

    //public float EnergyGainRateTile = 0.5f;
    //public float EnergyGainRateEnemy = 2f;

    void Start()
    {
        tileScanner             = GetComponent<SphereCollider>();
        playerAnimationManager  = GetComponentInParent<PlayerAnimationManager>();
        playerSFXManager        = GetComponentInParent<PlayerSFXManager>();
        heldEnergyManager       = GetComponentInParent<HeldEnergyManager>();
    }
	
	void Update()
    {
        if (Input.GetButton("DrainEnergy")) {
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
            IDie dying = objectsInProximity[i].GetComponent<IDie>();
            IHoldEnergy energyHolder = objectsInProximity[i].GetComponent<IHoldEnergy>();

            if (dying != null) {
                dying.Die();
            }

            if(energyHolder != null) {
                heldEnergyManager.IncreaseHeldEnergy(energyHolder.GetHeldEnergy());
            }
        }
    }

    private Collider[] GetAllObjectsInProximity()
    {
        return Physics.OverlapSphere(tileScanner.transform.position, tileScanner.radius);
    }
}
