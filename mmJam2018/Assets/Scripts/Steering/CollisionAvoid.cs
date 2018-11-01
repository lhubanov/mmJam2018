using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class CollisionAvoid : SteeringBase
    {
        [SerializeField]
        private float AvoidanceRangeScale = 1;
        [SerializeField]
        private float MaxAvoidanceForce = 0.5f;

        public Vector3 GetSteering(Transform transform, Vector3 velocity, Vector3 target)
        {
            Vector3 direction = (target - transform.position).normalized;
            RaycastHit hitInfo = new RaycastHit();

            // FIXME: Raycasts draw up as well
            if(Physics.Raycast(transform.position, target*AvoidanceRangeScale, out hitInfo, 2))
            {
                // There is probably a better way to do this than just any firm colliders
                // Interface, as usual?
                if(!hitInfo.collider.isTrigger && hitInfo.transform != transform)
                {
                    Debug.Log(transform.parent.name);
                    Debug.Log("Raycast detected collision w/ " + hitInfo.collider.name);
                    Debug.Log("Old direction " + direction);

                    // Double-check the normal does not point along Z
                    direction = hitInfo.normal.normalized;
                    direction = Vector3.Scale(direction, new Vector3(MaxAvoidanceForce, MaxAvoidanceForce, MaxAvoidanceForce));

                    Debug.Log("New direction " + direction);
                }
            }

            return AddForce(velocity, direction);
        }
    }
}
