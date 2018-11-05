using UnityEngine;

namespace Assets.Scripts.Steering.SteeringData
{
    public class SteeringDataBase : ISteeringData
    {
        public Vector3? Position    { get; set; }
        public Vector3? Velocity    { get; set; }
        public Vector3? Target      { get; set; }

        public SteeringDataBase()
        {
            Position    = null;
            Velocity    = null;
            Target      = null;
        }

        public SteeringDataBase(Vector3? position, Vector3? velocity, Vector3? target)
        {
            Position    = position;
            Velocity    = velocity;
            Target      = target;
        }
    }
}
