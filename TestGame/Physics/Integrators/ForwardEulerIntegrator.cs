using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyGame.TestGame.Physics.Integrators
{
    public class ForwardEulerIntegrator : Integrator
    {
        public ForwardEulerIntegrator(Game game) : base(game)
        {
        }

        public override void Integrate(Vector3 acceleration, SimulationObject simulationObject)
        {
            simulationObject.CurrentPosition += simulationObject.CurrentVelocity * FixedTimeStep;
            simulationObject.CurrentVelocity += acceleration * FixedTimeStep;
        }
    }
}
