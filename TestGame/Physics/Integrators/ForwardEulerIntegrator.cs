using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.TestGame.Components;

namespace MyGame.TestGame.Physics.Integrators
{
    public class ForwardEulerIntegrator : Integrator
    {
        public ForwardEulerIntegrator(Game game) : base(game)
        {
        }

        public override void Integrate(Vector3 acceleration, RigidBodyComponent simulationObject)
        {
            simulationObject.PreviousPosition = simulationObject.CurrentPosition;
            simulationObject.CurrentPosition += simulationObject.CurrentVelocity * FixedTimeStep;
            simulationObject.CurrentVelocity += acceleration * FixedTimeStep;
        }
    }
}
