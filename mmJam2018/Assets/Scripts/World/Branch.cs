using UnityEngine;

using Assets.Scripts;
using FMODUnity;

public class Branch : MonoBehaviour, IDie, IHoldEnergy
{
    [SerializeField]
    private StateMachine stateMachine;

    [SerializeField]
    private float heldEnergy;
    private Animator animator;

    // FIXME: Try playing via HashCode (see docu)
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

    public float GetHeldEnergy()
    {
        return heldEnergy;
    }

    public void Die()
    {
        animator.Play(animation);
        RuntimeManager.PlayOneShot(DestructionSound);

        // This goes alongside with above FIXME comment
        if(animation == "BranchDisappear3") {
            stateMachine.FadeAmount = 255;
            stateMachine.CurrentState.AdvanceState(stateMachine);
        }
    }
}
