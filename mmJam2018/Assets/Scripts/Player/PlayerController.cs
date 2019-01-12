
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

    private PlayerAnimationManager animationManager;

    private bool keyPressed = false;
    private float nextTurn = 0;

    [SerializeField]
    private Vector3 teleportLocation;

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

        //walkingInstance = RuntimeManager.CreateInstance(CloakSound);
        animationManager = GetComponent<PlayerAnimationManager>();
    }

    // FIXME: This is in heavy need of refactoring
    void Update()
    {
        if (Input.GetButton("Teleport")) {
            transform.position = teleportLocation;
            animationManager.AnimateTeleport();
            return;
        }

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {
            keyPressed = true;
        } else {
            keyPressed = false;
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        if ((moveHorizontal < 0) && Time.time > nextTurn && keyPressed)
        {
            nextTurn = Time.time + TurningCooldown;
            sprite.flipX = true;
            animationManager.AnimateSideWalk();
        }
        else if (moveHorizontal > 0 && keyPressed && Time.time > nextTurn)
        {
            nextTurn = Time.time + TurningCooldown;
            sprite.flipX = false;
            animationManager.AnimateSideWalk();
        }

        float moveVertical = Input.GetAxis("Vertical");
        if ((moveVertical < 0) && keyPressed && Time.time > nextTurn)
        {
            nextTurn = Time.time + TurningCooldown;
            animationManager.AnimateDownMove();
            sprite.flipY = false;
        }
        else if (moveVertical > 0 && keyPressed && Time.time > nextTurn)
        {
            nextTurn = Time.time + TurningCooldown;
            animationManager.AnimateUpMove();
        }


        if (!Input.GetButton("DrainEnergy"))
        {
            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            float slowedDownSpeed = Speed - WorldRules.PlayerMovementSlowdown;

            rbody.AddForce(movement * slowedDownSpeed);
        }
    }
}