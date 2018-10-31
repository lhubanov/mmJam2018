using System.Collections;
using UnityEngine;
using Assets.Scripts.Steering;

public class EnemyController : MonoBehaviour
{
    private BoxCollider idleMovementRange;
    private Vector3     nextIdleMovementPosition;

    private Transform   player;
    private Seek        Seeker;
    private Arrive      Arrive;
    private Flee        Flee;
    private Wander      Wander;
    private CollisionAvoid CollisionAvoid;
    [SerializeField]
    protected float maxForce = 0.2f;


    private float       NextIdleMovement = 0;
    private bool        idling = true;

    public  float       IdleMovementCooldown = 5f;

    public  float       SmoothFactor = 0.5f;
    public  float       AggroSmoothFactor = 0.8f;

    [SerializeField]
    private Vector3     velocity;
    


    private void Start()
    {
        velocity = new Vector3(0,0,0);
        idleMovementRange = GetComponentInParent<BoxCollider>();
        Seeker = GetComponent<Seek>();
        Flee = GetComponent<Flee>();
        Wander = GetComponent<Wander>();
        CollisionAvoid = GetComponent<CollisionAvoid>();
        player = null;

        NextIdleMovement = IdleMovementCooldown;
        nextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
    }

    private void Update()
    {
        if (idling)
        {           
            if (Wander != null)
            {
                //Wander
                //transform.position += Wander.GetSteering(transform.position, velocity);
                velocity += Wander.GetSteering(transform, velocity);
                Vector3 newPos = transform.position + velocity;
                //Debug.DrawLine(transform.position, newPos * 2, Color.red);

                velocity += CollisionAvoid.GetSteering(transform, velocity, newPos);
                velocity = Vector3.ClampMagnitude(velocity, maxForce);

                newPos = transform.position + velocity;
                Debug.DrawLine(transform.position, newPos * 2, Color.green);

                transform.position = newPos;
            } else {

                if (Time.time <= NextIdleMovement)
                {
                    //transform.position = Vector3.Lerp(transform.position, nextIdleMovementPosition, Time.deltaTime * SmoothFactor);
                    transform.position += Seeker.GetSteering(transform.position, velocity, nextIdleMovementPosition);
                }
                else
                {
                    NextIdleMovement = Time.time + IdleMovementCooldown;
                    if (idleMovementRange.bounds.Contains(this.transform.position)) {
                        nextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
                    } else {
                        nextIdleMovementPosition = idleMovementRange.transform.position;
                    }
                }
            }
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.position, playerPosition.transform.position, Time.deltaTime * AggroSmoothFactor);

            // Seek player (not really chasing)
            //transform.position += Seeker.GetSteering(transform.position, velocity, player.transform.position);

            // FIXME: Move these to interface and call GetIdleAction(); or whatever!


            // Flee from player
            transform.position += Flee.GetSteering(transform.position, velocity, player.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            player = other.gameObject.transform;
            idling = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") {
            NextIdleMovement = Time.time + 1f;
            player = null;
            idling = true;
        }
    }
}
