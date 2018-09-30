using UnityEngine;
using Assets.Scripts.UI;

public class HeldEnergyUI : MonoBehaviour, IUpdateHeldEnergy
{
    public StateMachine World;

    public float GetValueFromStateMachine()
    {
        return World.HeldEnergy;
    }
}
