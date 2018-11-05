using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public class Evade : SteeringBase
    {
        private Flee flee;

        public void Start()
        {
            flee = GetComponent<Flee>();
        }
        
        public override Vector3 GetSteering(ISteeringData steeringData) //Vector3 position, Vector3 velocity, Vector3 target)
        {
            if (steeringData.Position == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Position is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            } if (steeringData.Velocity == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Velocity is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            } if (steeringData.Target == null) {
                throw new System.NullReferenceException(string.Format("{0}: steeringData.Target is null", System.Reflection.MethodBase.GetCurrentMethod().Name));
            }

            Vector3 distance = steeringData.Target.Value - steeringData.Position.Value;
            float fleeDistance = distance.magnitude / maxVelocity;

            Vector3 futurePosition = steeringData.Position.Value + steeringData.Velocity.Value * fleeDistance;
            return flee.GetSteering(new SteeringDataBase(steeringData.Position, steeringData.Velocity, futurePosition));
        }
    }
}
