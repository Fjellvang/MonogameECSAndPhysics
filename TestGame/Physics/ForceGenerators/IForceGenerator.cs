using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.ForceGenerators
{
    public interface IForceGenerator
    {
        void ApplyForce(SimulationObject simulationObject);
    }
}
