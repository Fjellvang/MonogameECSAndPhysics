using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.Integrators
{
    public abstract class Integrators
    {
        Game game;

        public float FixedTimeStep { get; set; }

        public Integrators(Game game)
        {
            this.game = game;

            this.FixedTimeStep = (float)game.TargetElapsedTime.TotalSeconds;
        }

        public abstract void Integrate(Vector3 acceleration, SimulationObject simulationObject);
    }
}
