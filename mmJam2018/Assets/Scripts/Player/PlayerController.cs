
using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    [EventRef]
    public string CloakSound;
    private EventInstance walkingInstance;

    private Rigidbody rigidbody;
    private SpriteRenderer sprite;
    private Animator animator;

    private bool keyPressed = false;
    private float NextTurn = 0;

    public float TurningCooldown = 0.5f;
    public bool Talking = false;
    public float Speed;


    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rigidbody = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();

        walkingInstance = RuntimeManager.CreateInstance(CloakSound);

        animator = GetComponent<Animator>();
        animator.Play("playerIdle");
    }


    private void Update()
    {
        //FMOD.Studio.PLAYBACK_STATE walkingSoundState;
        //walkingInstance.getPlaybackState(out walkingSoundState);
        //if (walkingSoundState != PLAYBACK_STATE.STOPPING) {
        //    walkingInstance.stop(STOP_MODE.ALLOWFADEOUT);
        //}
    }

    // This is in heavy need of refactoring
    void FixedUpdate()
    {

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            keyPressed = true;
        } else {
            keyPressed = false;
        }

        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");
        if ((moveHorizontal < 0) && Time.time > NextTurn && keyPressed)
        {
            NextTurn = Time.time + TurningCooldown;
            sprite.flipX = true;
            animator.Play("playerSideWalk");
        }
        else if (moveHorizontal > 0 && keyPressed && Time.time > NextTurn)
        {
            NextTurn = Time.time + TurningCooldown;
            sprite.flipX = false;
            animator.Play("playerSideWalk");
        }

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");
        if ((moveVertical < 0) && keyPressed && Time.time > NextTurn)
        {
            NextTurn = Time.time + TurningCooldown;
            animator.Play("moveDownwards");
            sprite.flipY = false;
        }
        else if (moveVertical > 0 && keyPressed && Time.time > NextTurn)
        {
            NextTurn = Time.time + TurningCooldown;
            animator.Play("moveUpwards");
        }


        if (!Input.GetButton("Fire2") && !Talking)
        {
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rigidbody.AddForce(movement * Speed);

            FMOD.Studio.PLAYBACK_STATE walkingSoundState;
            walkingInstance.getPlaybackState(out walkingSoundState);
            if (walkingSoundState != PLAYBACK_STATE.PLAYING) {
                walkingInstance.start();
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy") {
            Debug.Log("I'm-a dying!");
            //reduce gathered energy
        }
    }

}