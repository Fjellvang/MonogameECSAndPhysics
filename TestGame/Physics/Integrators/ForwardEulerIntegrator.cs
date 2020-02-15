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

        public override void Integrate(Vector2 acceleration,float angularAcceleration, RigidBodyComponent simulationObject)
        {
            simulationObject.PreviousPosition = simulationObject.CurrentPosition;
            simulationObject.CurrentPosition += simulationObject.CurrentVelocity * FixedTimeStep;
            simulationObject.CurrentVelocity += acceleration * FixedTimeStep;

            simulationObject.PreviousAngle = simulationObject.CurrentAngle;
            simulationObject.CurrentAngle += simulationObject.CurrentAngularVelocity * FixedTimeStep;
            simulationObject.CurrentAngularVelocity += angularAcceleration * FixedTimeStep;
        }
    }
}
