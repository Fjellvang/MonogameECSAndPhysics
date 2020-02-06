using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;

namespace MyGame.TestGame.Components
{
    public class BoxCollider : ColliderComponent
    {
        public BoxCollider(IEntity entity, Vector2 width, Vector2 height) : base(entity)
        {
            Width = width;
            Height = height;
        }

        public Vector2 Width { get; }
        public Vector2 Height { get; }

        public override bool CollidesWith(ColliderComponent collider, out Vector2 point)
        {
            throw new NotImplementedException();
        }
    }
}
