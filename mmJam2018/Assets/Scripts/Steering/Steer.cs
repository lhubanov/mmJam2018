using UnityEngine;
using Assets.Scripts.Steering.SteeringData;

namespace Assets.Scripts.Steering
{
    // FIXME:   Move all of this to separate steering Prefab/Component, which has all scripts attached
    //          So that it can be plugged on top of a separate Enemy Controller script (which does things
    //          like e.g. checking collider interactions and just calls getSteering to wander etc.)
    public class Steer : SteeringBase
    {
        private Seek                seek;
        private Arrive              arrive;
        private Flee                flee;
        private Wander              wander;
        private Pursue              pursue;
        private CollisionAvoid      collisionAvoid;

        /// <summary>
        /// Probabilities, used for Prioritized Dithering method
        /// for combining steering behaviours
        /// </summary>

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
        //[SerializeField]
        //private float maxSteeringForce = 0.2f;


        /// <summary>
        /// Running total, determining whether or not
        /// more steering behaviours can be calculated,
        /// allowing for optimizing out low-priority behaviours.
        /// 
        /// Used for the Weighed Truncated Running Sum with Prioritization
        /// method for combining steering behaviours.
        /// </summary>
        [SerializeField]
        private Vector3 steeringMaxForce = new Vector3 (0.8f, 0.8f, 0.8f);


        // Public flags to enable/disable specific behaviour (e.g. chasing or fleeing)
        public bool fleeing             = false;
        public bool pursuing            = false;
        // Wander & avoid collisions, until specifically told otherwise
        public bool wandering           = true;
        public bool avoidCollisions     = true;

        private bool On(ISteer steeringBehaviour)
        {
            return (steeringBehaviour != null);
        }

        private bool AccumulateForce(ref Vector3 runningTotal, Vector3 forceToAdd)
        {
            float currentMagnitude = runningTotal.magnitude;
            float remainingMagnitude = steeringMaxForce.magnitude - currentMagnitude;

            if(remainingMagnitude <= 0) {
                return false;
            }

            if (forceToAdd.magnitude < remainingMagnitude) {
                runningTotal += forceToAdd;
            } else {
                // Step-through to get a better idea

                // This adds as much as possible of the forceToAdd, without going over
                // the remaining magnitude
                runningTotal += (forceToAdd.normalized * remainingMagnitude);
            }

            return true;
        }

        //Start() and Awake() are not called on Prefabs (:O took me a year to find that out)
        public void Initialize()
        {
            velocity        = Vector3.zero;
            arrive          = GetComponent<Arrive>();
            seek            = GetComponent<Seek>();

            flee            = GetComponent<Flee>();
            pursue          = GetComponent<Pursue>();
            wander          = GetComponent<Wander>();
            collisionAvoid  = GetComponent<CollisionAvoid>();
        }

        // FIXME: Test if the velocity adding works as expected & make steering prefab

        // Currently trying out Weighed Truncated Running Sum with Prioritization
        // Test how Prioritized Dithering works as well (abstract each into separate method)
        public override Vector3 GetSteering(ISteeringData steeringData)
        {
            velocity = Vector3.zero;
            Vector3 forceToAdd = Vector3.zero;

            // Note: there's probably a way to avoid this copy-pasting via delegates or sth
            if (avoidCollisions && On(collisionAvoid))
            {
                forceToAdd = collisionAvoid.GetSteering(steeringData);

                if(AccumulateForce(ref velocity, forceToAdd)) {
                    AddForce(steeringData.Velocity.Value, velocity);
                }
            }

            if(pursuing && On(pursue))
            {
                forceToAdd = pursue.GetSteering(steeringData);

                if (AccumulateForce(ref velocity, forceToAdd)) {
                    AddForce(steeringData.Velocity.Value, velocity);
                }
            }

            if (fleeing && On(flee))
            {
                forceToAdd = flee.GetSteering(steeringData);

                if (AccumulateForce(ref velocity, forceToAdd)) {
                    AddForce(steeringData.Velocity.Value, velocity);
                }
            }

            if (wandering && On(wander))
            {
                forceToAdd = wander.GetSteering(steeringData);

                if (AccumulateForce(ref velocity, forceToAdd)) {
                    AddForce(steeringData.Velocity.Value, velocity);
                }
            }

            // Perhaps Arrive goes here? (and is somehow not subject to the
            // running total prioritization?)

            //return velocity;
            return AddForce(steeringData.Velocity.Value, velocity);
        }
    }
}
