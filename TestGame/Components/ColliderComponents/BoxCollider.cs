using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;

namespace MyGame.TestGame.Components.ColliderComponents
{
    public sealed class BoxCollider : ColliderBaseComponent
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

        public override Vector2[] Normals(Matrix nextRotation)
        {
            var retval = new Vector2[2];
            retval[0] = (Vector2.Transform(vertices[1] - vertices[0], nextRotation)).ToLeftTurnedNormal();
            retval[1] = (Vector2.Transform(vertices[2] - vertices[1], nextRotation)).ToLeftTurnedNormal();
            return retval;
        }

        public override bool CollidesWith(Vector2 nextPos, Matrix nextRotation, ColliderBaseComponent collider, out Vector2? point)
        {
            switch (collider)
            {
                case BoxCollider other:
                    var axes = this.Normals(nextRotation);
                    var otherAxes = other.Normals(other.Entity.Rotation);
                    var thatpos = collider.Entity.Position.ToVector2();
                    for (int i = 0; i < 2; i++)
                    {
                        if (!CollisionOnAxis(this.Vertices(nextPos, nextRotation), other.Vertices(thatpos, other.Entity.Rotation), axes[i])
                         || !CollisionOnAxis(this.Vertices(nextPos, nextRotation), other.Vertices(thatpos, other.Entity.Rotation), otherAxes[i]))
                        {
                            //If just one of the axis returns no collision. well then we dont have one, we can stop computation
                            point = null;//todo
                            return false;
                        }
                    }
                    //No collision returned false. we are colliding
                    point = null;
                    return true;
                case PolygonCollider other:
                    point = null;
                    return CollisionBoxToPolygon(other, this, other.Entity.Position.ToVector2(), other.Entity.Rotation, nextPos, nextRotation);
                default:
                    break;
            }
            point = null;
            return false;
        }
        public bool CollisionBoxToPolygon(
            PolygonCollider polygonCollider, BoxCollider boxCollider, Vector2 polygondNextPos, Matrix polygonNextRotation, Vector2 boxNextPos, Matrix BoxNextRot)
        {
            var polygonAxes = polygonCollider.Normals(polygonNextRotation);
            var boxNormals = boxCollider.Normals(polygonNextRotation);
            //var thispos = Entity.Position.ToVector2();
            for (int i = 0; i < polygonAxes.Length; i++)
            {
                if (!CollisionOnAxis(polygonCollider.Vertices(polygondNextPos, polygonNextRotation), boxCollider.Vertices(boxNextPos, BoxNextRot), polygonAxes[i]))
                {
                    return false;
                }
            }
            for (int i = 0; i < boxNormals.Length; i++)
            {
                if (!CollisionOnAxis(polygonCollider.Vertices(polygondNextPos, polygonNextRotation), boxCollider.Vertices(boxNextPos, BoxNextRot), boxNormals[i]))
                {
                    return false;
                }
            }
            return true;
        }

        //private bool CollisionOnAxis(Vector2[] box1Points, Vector2[] box2Points, Vector2 axis)
        //{
        //    float box1MinDot = Vector2.Dot(box1Points[0], axis);
        //    float box1MaxDot = Vector2.Dot(box1Points[0], axis);

        //    float box2MinDot = Vector2.Dot(box2Points[0], axis);
        //    float box2MaxDot = Vector2.Dot(box2Points[0], axis);
        //    for (int i = 1; i < 4; i++)
        //    {
        //        var box1Proj = Vector2.Dot(box1Points[i], axis);
        //        var box2Proj = Vector2.Dot(box2Points[i], axis);
        //        if (box1Proj < box1MinDot)
        //        {
        //            box1MinDot = box1Proj;
        //        } else
        //        if (box1Proj > box1MaxDot)
        //        {
        //            box1MaxDot = box1Proj;
        //        }

        //        if (box2Proj < box2MinDot)
        //        {
        //            box2MinDot = box2Proj;
        //        } else
        //        if (box2Proj > box2MaxDot)
        //        {
        //            box2MaxDot = box2Proj;
        //        }
        //    }

        //    if ((box1MinDot > box2MaxDot || box2MinDot > box1MaxDot))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
