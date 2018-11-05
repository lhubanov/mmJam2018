using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public class Pursue : SteeringBase
    {
        private Seek seeker;

        public void Start()
        {
            seeker = GetComponent<Seek>();
        }

        public override Vector3 GetSteering(ISteeringData steeringData) //Vector3 position, Vector3 velocity, Vector3 target)
        {
            Vector3 distance = steeringData.Target.Value - steeringData.Position.Value;
            float pursuitCoeff = distance.magnitude / maxVelocity;

            Vector3 futurePosition = steeringData.Position.Value + steeringData.Velocity.Value * pursuitCoeff;
            return seeker.GetSteering(new SteeringDataBase(steeringData.Position, steeringData.Velocity, futurePosition));
        }
    }
}
