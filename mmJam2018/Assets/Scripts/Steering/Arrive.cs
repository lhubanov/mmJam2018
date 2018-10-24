using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class Arrive : SteeringBase
    {
        [SerializeField]
        private float slowingRadius = 5f;

        public Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target)
        {
            Vector3 desiredVelocity = new Vector3(0, 0, 0);
            Vector3 distanceToTarget = target - position;

            if(distanceToTarget.magnitude < slowingRadius) {
                desiredVelocity = distanceToTarget.normalized * maxVelocity * (distanceToTarget.magnitude / slowingRadius);
            } else {
                // Basically seeking
                desiredVelocity = distanceToTarget.normalized * maxVelocity;
            }

            Vector3 steering = desiredVelocity - velocity;
            return AddForce(velocity, steering);
        }
    }
}
