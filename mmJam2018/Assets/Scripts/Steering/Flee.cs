using UnityEngine;
namespace Assets.Scripts.Steering
{
    public class Flee : SteeringBase
    {
        private Seek Seeker;

        public void Start()
        {
            // FIXME:   I realized this is bad, as it hides all the constants and just uses the defaults.
            //          Instead, fetch those from a scriptable object or some separate
            //          storage for all the constants;
            //          This approach will also need a way to override them, if needed (or just setters?)
            Seeker = new Seek();
        }

        public Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target)
        {
            return -Seeker.GetSteering(position, velocity, target);
        }
    }
}
