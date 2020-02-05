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
        private Matrix LastRot;

        public LineRelativeToEntityComponent(IEntity entity, Vector2 point1, Vector2 point2) : base(entity,Color.DarkBlue)
        {
            this.point1 = point1;
            this.point2 = point2;
            LastRot = entity.Rotation;
        }

        public override void CalculateLine(out Vector2 drawFrom, out float distance, out float angle)
        {
            drawFrom = Entity.Position.ToVector2() + point1;//Entity.Position.ToVector2();
                                                            //TESS
            if (LastRot != Entity.Rotation)
            {
                drawFrom = Vector2.Transform(drawFrom, Entity.Rotation);
                this.point1 = Vector2.Transform(point1, Entity.Rotation);
                this.point2 = Vector2.Transform(point2, Entity.Rotation);
                LastRot = Entity.Rotation;
            }
            distance = Vector2.Distance(this.point2, this.point1);
            // calculate the angle between the two vectors
            angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        }
    }
}
