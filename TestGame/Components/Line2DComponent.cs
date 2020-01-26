using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class Line2DComponent : BaseComponent<Line2DComponent>
    {
        public Line2DComponent(IEntity entity, IEntity ToEntity, Color color) : base(entity)
        {
            this.ToEntity = ToEntity;
            Color = color;
        }

        public IEntity ToEntity { get; }
        public Color Color { get; }
    }
}
