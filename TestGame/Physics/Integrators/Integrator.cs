using Microsoft.Xna.Framework;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.Integrators
{
    public abstract class Integrator
    {
        Game game;

        public float FixedTimeStep { get; set; }

        public Integrator(Game game)
        {
            this.game = game;

            this.FixedTimeStep = (float)game.TargetElapsedTime.TotalSeconds;
        }

        public abstract void Integrate(Vector3 acceleration,float angularAcceleration, RigidBodyComponent simulationObject);
    }
}
