using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.TestGame.Components;

namespace MyGame.TestGame.Physics.Integrators
{
    public class VerletNoVelocityIntegrator : Integrator
    {
        private float drag;

        public float Drag
        {
            get { return drag; }
            set {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentException("drag must be between 0 and 1");
                }
                drag = value; 
            }
        }
        
        public VerletNoVelocityIntegrator(Game game) : this(game, 0.005f)
        {
        }

        public VerletNoVelocityIntegrator(Game game, float drag) : base(game)
        {
            Drag = drag;
        }

        public override void Integrate(Vector2 acceleration, float angularAcceleraion, RigidBodyComponent simulationObject, float dt)
        {
            var newPos = (2 - drag) * simulationObject.CurrentPosition
                - (1 - drag) * simulationObject.PreviousPosition
                + acceleration * dt * dt;

            simulationObject.CurrentVelocity += acceleration * dt;
            simulationObject.PreviousPosition = simulationObject.CurrentPosition;
            simulationObject.CurrentPosition = newPos;

            var newAngle = (2 - drag) * simulationObject.CurrentAngle
                - (1 - drag) * simulationObject.PreviousAngle
                + angularAcceleraion * dt * dt;

            simulationObject.CurrentAngularVelocity += angularAcceleraion * dt;
            simulationObject.PreviousAngle = simulationObject.CurrentAngularVelocity;
            simulationObject.CurrentAngle = newAngle;
        }

    }
}
