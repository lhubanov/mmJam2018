using System;

using UnityEngine;
using Assets.Scripts;

public class SlowDownPlayer : MonoBehaviour
{
    [SerializeField]
    private StateMachine WorldRules;

    [SerializeField]
    private float slowdown = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if(player != null) {
            WorldRules.PlayerMovementSlowdown = slowdown;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null) {
            WorldRules.PlayerMovementSlowdown = 0;
        }
    }
}
