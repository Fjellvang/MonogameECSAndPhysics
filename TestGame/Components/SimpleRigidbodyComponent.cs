using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class SimpleRigidbodyComponent : BaseComponent<SimpleRigidbodyComponent>
    {
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public SimpleRigidbodyComponent(IEntity entity) : base(entity)
        {
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
        }
    }
}
