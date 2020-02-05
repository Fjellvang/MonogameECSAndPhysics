using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components.SpriteComponents
{
    public abstract class LineComponentBase : BaseComponent<LineComponentBase>
    {
        public readonly Color color;

        public LineComponentBase(IEntity entity, Color color) : base(entity)
        {
            this.color = color;
        }

        public abstract void CalculateLine(out Vector2 drawFrom, out float distance, out float angle);

    }
}
