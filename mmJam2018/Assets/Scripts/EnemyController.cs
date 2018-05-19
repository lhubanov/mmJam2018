using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BoxCollider idleMovementRange;
    private Vector3 NextIdleMovementPosition;

    private float   NextIdleMovement = 0;
    private bool    idling = true;

    public  float   IdleMovementCooldown = 5f;

    public  float   SmoothFactor = 0.5f;
    public  float   AggroSmoothFactor = 0.8f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        idleMovementRange = GetComponentInParent<BoxCollider>();
        NextIdleMovement = IdleMovementCooldown;
        NextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
    }

    private void Update()
    {
        if(Time.time <= NextIdleMovement && idling) {
            //FIXME: Play movement animation
            transform.position = Vector3.Lerp(transform.position, NextIdleMovementPosition, Time.deltaTime * SmoothFactor);
        } else if (idling) {
            NextIdleMovement = Time.time + IdleMovementCooldown;
            if (idleMovementRange.bounds.Contains(this.transform.position)) {
                NextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
            } else {
                //transform.position = Vector3.Lerp(transform.position, idleMovementRange.transform.position, Time.deltaTime * SmoothFactor);
                NextIdleMovementPosition = idleMovementRange.transform.position;
            }
        } else {
            GameObject player = GameObject.Find("Player");
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * AggroSmoothFactor);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            idling = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") {
            idling = true;
            NextIdleMovement = Time.time + 1f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            // This seems primitive, especially when done so many times :D
            if (Input.GetButton("Fire2")) {

                var playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
                playerAnimator.Play("attack");

                //play dying animation here
                sprite.color = new Color32(0, 0, 0, 255);  
            } 
        }
    }
}
