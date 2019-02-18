using UnityEngine;
using Assets.Scripts;

public class HeldEnergyManager : MonoBehaviour
{
    private SphereCollider          tileScanner;
    private PlayerAnimationManager  playerAnimationManager;
    private PlayerSFXManager        playerSFXManager;

    [SerializeField]
    private StateMachine World;

    [SerializeField]
    private float EnergyLoseRate = 2f;

    [SerializeField]
    private float EnergyLoseDelay = 1f;

    [SerializeField]
    private float NextTick = 0f;

    void Start()
    {
        tileScanner             = GetComponent<SphereCollider>();
        playerAnimationManager  = GetComponentInParent<PlayerAnimationManager>();
        playerSFXManager        = GetComponentInParent<PlayerSFXManager>();

        World.HeldEnergy = 0;
    }
	
	void Update()
    {
        if (Input.GetButton("DrainEnergy")) {
            DrainLife();
        }
    }

    private Collider[] GetAllObjectsInProximity()
    {
        return Physics.OverlapSphere(tileScanner.transform.position, tileScanner.radius);
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

            if (energyHolder != null) {
                IncreaseHeldEnergy(energyHolder.GetHeldEnergy());
            }

            if (dying != null && !(dying is Mom)) {
                dying.Die();
            }
        }
    }

    private void IncreaseHeldEnergy(float amount)
    {
        if (World.HeldEnergy < 100) {
            World.HeldEnergy += amount;
        }
    }

    private void DecreaseHeldEnergy(float amount)
    {
        if (World.HeldEnergy > 0) {
            World.HeldEnergy -= amount;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<EnemyController>() != null)
        {
            if (Time.time > NextTick) {
                NextTick = Time.time + EnergyLoseDelay;
                DecreaseHeldEnergy(EnergyLoseRate);
            }
        }
    }
}
