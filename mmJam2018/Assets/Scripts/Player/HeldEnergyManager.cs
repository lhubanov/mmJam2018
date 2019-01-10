using UnityEngine;
using UnityEngine.UI;

public class HeldEnergyManager : MonoBehaviour
{
    //[SerializeField]
    //private StateMachine World;

    //[SerializeField]
    //private float EnergyLoseRate = 2f;

    //[SerializeField]
    //private float EnergyLoseDelay = 1f;

    //[SerializeField]
    //private float NextTick = 0f;

    //private void Start()
    //{
    //    World.HeldEnergy = 0;
    //}

    //public void IncreaseHeldEnergy(float amount)
    //{
    //    if(World.HeldEnergy < 100) { 
    //        World.HeldEnergy += amount;
    //    }
    //}

    //private void DecreaseHeldEnergy(float amount)
    //{
    //    if(World.HeldEnergy > 0) { 
    //        World.HeldEnergy -= amount;
    //    }
    //}

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.GetComponent<EnemyController>() != null)
    //    {
    //        if(Time.time > NextTick) {
    //            NextTick = Time.time + EnergyLoseDelay;
    //            DecreaseHeldEnergy(EnergyLoseRate);
    //        }
    //    }
    //}
}
