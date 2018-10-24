using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class Flee : SteeringBase
    {
        // Assignable, if left null- searches for an attached member
        [SerializeField]
        private Seek Seeker;

        public void Start()
        {
            // FIXME:   There is probably some way to avoid this
            //          by having one movement controller script or sth
            //          that you call methods onto;
            //          Or, alternatively- by inheriting down?
            //
            //          Dunno, think about it
            if(Seeker == null) { 
                Seeker = GetComponent<Seek>();
            }
        }

        public Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target)
        {
            return -Seeker.GetSteering(position, velocity, target);
        }
    }
}
