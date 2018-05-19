﻿
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float speed;             //Floating point variable to store the player's movement speed.
    private Rigidbody2D rb2d;       //Store a reference to the Rigidbody2D component required to use 2D Physics.
    private SpriteRenderer sprite;
    //private bool turnLeft = false;
    //private bool turnRight = false;

    private bool keyPressed = false;
    private float NextTurn = 0;
    public float TurningCooldown = 0.5f;

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetAxisRaw("Horizontal") != 0) {
            keyPressed = true;
        } else {
            keyPressed = false;
        }
        
        if((moveHorizontal < 0) && Time.time > NextTurn && keyPressed) {
            NextTurn = Time.time + TurningCooldown;
            sprite.flipX = true;
        } else if (moveHorizontal >= 0 && keyPressed) {
            NextTurn = Time.time + TurningCooldown;
            sprite.flipX = false;
        }

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis("Vertical");

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        //Call the AddForce function of our Rigidbody2D rb2d supplying movement multiplied by speed to move our player.
        rb2d.AddForce(movement * speed);
    }
}