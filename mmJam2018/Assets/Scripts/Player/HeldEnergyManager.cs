using System;
using UnityEngine;

public class HeldEnergyManager : MonoBehaviour
{
    public StateMachine World;

    private void Start()
    {
        World.HeldEnergy.value = 0;
    }

    public void IncreaseHeldEnergy(float amount)
    {
        World.HeldEnergy.value += amount;
    }
}
