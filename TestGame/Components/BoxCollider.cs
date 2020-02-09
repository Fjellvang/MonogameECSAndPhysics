using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;

namespace MyGame.TestGame.Components
{
    public sealed class BoxCollider : ColliderComponent
    {
        public BoxCollider(IEntity entity, Vector2 width, Vector2 height) : base(entity)
        {
            Width = width;
            Height = height;
            var V0 = Vector2.Zero;
            var V1 = width;
            var V2 = width + height;
            var V3 = height;
            Center = new Vector2((width * .5f).Length(), (height * .5f).Length());
            vertices = new Vector2[] { V0, V1, V2, V3 };
        }

        public Vector2 Width { get; }
        public Vector2 Height { get; }
        public Vector2 Center { get; }


        public Vector2[] vertices;
        public Vector2[] Vertices(Vector2 pos, Matrix nextRotation)
        {
            var retval = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                retval[i] = pos + Vector2.Transform(vertices[i], nextRotation);
            }
            return retval;
        }

        public Vector2[] Normals(Matrix nextRotation)
        {
            var retval = new Vector2[2];
            retval[0] = (Vector2.Transform(vertices[1] - vertices[0], nextRotation)).ToLeftTurnedNormal();
            retval[1] = (Vector2.Transform(vertices[2] - vertices[1], nextRotation)).ToLeftTurnedNormal();
            //retval[2] = (Vector2.Transform(vertices[3] - vertices[2], this.Entity.Rotation)).ToLeftTurnedNormal();
            //retval[3] = (Vector2.Transform(vertices[0] - vertices[3], this.Entity.Rotation)).ToLeftTurnedNormal();
            return retval;
        }

        public override bool CollidesWith(Vector2 nextPos, Matrix nextRotation, ColliderComponent collider, out Vector2? point)
        {
            if (collider is BoxCollider other)
            {
                var axes = this.Normals(nextRotation);
                var otherAxes = other.Normals(other.Entity.Rotation);
                //var thispos = Entity.Position.ToVector2();
                var thatpos = collider.Entity.Position.ToVector2();
                var one = !CollisionOnAxis(this.Vertices(nextPos, nextRotation), other.Vertices(thatpos, other.Entity.Rotation), axes[0]);
                var two = !CollisionOnAxis(this.Vertices(nextPos, nextRotation), other.Vertices(thatpos, other.Entity.Rotation), axes[1]);
                var three = !CollisionOnAxis(this.Vertices(nextPos, nextRotation), other.Vertices(thatpos, other.Entity.Rotation), otherAxes[0]);
                var four = !CollisionOnAxis(this.Vertices(nextPos, nextRotation), other.Vertices(thatpos, other.Entity.Rotation), otherAxes[1]);
                point = null;
                if (one || two || three || four)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            point = null;
            return false;
        }

        private bool CollisionOnAxis(Vector2[] box1Points, Vector2[] box2Points, Vector2 axis)
        {
            float box1MinDot = Vector2.Dot(box1Points[0], axis);
            float box1MaxDot = Vector2.Dot(box1Points[0], axis);

            float box2MinDot = Vector2.Dot(box2Points[0], axis);
            float box2MaxDot = Vector2.Dot(box2Points[0], axis);
            for (int i = 1; i < 4; i++)
            {
                var box1Proj = Vector2.Dot(box1Points[i], axis);
                var box2Proj = Vector2.Dot(box2Points[i], axis);
                if (box1Proj < box1MinDot)
                {
                    box1MinDot = box1Proj;
                } else
                if (box1Proj > box1MaxDot)
                {
                    box1MaxDot = box1Proj;
                }

                if (box2Proj < box2MinDot)
                {
                    box2MinDot = box2Proj;
                } else
                if (box2Proj > box2MaxDot)
                {
                    box2MaxDot = box2Proj;
                }
            }

            if ((box1MinDot > box2MaxDot || box2MinDot > box1MaxDot))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
