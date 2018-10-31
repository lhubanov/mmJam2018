using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class SteeringBase : MonoBehaviour, ISteer
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

        // FIXME: There are quaternions for this!
        protected Vector3 RotateVector(Vector3 vector, float angle)
        {
            Vector3 v = new Vector3(0,0,0);
            float magnitude = vector.magnitude;
            v.x = vector.x * Mathf.Cos(angle) - vector.y * Mathf.Sin(angle);
            v.y = vector.y * Mathf.Cos(angle) + vector.x * Mathf.Sin(angle);

            return v;
        }

        public virtual Vector3 GetSteering(Vector3 position, Vector3 velocity) { return new Vector3(0, 0, 0); }
        public virtual Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target) { return new Vector3(0, 0, 0); }
    }
}
