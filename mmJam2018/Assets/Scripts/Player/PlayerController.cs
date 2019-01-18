
using UnityEngine;

using Assets.Scripts.States;

using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    //[EventRef]
    //public string CloakSound;
    //private EventInstance walkingInstance;

    private Rigidbody rbody;

    private PlayerAnimationManager animationManager;

    [SerializeField]
    private Vector3 teleportLocation;

    [SerializeField]
    private float speed;

    [SerializeField]
    private StateMachine stateMachine;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        //walkingInstance = RuntimeManager.CreateInstance(CloakSound);
        animationManager = GetComponent<PlayerAnimationManager>();
    }

    void Update()
    {
        if (Input.GetButton("Teleport") || (stateMachine.CurrentState is EndingState)) {
            animationManager.AnimateTeleport();
            transform.position = teleportLocation;
            return;
        }

        if (!Input.GetButton("DrainEnergy"))
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (moveVertical < 0) {
                animationManager.MoveDownwards();
            } else if (moveVertical > 0) {
                animationManager.MoveUpwards();
            }

            if (moveHorizontal < 0) {
                animationManager.MoveLeft();
            } else if (moveHorizontal > 0) {
                animationManager.MoveRight();
            }

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            float movementSpeed = speed - stateMachine.PlayerMovementSlowdown;

            rbody.AddForce(movement * movementSpeed);
        }
    }
}