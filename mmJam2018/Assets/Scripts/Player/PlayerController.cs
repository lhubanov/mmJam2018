
using UnityEngine;

using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    [EventRef]
    public string CloakSound;
    private EventInstance walkingInstance;

    private Rigidbody rbody;
    private SpriteRenderer sprite;

    // FIXME: Move to animation manager and play via member
    private Animator animator;

    private bool keyPressed = false;
    private float nextTurn = 0;
    private bool talking = false;

    [SerializeField]
    private float TurningCooldown = 0.5f;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private StateMachine WorldRules;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();

        walkingInstance = RuntimeManager.CreateInstance(CloakSound);

        animator = GetComponent<Animator>();
        animator.Play("playerIdle");
    }

    // FIXME: This is in heavy need of refactoring
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            keyPressed = true;
        } else {
            keyPressed = false;
        }

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        if ((moveHorizontal < 0) && Time.time > nextTurn && keyPressed)
        {
            nextTurn = Time.time + TurningCooldown;
            sprite.flipX = true;
            animator.Play("playerSideWalk");
        }
        else if (moveHorizontal > 0 && keyPressed && Time.time > nextTurn)
        {
            nextTurn = Time.time + TurningCooldown;
            sprite.flipX = false;
            animator.Play("playerSideWalk");
        }

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");
        if ((moveVertical < 0) && keyPressed && Time.time > nextTurn)
        {
            nextTurn = Time.time + TurningCooldown;
            animator.Play("moveDownwards");
            sprite.flipY = false;
        }
        else if (moveVertical > 0 && keyPressed && Time.time > nextTurn)
        {
            nextTurn = Time.time + TurningCooldown;
            animator.Play("moveUpwards");
        }


        if (!Input.GetButton("Fire2") && !talking)
        {
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            float slowedDownSpeed = Speed - WorldRules.PlayerMovementSlowdown;

            rbody.AddForce(movement * slowedDownSpeed);
        }
    }
}