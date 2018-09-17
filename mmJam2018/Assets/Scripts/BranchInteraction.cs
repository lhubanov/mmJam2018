using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BranchInteraction : MonoBehaviour
{
    public StateMachine stateMachine;

    [EventRef]
    public string DestructionSound;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && Input.GetButton("Fire1"))
        {
            GetComponentInParent<Animator>().Play("BranchDisappear1");
            RuntimeManager.PlayOneShot(DestructionSound);
            stateMachine.CurrentState.AdvanceState(stateMachine);
        }
    }
}
