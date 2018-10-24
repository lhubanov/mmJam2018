using UnityEngine;

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

        // Radians
        [SerializeField]
        private float wanderAngle;

        private void Start()
        {
            wanderAngle = Random.Range(-3.14f, 3.14f);
        }

        private Vector3 GetCircleDistance(Vector3 velocity)
        {
            // TODO: Double-check this is what the scaling is supposed to do
            return Vector3.Scale(velocity.normalized, new Vector3(circleDistance, circleDistance, circleDistance));
        }

        private Vector3 GetDisplacementVector()
        {
            Vector3 displacement = new Vector3(1, 1, 0);
            displacement = Vector3.Scale(displacement, new Vector3(circleRadius, circleRadius, circleRadius));


            // double check this does not cause odd rotations or
            // rotations only one way or sth like that

            //SetAngle logic
            float magnitude = displacement.magnitude;
            displacement.x = Mathf.Cos(wanderAngle) * magnitude;
            displacement.y = Mathf.Sin(wanderAngle) * magnitude;

            // update wanderangle by a little bit
            wanderAngle += (Random.Range(-3.14f, 3.14f) * wanderAngleChangeRate) - (wanderAngleChangeRate * 0.5f);

            return displacement;
        }

        public Vector3 GetSteering(Vector3 position, Vector3 velocity)
        {
            Vector3 circleDistance = GetCircleDistance(velocity);
            Vector3 displacement = GetDisplacementVector();

            Vector3 wanderForce = circleDistance + displacement;

            return AddForce(velocity, wanderForce);
        }
    }
}
