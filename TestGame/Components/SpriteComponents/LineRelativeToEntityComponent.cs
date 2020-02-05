using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;

namespace MyGame.TestGame.Components.SpriteComponents
{
    public class LineRelativeToEntityComponent : LineComponentBase
    {
        private readonly Vector2 point1;
        private readonly Vector2 point2;

        public LineRelativeToEntityComponent(IEntity entity, Vector2 point1, Vector2 point2) : base(entity,Color.AliceBlue)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        public override void CalculateLine(out Vector2 drawFrom, out float distance, out float angle)
        {
            drawFrom = Entity.Position.ToVector2() + point1;//Entity.Position.ToVector2();
            //var t = this.point2 - this.point1;
            //var drawTo = this.point2 - this.point1;
            distance = Vector2.Distance(this.point2, this.point1);
            // calculate the angle between the two vectors
            angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        }
    }
}
