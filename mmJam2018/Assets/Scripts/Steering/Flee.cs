using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public class Flee : SteeringBase
    {
        private Seek Seeker;

        public void Start()
        {
            Seeker = GetComponent<Seek>();
        }

        public override Vector3 GetSteering(ISteeringData steeringData) //Vector3 position, Vector3 velocity, Vector3 target)
        {
            return -Seeker.GetSteering(new SteeringDataBase(steeringData.Position, steeringData.Velocity, steeringData.Target));
        }
    }
}
