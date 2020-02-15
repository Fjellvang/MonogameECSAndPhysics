using Microsoft.Xna.Framework;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.ForceGenerators
{
    public class Gravity : IForceGenerator
    {
        public Vector2 Acceleration { get; set; }
        public Gravity() : this(new Vector2(0,9.81f))
        {
            
        }
        public Gravity(float yGrav) : this(new Vector2(0,yGrav))
        {
            
        }
        public Gravity(Vector2 acceleration)
        {
            this.Acceleration = acceleration;
        }
        public void ApplyForce(RigidBodyComponent simulationObject)
        {
            simulationObject.ResultantForce += simulationObject.Mass * Acceleration;
        }
    }
}
