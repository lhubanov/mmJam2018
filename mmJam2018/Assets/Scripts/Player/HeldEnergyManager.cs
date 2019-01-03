using UnityEngine;
using UnityEngine.UI;

public class HeldEnergyManager : MonoBehaviour
{
    public StateMachine World;

    public float EnergyLoseRate = 2f;
    public float EnergyLoseDelay = 1f;
    private float NextTick = 0f;

    private void Start()
    {
        World.HeldEnergy = 0;
    }

    public void IncreaseHeldEnergy(float amount)
    {
        if(World.HeldEnergy < 100) { 
            World.HeldEnergy += amount;
        }
    }

    public void DecreaseHeldEnergy(float amount)
    {
        if(World.HeldEnergy > 0) { 
            World.HeldEnergy -= amount;
        }
    }

    // FIXME: remove this string check
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if(Time.time > NextTick)
            {
                NextTick = Time.time + EnergyLoseDelay;
                DecreaseHeldEnergy(EnergyLoseRate);
            }
        }
    }
}
