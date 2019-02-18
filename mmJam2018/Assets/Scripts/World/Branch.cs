using UnityEngine;

using Assets.Scripts;
using FMODUnity;

public class Branch : MonoBehaviour, IDie, IHoldEnergy
{
    [SerializeField]
    protected StateMachine stateMachine;

    [SerializeField]
    private float heldEnergy;
    private Animator animator;

    // Apparently using the StateHashName is also unreliable. Stuck with strings for now, I guess.
    [SerializeField]
    protected string animation;

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

    public virtual void Die()
    {
        animator.Play(animation);
        RuntimeManager.PlayOneShot(DestructionSound);
    }
}
