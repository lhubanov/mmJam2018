using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void AnimateLifeDrain()
    {
        animator.Play("drainLife");
    }
}
