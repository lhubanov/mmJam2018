using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public abstract class SteeringBase : MonoBehaviour, ISteer
    {
        [SerializeField]
        protected float maxSpeed = 0.2f;
        [SerializeField]
        protected float maxForce = 0.2f;
        [SerializeField]
        protected float maxVelocity = 0.2f;
        [SerializeField]
        protected float mass = 1f;

        // TODO: Review, bad name probably
        protected Vector3 AddForce(Vector3 velocity, Vector3 steering)
        {
            steering = Vector3.ClampMagnitude(steering, maxForce);
            steering = steering / mass;

            velocity = Vector3.ClampMagnitude(velocity + steering, maxSpeed);
            return velocity;
        }

        // Leaving this here, for the time being, just as a prototype
        //protected Vector3 RotateVector(Vector3 vector, float angle)
        //{
        //    Vector3 v = new Vector3(0,0,0);
        //    float magnitude = vector.magnitude;
        //    v.x = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
        //    v.y = vector.y * Mathf.Cos(angle) + vector.x * Mathf.Sin(angle);

        //    return v;
        //}

        public abstract Vector3 GetSteering(ISteeringData steeringData);
    }
}
