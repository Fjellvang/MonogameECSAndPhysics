using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.ForceGenerators
{
    /// <summary>
    /// Class to represent the medium an object is in. Its viscosity will determine the drag on the object. Ei higher drag for an object in honey vs in water
    /// </summary>
    public class Medium : IForceGenerator
    {
        public float DragCoefficient { get; set; }
        public Medium(float dragCoefficient)
        {
            DragCoefficient = dragCoefficient;
        }
        /// <summary>
        /// drag force = -drag coefficient * object velocity
        /// </summary>
        /// <param name="simulationObject"></param>
        public void ApplyForce(RigidBodyComponent simulationObject)
        {
            simulationObject.ResultantForce += -DragCoefficient * simulationObject.CurrentVelocity;
            simulationObject.ResultantAngularForce += -DragCoefficient * simulationObject.CurrentAngularVelocity;
        }
    }
}
