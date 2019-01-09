using UnityEngine;

using Assets.Scripts;
using FMODUnity;

public class BranchInteraction : MonoBehaviour, IDie
{
    [SerializeField]
    private StateMachine stateMachine;
    private Animator animator;

    // FIXME: Surely there's a way to play animations not via Animator.Play(string)
    [SerializeField]
    private string animation;

    [EventRef]
    public string DestructionSound;


    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null && Input.GetButton("DrainEnergy")) {
            Die();
        }
    }

    public void Die()
    {
        animator.Play(animation);
        RuntimeManager.PlayOneShot(DestructionSound);
        stateMachine.CurrentState.AdvanceState(stateMachine);
    }
}
