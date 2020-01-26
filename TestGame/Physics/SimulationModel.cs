using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyGame.TestGame.Physics
{
    public sealed class SimulationModel : SimulationObject
    {
        public SimulationModel(float mass, SimulationObjectType objectType) : base(mass, objectType)
        {
        }

        public override void Update(GameTime time)
        {
            //TODO: This i probably redundant... for now atleast
        }
    }
}
