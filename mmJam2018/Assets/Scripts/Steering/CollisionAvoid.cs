using UnityEngine;

namespace Assets.Scripts.Steering
{
    public class CollisionAvoid : SteeringBase
    {
        [SerializeField]
        private float TurnAngle = 45;
        [SerializeField]
        private float AvoidanceRangeScale = 1;

        [SerializeField]
        private float MaxAvoidanceForce = 0.5f;

        public Vector3 GetSteering(Transform transform, Vector3 velocity, Vector3 target)
        {
            // FIXME: Draw ray
            Vector3 direction = (target - transform.position).normalized;
            RaycastHit hitInfo = new RaycastHit();

            if(Physics.Raycast(transform.position, target*AvoidanceRangeScale, out hitInfo, 2))
            {
                if(!hitInfo.collider.isTrigger && hitInfo.transform != transform)
                {
                    //Vector3 colliderCenter = new Vector3(0,0,0);
                    //if(raycastHit.collider is BoxCollider) {
                    //    BoxCollider c = raycastHit.collider as BoxCollider;
                    //    colliderCenter = c.center;
                    //} else if(raycastHit.collider is CapsuleCollider) {
                    //    CapsuleCollider c = raycastHit.collider as CapsuleCollider;
                    //    colliderCenter = c.center;
                    //}

                    //if(colliderCenter != Vector3.zero)
                    //{ 
                        Debug.Log(transform.parent.name);
                        Debug.Log("Raycast detected collision w/ " + hitInfo.collider.name);
                        Debug.Log("Old direction " + direction);
                        //direction = Quaternion.AngleAxis(TurnAngle, new Vector3(0, 0, 1)) * direction;
                        //direction = (direction - colliderCenter).normalized;
                        direction = hitInfo.normal.normalized;
                        direction = Vector3.Scale(direction, new Vector3(MaxAvoidanceForce, MaxAvoidanceForce, MaxAvoidanceForce));

                        Debug.Log("New direction " + direction);
                    //}
                }
            }

            return AddForce(velocity, direction);
        }
    }
}
