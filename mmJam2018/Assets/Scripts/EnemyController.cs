
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Steering;
using Assets.Scripts.Steering.SteeringData;

public class EnemyController : MonoBehaviour, IDie, IHoldEnergy
{
    private BoxCollider     idleMovementRange;
    private Vector3         nextIdleMovementPosition;

    private Transform       player;
    private Seek            Seeker;
    private Arrive          Arrive;
    private Flee            Flee;
    private Wander          Wander;
    private CollisionAvoid  CollisionAvoid;

    private Steer steering = null;

    [SerializeField]
    protected float         maxForce = 0.2f;

    [SerializeField]
    private GameObject      steeringPrefab = null;

    [SerializeField]
    private float           NextIdleMovement = 0;

    [SerializeField]
    private bool            idling;

    [SerializeField]
    private  float          IdleMovementCooldown = 5f;

    [SerializeField]
    private Vector3         velocity;

    [SerializeField]
    private Vector3         nextPosition;

    [SerializeField]
    private float           heldEnergy;




    private void Start()
    {
        if(steeringPrefab != null) { 
            steering = steeringPrefab.GetComponent<Steer>();
            steering.Initialize();
        }

        nextPosition = new Vector3(0, 0, 0);
        velocity = new Vector3(0,0,0);
        idling = true;

        idleMovementRange = GetComponentInParent<BoxCollider>();
        Seeker = GetComponent<Seek>();
        Flee = GetComponent<Flee>();
        Wander = GetComponent<Wander>();
        CollisionAvoid = GetComponent<CollisionAvoid>();
        player = null;

        NextIdleMovement = IdleMovementCooldown;
        nextIdleMovementPosition = new Vector3(transform.position.x + Random.Range(-5, 5), transform.position.y + Random.Range(-5, 5), 0);
    }

    // FIXME: Test idling - to chasing behaviour
    private void Update()
    {
        if(steering != null)
        {
            if (!idling) {
                nextPosition = player.transform.position;
            }

            velocity += steering.GetSteering(new SteeringDataBase(transform.position, velocity, nextPosition));
            velocity = Vector3.ClampMagnitude(velocity, maxForce);

            Debug.DrawLine(transform.position, nextPosition * 2, Color.green);
            nextPosition = transform.position + velocity;

            Debug.Log(string.Format("New position: {0}", nextPosition));
            transform.position = nextPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null) {
            player = other.gameObject.transform;
            idling = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null) {
            NextIdleMovement = Time.time + 1f;
            player = null;
            idling = true;
        }
    }

    public float GetHeldEnergy()
    {
        return heldEnergy;
    }

    public void Die()
    {
        EnemyAnimationManager animationManager = GetComponentInChildren<EnemyAnimationManager>();

        if (animationManager != null) {
            animationManager.PlayDeathAnimation();
        }

        this.gameObject.SetActive(false);
        Object.Destroy(this.transform.gameObject);
    }
}
