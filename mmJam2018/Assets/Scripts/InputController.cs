using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputController : MonoBehaviour {

    public float speed;

    private Rigidbody2D player;
    private float acceleration;
    private Vector3 forwardMotion;
    
    public float accelerationForce      = 10f;
    public float rotationForce          = 3f;
    public float rotationForceLimit     = 7f;
    public float accelerationIncrement  = 0.05f;
    public float accelerationDecrement  = 0.05f;

    public float maxSpeed               = 8f;

    void Start() {
        player = GetComponent<Rigidbody2D>();
        acceleration = 0;
        forwardMotion = Vector3.zero;
    }

    //Note: this only gets called when interacting with player via keyboard
    //      input; does not get called continuously;
	void FixedUpdate ()
    {
        // horizontal and vertical movement keys set by default in input manager
        float rotation = Input.GetAxis("Horizontal");

        // deceleration is handled by the rigidbody's drag property (kind of; TODO: further investigate!)
        if (Input.GetKey("up")) {
            //this.GetComponentInChildren<ParticleSystem>().Emit(5);
            if (acceleration <= maxSpeed) {
                acceleration += accelerationIncrement;
            }
        } else if (!Input.GetKey("up")) { 
            acceleration -= accelerationDecrement;
        }

        //Debug.Log("Acceleration: " + acceleration);

        //TODO: rework & investigate drag thing! this is rough
        if (acceleration < 0) { 
            acceleration = 0;
        }

        forwardMotion = transform.forward * acceleration * accelerationForce;
        player.AddForce(forwardMotion);


        //Debug.Log("Rotation force: " + rotation * rotationForce);

        //rigidbody rotation only around y axis
        if((rotation * rotationForce) < rotationForceLimit) {
            //Vector3 torque = new Vector3(0, rotation * rotationForce, 0);
            float torque = rotation * rotationForce;
            player.AddTorque(torque);
        }
    }
}