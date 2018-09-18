using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private BoxCollider idleMovementRange;
    private Vector3 nextIdleMovementPosition;

    private Transform playerPosition;

    private float   NextIdleMovement = 0;
    private bool    idling = true;

    public  float   IdleMovementCooldown = 5f;

    public  float   SmoothFactor = 0.5f;
    public  float   AggroSmoothFactor = 0.8f;

    private void Start()
    {
        idleMovementRange = GetComponentInParent<BoxCollider>();
        playerPosition = null;

        NextIdleMovement = IdleMovementCooldown;
        nextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
    }

    private void Update()
    {
        if(Time.time <= NextIdleMovement && idling)
        {
            transform.position = Vector3.Lerp(transform.position, nextIdleMovementPosition, Time.deltaTime * SmoothFactor);
        }
        else if (idling)
        {
            NextIdleMovement = Time.time + IdleMovementCooldown;
            if (idleMovementRange.bounds.Contains(this.transform.position)) {
                nextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
            } else {
                nextIdleMovementPosition = idleMovementRange.transform.position;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, playerPosition.transform.position, Time.deltaTime * AggroSmoothFactor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            playerPosition = other.gameObject.transform;
            idling = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") {
            idling = true;
            NextIdleMovement = Time.time + 1f;
            playerPosition = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // FIXME: all of this should happen in the other script, where enemy animate gets called
        if(other.tag == "Player")
        {
            if (Input.GetButton("Fire2"))
            {
                PlayDeathAnimation();
            } 
        }
    }

    public void PlayDeathAnimation()
    {
        StartCoroutine(disappearIntoTheVoid(0.75f));
    }

    IEnumerator disappearIntoTheVoid(float delay)
    {
        GetComponent<Animator>().Play("enemyDie");
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(false);
    }
}
