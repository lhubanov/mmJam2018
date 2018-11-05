using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public interface ISteer
    {
        Vector3 GetSteering(ISteeringData steeringData);
    }
}
