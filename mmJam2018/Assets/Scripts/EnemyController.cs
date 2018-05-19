using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BoxCollider idleMovementRange;

    private float   NextIdleMovement = 0;
    public  float   IdleMovementCooldown = 5f;

    public  float   SmoothFactor = 0.5f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        idleMovementRange = GetComponentInParent<BoxCollider>();
    }

    private void Update()
    {
        if (idleMovementRange.bounds.Contains(this.transform.position)) {
            if(Time.time <= NextIdleMovement) {
                //FIXME: Play movement animation
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x + Random.Range(0, 2), transform.position.y + Random.Range(0, 2), 0), Time.deltaTime * SmoothFactor);
            } else {

            }
        } else {
            transform.position = Vector3.Lerp(transform.position, idleMovementRange.center, Time.deltaTime * SmoothFactor);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") {
            // This seems primitive, especially when done so many times :D
            if (Input.GetButton("Fire2")) {
                sprite.color = new Color32(0, 0, 0, 255);  
            } 
        }
    }
}
