using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    /// <summary>
    /// Base Steering behaviour interface
    /// </summary>
    public interface ISteer
    {
        /// <summary>
        /// Getter for respective steering behaviour vector;
        /// 
        /// Note:   As the ISteeringData Vector3 fields are null by default
        ///         implementations need to validate whatever input they are
        ///         using is not null!
        /// </summary>
        /// <param name="steeringData">SteeringData container</param>
        /// <returns></returns>
        Vector3 GetSteering(ISteeringData steeringData);
    }
}
