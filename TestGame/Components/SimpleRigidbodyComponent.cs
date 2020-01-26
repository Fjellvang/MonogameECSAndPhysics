using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using MyGame.TestGame.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class SimpleRigidbodyComponent : BaseComponent<SimpleRigidbodyComponent>
    {
        //public Vector2 Velocity { get; set; }
        //public Vector2 Acceleration { get; set; }
        public SimulationObject SimulationObject { get; set; }
        public SimpleRigidbodyComponent(IEntity entity, SimulationObject simulationObject) : base(entity)
        {

            simulationObject.CurrentPosition = entity.GetComponent<TransformComponent>().Position;

            this.SimulationObject = simulationObject;
            //Velocity = Vector2.Zero;
            //Acceleration = Vector2.Zero;
        }
    }
}
