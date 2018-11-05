using UnityEngine;

namespace Assets.Scripts.Steering.SteeringData
{
    public interface ISteeringData
    {
        Vector3? Position    { get; set; }
        Vector3? Velocity    { get; set; }
        Vector3? Target      { get; set; }
    }
}
