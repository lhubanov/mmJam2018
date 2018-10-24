using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class Pursue : SteeringBase
    {
        private Seek seeker;

        public void Start()
        {
            seeker = GetComponent<Seek>();
        }

        public Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target)
        {
            Vector3 distance = target - position;
            float pursuitCoeff = distance.magnitude / maxVelocity;

            Vector3 futurePosition = position + velocity * pursuitCoeff;

            return seeker.GetSteering(position, velocity, futurePosition);
        }
    }
}
