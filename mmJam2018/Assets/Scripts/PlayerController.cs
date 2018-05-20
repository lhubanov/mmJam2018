﻿
using UnityEngine;
using System.Collections;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    [EventRef]
    public string CloakSound;
    private EventInstance walkingInstance;



    public float speed;             //Floating point variable to store the player's movement speed.

    private Rigidbody rigidbody;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private SpriteRenderer sprite;
    private Animator animator;

    private bool keyPressed = false;
    private float NextTurn = 0;

    public float TurningCooldown = 0.5f;
    public bool Talking = false;


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

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            keyPressed = true;
        } else {
            keyPressed = false;
        }
        
        if((moveHorizontal < 0) && Time.time > NextTurn && keyPressed) {
            NextTurn = Time.time + TurningCooldown;
            sprite.flipX = true;
            animator.Play("playerSideWalk");

            //FIXME: play animation here
        } else if (moveHorizontal > 0 && keyPressed && Time.time > NextTurn) {
            NextTurn = Time.time + TurningCooldown;
            sprite.flipX = false;
            animator.Play("playerSideWalk");

            //FIXME: play animation here
        }

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");
        Debug.Log("moveVertical: " + moveVertical);

        if ((moveVertical < 0) && keyPressed && Time.time > NextTurn) {
            NextTurn = Time.time + TurningCooldown;

            animator.Play("moveDownwards");
            sprite.flipY = false;
            //FIXME: play animation here
        } else if (moveVertical > 0 && keyPressed && Time.time > NextTurn) {
            NextTurn = Time.time + TurningCooldown;
            
            animator.Play("moveUpwards");
            //sprite.flipY = true;
            //FIXME: play animation here
        }

        if (!Input.GetButton("Fire2") && !Talking) {
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rigidbody.AddForce(movement * speed);

            FMOD.Studio.PLAYBACK_STATE walkingSoundState;
            walkingInstance.getPlaybackState(out walkingSoundState);
            if (walkingSoundState != PLAYBACK_STATE.PLAYING) {
                //walkingInstance.start();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy") {
            Debug.Log("I'm-a dying!");
            //reduce player health
        }
    }
}