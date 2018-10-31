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
            wanderAngle = Random.Range(-6.28f, 6.28f);
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
            //float magnitude = displacement.magnitude;
            //displacement.x = Mathf.Cos(wanderAngle) * magnitude;
            //displacement.y = Mathf.Sin(wanderAngle) * magnitude;
            displacement = RotateVector(displacement, wanderAngle);

            // update wanderangle by a little bit
            wanderAngle += (Random.Range(-6.28f, 6.28f) * wanderAngleChangeRate); //- (wanderAngleChangeRate * 0.5f);
            if(wanderAngle > 6.28f)
            {
                wanderAngle = -6.28f;
            }
            else if(wanderAngle < -6.28f)
            {
                wanderAngle = 6.28f;
            }

            return displacement;
        }

        public Vector3 GetSteering(Transform transform, Vector3 velocity)
        {
            Vector3 circleDistance = GetCircleDistance(velocity);
            wanderAngle += (Random.Range(-180, 180) * wanderAngleChangeRate); //- (wanderAngleChangeRate * 0.5f);

            Vector3 displacement = new Vector3(1, 1, 0);
            displacement = Vector3.Scale(displacement, new Vector3(circleRadius, circleRadius, circleRadius));
            displacement = Quaternion.AngleAxis(wanderAngle, new Vector3(0, 0, 1)) * displacement;
            Vector3 wanderForce = circleDistance + displacement;

            return AddForce(velocity, wanderForce);
        }

        public Vector3 GetSteering(Vector3 position, Vector3 velocity)
        {
            Vector3 circleDistance = GetCircleDistance(velocity);
            Vector3 displacement = GetDisplacementVector();

            Vector3 wanderForce = circleDistance + displacement;
            Debug.DrawLine(transform.position, wanderForce);

            return AddForce(velocity, wanderForce);
        }
    }
}
