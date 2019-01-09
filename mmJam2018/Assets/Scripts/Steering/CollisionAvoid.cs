using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public class CollisionAvoid : SteeringBase
    {
        [SerializeField]
        private float AvoidanceRangeScale = 1;
        [SerializeField]
        private float MaxAvoidanceForce = 0.5f;

        /// <inheritdoc/>
        public override Vector3 GetSteering(ISteeringData steeringData) //Transform transform, Vector3 velocity, Vector3 target)
        {
            if (steeringData.Position == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Position is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            } if (steeringData.Velocity == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Velocity is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            } if (steeringData.Target == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Target is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            }

            Vector3 direction = (steeringData.Target.Value - steeringData.Position.Value).normalized;
            RaycastHit hitInfo = new RaycastHit();

            if(Physics.Raycast(steeringData.Position.Value, steeringData.Target.Value * AvoidanceRangeScale, out hitInfo, 2))
            {
                // There is probably a better way to do this than just any firm colliders
                // Interface, as usual?
                if(!hitInfo.collider.isTrigger && hitInfo.transform != transform)
                {
                    //Debug.Log("Raycast detected collision w/ " + hitInfo.collider.name);
                    //Debug.Log("Old direction " + direction);

                    direction = hitInfo.normal.normalized;
                    direction = Vector3.Scale(direction, new Vector3(MaxAvoidanceForce, MaxAvoidanceForce, MaxAvoidanceForce));

                    // FIXME:
                    //
                    // Surprisingly, this basic resetting to 0 works well.
                    //
                    // However, probably need a way of rotating the normal
                    // 90 or -90 degrees around x, maybe?
                    //
                    // Because now it keeps within bounds by backing up,
                    // not by trying to collision avoid, but maintain course.
                    direction.z = 0;
                    //Debug.Log("New direction " + direction);
                }
            }

            return AddForce(steeringData.Velocity.Value, direction);
        }
    }
}
