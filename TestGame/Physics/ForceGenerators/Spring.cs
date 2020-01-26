using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.ForceGenerators
{
    public class Spring : IForceGenerator
    {
        /// <summary>
        /// AKA Spring constant. Determines the spring stiffness. The higher the value, the tigther the spring, IE the faster it will return to its rest state.
        /// </summary>
        public float Stiffness { get; set; }
        /// <summary>
        /// Amount of internal foce the spring will experience. Paralesses to directon of spring
        /// </summary>
        public float Damping { get; set; }
        /// <summary>
        /// The lengtht the spring will try to maintain when undergoing compression or strecthing
        /// </summary>
        public float RestLength { get; set; }
        /// <summary>
        /// First of the two connected objects
        /// </summary>
        public SimulationObject SimulationObjectA { get; set; }
        /// <summary>
        /// The other connected object
        /// </summary>
        public SimulationObject SimulationObjectB { get; set; }
        public Spring(float stiffness, float damping, SimulationObject objA, SimulationObject objB) : this(stiffness, damping, objA, objB, (objB.CurrentPosition - objA.CurrentPosition).Length())
        {

        }
        public Spring(float stiffness, float damping, SimulationObject objA, SimulationObject objB, float restLength)
        {
            Stiffness = stiffness;
            Damping = damping;
            SimulationObjectA = objA;
            SimulationObjectB = objB;
            RestLength = restLength;
        }
        //Vector3 direction;

        public void ApplyForce(SimulationObject simulationObject)
        {
            var direction = SimulationObjectA.CurrentPosition - SimulationObjectB.CurrentPosition;
            if (direction != Vector3.Zero)
            {
                var currentLength = direction.Length();
                direction.Normalize();
                //add spring force
                var force = -Stiffness * ((currentLength - RestLength) * direction);

                //add spring damping force
                force += -Damping * Vector3.Dot(SimulationObjectA.CurrentVelocity - SimulationObjectB.CurrentVelocity, direction) * direction;

                SimulationObjectA.ResultantForce += force;
                SimulationObjectB.ResultantForce += -force;

            }
        }
    }
}
