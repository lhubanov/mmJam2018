using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public class Seek : SteeringBase
    {
        public override Vector3 GetSteering(ISteeringData steeringData)//Vector3 position, Vector3 velocity, Vector3 target)
        {
            if (steeringData.Position == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Position is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            } if (steeringData.Velocity == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Velocity is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            } if (steeringData.Target == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Target is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            }

            Vector3 distanceToTarget = steeringData.Target.Value - steeringData.Position.Value;
            Vector3 desiredVelocity = distanceToTarget.normalized * maxVelocity;

            Vector3 steering = desiredVelocity - steeringData.Velocity.Value;
            return AddForce(steeringData.Velocity.Value, steering);
        }
    }
}
