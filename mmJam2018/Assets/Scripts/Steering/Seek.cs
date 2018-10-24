using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class Seek : SteeringBase
    {
        public Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target)
        {
            Vector3 distanceToTarget = target - position;
            Vector3 desiredVelocity = distanceToTarget.normalized * maxVelocity;

            Vector3 steering = desiredVelocity - velocity;
            return AddForce(velocity, steering);
        }
    }
}
