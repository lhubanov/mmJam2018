using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        AnimateIdle();
    }

    public void AnimateLifeDrain()
    {
        animator.Play("drainLife");
    }

    public void AnimateSideWalk()
    {
        animator.Play("playerSideWalk");
    }

    public void AnimateUpMove()
    {
        animator.Play("moveUpwards");
    }

    public void AnimateDownMove()
    {
        animator.Play("moveDownwards");
    }

    public void AnimateIdle()
    {
        animator.Play("playerIdle");
    }

    public void AnimateTeleport()
    {
        // Add teleport animation
    }
}
