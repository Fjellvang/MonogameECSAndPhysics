using Microsoft.Xna.Framework;
using MyGame.TestGame.Components;
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
        public RigidBodyComponent SimulationObjectA { get; set; }
        /// <summary>
        /// The other connected object
        /// </summary>
        public RigidBodyComponent SimulationObjectB { get; set; }
        public Spring(float stiffness, float damping, RigidBodyComponent objA, RigidBodyComponent objB) : this(stiffness, damping, objA, objB, (objB.Entity.Position - objA.Entity.Position).Length())
        {

        }
        public Spring(float stiffness, float damping, RigidBodyComponent objA, RigidBodyComponent objB, float restLength)
        {
            Stiffness = stiffness;
            Damping = damping;
            SimulationObjectA = objA;
            SimulationObjectB = objB;
            RestLength = restLength;
        }
        //Vector3 direction;

        public void ApplyForce(RigidBodyComponent simulationObject)
        {
            var direction = (SimulationObjectA.Entity.Position - SimulationObjectB.Entity.Position).ToVector2();
            if (direction != Vector2.Zero)
            {
                var currentLength = direction.Length();
                direction.Normalize();
                //add spring force
                var force = -Stiffness * ((currentLength - RestLength) * direction);

                //add spring damping force
                var addition = -Damping * Vector2.Dot(SimulationObjectA.CurrentVelocity - SimulationObjectB.CurrentVelocity, direction) * direction;
                force += addition;

                SimulationObjectA.ResultantForce += force;
                SimulationObjectB.ResultantForce += -force;

            }
        }
    }
}
