using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            V0 = Vector2.Zero;
            V1 = width;
            V2 = width + height;
            V3 = height;
            Center = new Vector2((width * .5f).Length(), (height * .5f).Length());
            vertices = new Vector2[] { V0, V1, V2, V3 };
        }

        public Vector2 Width { get; }
        public Vector2 Height { get; }
        public Vector2 Center { get; }

        //Vertices...
        public Vector2 V0;
        public Vector2 V1;
        public Vector2 V2;
        public Vector2 V3;

        public Vector2[] vertices;
        public IEnumerable<Vector2> Vertices()
        {
            var pos = this.Entity.Position.ToVector2();
            for (int i = 0; i < vertices.Length; i++)
            {
                yield return pos + vertices[i];
            }
        }

        public Vector2[] Normals()
        {
            var retval = new Vector2[2];
            retval[0] = (Vector2.Transform(vertices[1] - vertices[0], this.Entity.Rotation)).ToLeftTurnedNormal();
            retval[1] = (Vector2.Transform(vertices[2] - vertices[1], this.Entity.Rotation)).ToLeftTurnedNormal();
            //retval[2] = (Vector2.Transform(vertices[3] - vertices[2], this.Entity.Rotation)).ToLeftTurnedNormal();
            //retval[3] = (Vector2.Transform(vertices[0] - vertices[3], this.Entity.Rotation)).ToLeftTurnedNormal();
            return retval;
        }

        public override bool CollidesWith(ColliderComponent collider, out Vector2? point)
        {
            if (collider is BoxCollider other)
            {
                var axes = this.Normals();
                var otherAxes = other.Normals();
                var one = !CollisionOnAxis(this.Vertices().ToArray(), other.Vertices().ToArray(), axes[0]);
                var two = !CollisionOnAxis(this.Vertices().ToArray(), other.Vertices().ToArray(), axes[1]);
                var three = !CollisionOnAxis(this.Vertices().ToArray(), other.Vertices().ToArray(), otherAxes[0]);
                var four = !CollisionOnAxis(this.Vertices().ToArray(), other.Vertices().ToArray(), otherAxes[1]);
                if (one || two || three || four)
                {
                }
                else
                {
                    Debug.WriteLine("COLLISION" + DateTime.Now.Ticks);
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
