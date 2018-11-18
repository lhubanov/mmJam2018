using System.Collections;
using UnityEngine;
using Assets.Scripts.Steering;
using Assets.Scripts.Steering.SteeringData;

public class EnemyController : MonoBehaviour
{
    private BoxCollider     idleMovementRange;
    private Vector3         nextIdleMovementPosition;

    private Transform       player;
    private Seek            Seeker;
    private Arrive          Arrive;
    private Flee            Flee;
    private Wander          Wander;
    private CollisionAvoid  CollisionAvoid;

    [SerializeField]
    protected float         maxForce = 0.2f;

    [SerializeField]
    private GameObject      steeringPrefab = null;
    private Steer           steering = null;


    private float           NextIdleMovement = 0;
    private bool            idling = true;

    public  float           IdleMovementCooldown = 5f;

    public  float           SmoothFactor = 0.5f;
    public  float           AggroSmoothFactor = 0.8f;


    [SerializeField]
    private Vector3         velocity;
    [SerializeField]
    private Vector3         nextPosition;


    private void Start()
    {
        if(steeringPrefab != null) { 
            steering = steeringPrefab.GetComponent<Steer>();
            steering.Initialize();
        }

        nextPosition = new Vector3(0, 0, 0);
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

    //FIXME: This needs refactoring as well, once all the steering is cleaned up, move to single Steer script
    private void Update()
    {
        if(steering != null)
        { 
            velocity += steering.GetSteering(new SteeringDataBase(transform.position, velocity, nextPosition));
            velocity = Vector3.ClampMagnitude(velocity, maxForce);

            Debug.DrawLine(transform.position, nextPosition * 2, Color.green);
            nextPosition = transform.position + velocity;

            Debug.Log(string.Format("New position: {0}", nextPosition));
            transform.position = nextPosition;
        }

        //if (idling)
        //{           
        //    if (Wander != null)
        //    {
        //        // Wander
        //        velocity += Wander.GetSteering(new SteeringDataBase(null, velocity, null));
        //        Vector3 newPos = transform.position + velocity;
        //        //Debug.DrawLine(transform.position, newPos * 2, Color.red);

        //        // CollisionAvoid
        //        velocity += CollisionAvoid.GetSteering(new SteeringDataBase(transform.position, velocity, newPos)); //transform, velocity, newPos);
        //        velocity = Vector3.ClampMagnitude(velocity, maxForce);

        //        newPos = transform.position + velocity;
        //        Debug.DrawLine(transform.position, newPos * 2, Color.green);

        //        transform.position = newPos;
        //    } else {

        //        if (Time.time <= NextIdleMovement)
        //        {
        //            transform.position += Seeker.GetSteering(new SteeringDataBase(transform.position, velocity, nextIdleMovementPosition));
        //        }
        //        else
        //        {
        //            NextIdleMovement = Time.time + IdleMovementCooldown;
        //            if (idleMovementRange.bounds.Contains(this.transform.position)) {
        //                nextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
        //            } else {
        //                nextIdleMovementPosition = idleMovementRange.transform.position;
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    // FIXME: Move these to interface and call GetIdleAction(); or whatever!
        //    // Flee from player
        //    transform.position += Flee.GetSteering(new SteeringDataBase(transform.position, velocity, player.transform.position));
        //}
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
