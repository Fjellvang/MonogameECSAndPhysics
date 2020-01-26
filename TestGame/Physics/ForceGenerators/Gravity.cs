using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.ForceGenerators
{
    public class Gravity : IForceGenerator
    {
        public Vector3 Acceleration { get; set; }
        public Gravity() : this(new Vector3(0,9.81f,0))
        {
            
        }
        public Gravity(Vector3 acceleration)
        {
            this.Acceleration = acceleration;
        }
        public void ApplyForce(SimulationObject simulationObject)
        {
            simulationObject.ResultantForce += simulationObject.Mass * Acceleration;
        }
    }
}
