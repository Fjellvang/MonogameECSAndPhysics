using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class TransformComponent : BaseComponent<TransformComponent>
    {
        public Vector2 Position { get; set; }
        public TransformComponent(IEntity entity, Vector2 position) : base(entity)
        {
            Position = position;
        }
    }
}
