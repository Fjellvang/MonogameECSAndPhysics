using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;
using System;

namespace MyGame.TestGame.Components.SpriteComponents
{
    public sealed class LineEntityToEntityComponent : LineComponentBase
    {
        public LineEntityToEntityComponent(IEntity entity, IEntity ToEntity, Color color) : base(entity,color)
        {
            this.toEntity = ToEntity;
        }

        public override void CalculateLine(out Vector2 drawFrom, out float distance, out float angle)
        {
            drawFrom = Entity.Position.ToVector2();
            var point2 = toEntity.Position.ToVector2();
            distance = Vector2.Distance(drawFrom, point2);
            // calculate the angle between the two vectors
            angle = (float)Math.Atan2(point2.Y - drawFrom.Y, point2.X - drawFrom.X);
        }

        public IEntity toEntity { get; }
        public Color color { get; }
    }
}
