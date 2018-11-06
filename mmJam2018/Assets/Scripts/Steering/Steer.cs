using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    public class Steer : SteeringBase
    {
        private Seek                seek;
        private Arrive              arrive;
        private Flee                flee;
        private Wander              wander;
        private Pursue              pursue;
        private CollisionAvoid      collisionAvoid;

        // Let's say we arrive and seek normally every time
        //private const float         seekProbability;
        //private const float         arriveProbability;
        private const float         collisionAvoidProbability   = 0.9f;
        private const float         pursueProbability           = 0.85f;
        
        // Fleeing and pursuing on the same object might be weird
        private const float         fleeProbability             = 0.5f;
        private const float         wanderProbability           = 0.5f;

        [SerializeField]
        private Vector3 velocity;

        [SerializeField]
        protected float maxForce = 0.2f;

        private void Start()
        {
            velocity        = Vector3.zero;
            seek            = GetComponent<Seek>();
            flee            = GetComponent<Flee>();
            arrive          = GetComponent<Arrive>();
            pursue          = GetComponent<Pursue>();
            wander          = GetComponent<Wander>();
            collisionAvoid  = GetComponent<CollisionAvoid>();
        }

        public override Vector3 GetSteering(ISteeringData steeringData)
        {
            // Do all the null checking, for whatever component might be missing (as these should be pluggable-in)

            // Dithering goes here - maybe move in separate method actually
            throw new System.NotImplementedException();
        }

    }
}
