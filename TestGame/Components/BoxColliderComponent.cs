using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class BoxColliderComponent : ColliderComponent
    {
        public Rectangle Bounds { get; set; }
        public BoxColliderComponent(IEntity entity, Rectangle rectangle) : base(entity)
        {
            Bounds = rectangle;
        }
    }
}
