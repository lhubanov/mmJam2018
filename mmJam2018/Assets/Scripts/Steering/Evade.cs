using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class Evade : SteeringBase
    {
        private Flee flee;

        public void Start()
        {
            flee = GetComponent<Flee>();
        }
        
        // However, adding rules for not leaving
        // the game world using this won't be straightforward.
        // Potentially, target can be a vector in front (e.g. velocity?)
        // and if it's of a specific property, evade;

        // However, again, probably easier with raycasting (at least to do the check if not walking out of map/in a wall etc.)
        // Or maybe just colliders and upon collision, turn around?
        public Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target)
        {
            Vector3 distance = target - position;
            float fleeDistance = distance.magnitude / maxVelocity;

            Vector3 futurePosition = position + velocity * fleeDistance;
            return flee.GetSteering(position, velocity, futurePosition);
        }
    }
}
