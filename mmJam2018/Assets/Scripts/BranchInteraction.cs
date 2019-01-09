using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class BranchInteraction : MonoBehaviour
{
    public StateMachine stateMachine;
    private Animator animator;

    [EventRef]
    public string DestructionSound;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null && Input.GetButton("DrainEnergy"))
        {
            // Generify this
            animator.Play("BranchDisappear1");
            RuntimeManager.PlayOneShot(DestructionSound);
            stateMachine.CurrentState.AdvanceState(stateMachine);
        }
    }
}
