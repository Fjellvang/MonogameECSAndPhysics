using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;

namespace MyGame.TestGame.Components.SpriteComponents
{
    public class LineRelativeToEntityComponent : LineComponentBase
    {
        private Vector2 point1;
        private Vector2 point2;

        public LineRelativeToEntityComponent(IEntity entity, Vector2 point1, Vector2 point2, Color color = default) : base(entity,color)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public override void CalculateLine(out Vector2 drawFrom, out float distance, out float angle)
        {
                                                            //TESS
            //drawFrom = Vector2.Transform(drawFrom, Entity.Rotation);
            var p1 = Vector2.Transform(point1, Entity.Transform.Rotation);
            var p2 = Vector2.Transform(point2, Entity.Transform.Rotation);
            drawFrom = Entity.Transform.Position.ToVector2() + p1;//Entity.Position.ToVector2();
            distance = Vector2.Distance(p2, p1);
            // calculate the angle between the two vectors
            angle = (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X);
        }
    }
}
