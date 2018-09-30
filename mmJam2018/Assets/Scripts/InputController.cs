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

    // TODO: all of this needs refactoring; only works with keyboard i/p
    void FixedUpdate ()
    {
        float rotation = Input.GetAxis("Horizontal");

        if (Input.GetKey("up")) {
            if (acceleration <= maxSpeed) {
                acceleration += accelerationIncrement;
            }
        } else if (!Input.GetKey("up")) { 
            acceleration -= accelerationDecrement;
        }

        // Debug.Log("Acceleration: " + acceleration);
        if (acceleration < 0) { 
            acceleration = 0;
        }

        forwardMotion = transform.forward * acceleration * accelerationForce;
        player.AddForce(forwardMotion);

        if((rotation * rotationForce) < rotationForceLimit) {
            float torque = rotation * rotationForce;
            player.AddTorque(torque);
        }
    }
}