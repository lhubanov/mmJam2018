using UnityEngine;

namespace Assets.Scripts.Steering
{
    public interface ISteer
    {
        Vector3 GetSteering(Vector3 position, Vector3 velocity);
        Vector3 GetSteering(Vector3 position, Vector3 velocity, Vector3 target);
    }
}
