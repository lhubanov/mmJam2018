using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class Flee : SteeringBase
    {
        private Seek Seeker;

        public void Start()
        {
            Seeker = GetComponent<Seek>();
        }

        public Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target)
        {
            return -Seeker.GetSteering(position, velocity, target);
        }
    }
}
