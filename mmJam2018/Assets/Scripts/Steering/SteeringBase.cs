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
        protected float mass = 1f;

        protected Vector3 AddForce(Vector3 velocity, Vector3 steering)
        {
            steering = Vector3.ClampMagnitude(steering, maxForce);
            steering = steering / mass;

            return Vector3.ClampMagnitude(velocity + steering, maxSpeed);
        }

        public virtual Vector3 GetSteering(Vector3 position, Vector3 velocity) { return new Vector3(0, 0, 0); }
        public virtual Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target) { return new Vector3(0, 0, 0); }
    }
}
