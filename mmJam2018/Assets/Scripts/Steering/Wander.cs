using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    // FIXME:   This needs some rules to make enemies turn around,
    //          if at the edge of the map
    public class Wander : SteeringBase
    {
        [SerializeField]
        private float circleDistance = 2f;
        [SerializeField]
        private float circleRadius = 2f;
        [SerializeField]
        private float wanderAngleChangeRate = 0.5f;
        // Degrees
        [SerializeField]
        private float wanderAngle;

        private void Start()
        {
            wanderAngle = Random.Range(-180, 180);
        }

        private Vector3 GetCircleDistance(Vector3 velocity)
        {
            return Vector3.Scale(velocity.normalized, new Vector3(circleDistance, circleDistance, 0));
        }

        /// <inheritdoc/>
        public override Vector3 GetSteering(ISteeringData steeringData)
        {
            if(steeringData.Velocity == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Velocity is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            }

            Vector3 velocity = steeringData.Velocity.Value;
            Vector3 circleDistance = GetCircleDistance(velocity);
            wanderAngle += (Random.Range(-180, 180) * wanderAngleChangeRate);

            Vector3 displacement = new Vector3(1, 1, 0);
            displacement = Vector3.Scale(displacement, new Vector3(circleRadius, circleRadius, 0));
            displacement = Quaternion.AngleAxis(wanderAngle, new Vector3(0, 0, 1)) * displacement;
            Vector3 wanderForce = circleDistance + displacement;

            return AddForce(velocity, wanderForce);
        }
    }
}
