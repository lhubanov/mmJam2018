using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;

    [SerializeField]
    private float animationTimeout = 0;

    [SerializeField]
    private float animationCooldown = 0.5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        AnimateIdle();
    }

    public void AnimateLifeDrain()
    {
        animator.Play("drainLife");
    }

    private void AnimateSideWalk()
    {
        animator.Play("playerSideWalk");
    }

    private void AnimateUpMove()
    {
        animator.Play("moveUpwards");
    }

    private void AnimateDownMove()
    {
        animator.Play("moveDownwards");
    }

    private void AnimateIdle()
    {
        animator.Play("playerIdle");
    }

    public void AnimateTeleport()
    {
        // Add teleport animation
    }

    public void MoveUpwards()
    {
        if (Time.time > animationTimeout) { 
            animationTimeout = Time.time + animationCooldown;
            AnimateUpMove();
        }
    }

    public void MoveDownwards()
    {
        if (Time.time > animationTimeout) {
            animationTimeout = Time.time + animationCooldown;
            AnimateDownMove();
            sprite.flipY = false;
        }
    }

    public void MoveLeft()
    {
        if (Time.time > animationTimeout) {
            animationTimeout = Time.time + animationCooldown;
            sprite.flipX = true;
            AnimateSideWalk();
        }
    }

    public void MoveRight()
    {
        if (Time.time > animationTimeout) {
            animationTimeout = Time.time + animationCooldown;
            sprite.flipX = false;
            AnimateSideWalk();
        }
    }
}
