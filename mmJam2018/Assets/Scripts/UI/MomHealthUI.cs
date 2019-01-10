using UnityEngine;
using Assets.Scripts.UI;

public class MomHealthUI : MonoBehaviour, IUpdateMotherHealth
{
    public StateMachine World;

    public float GetValueFromStateMachine()
    {
        return World.MomCurrentHealth;
    }
}
