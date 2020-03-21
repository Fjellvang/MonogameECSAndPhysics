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
            var half = width / 2 + height / 2;
            var V0 = Vector2.Zero - half;
            var V1 = width - half;
            var V2 = width + height - half;
            var V3 = height - half;
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
            //retval[1] = (Vector2.Transform(vertices[3] - vertices[2], nextRotation)).ToLeftTurnedNormal();
            //retval[1] = (Vector2.Transform(vertices[0] - vertices[3], nextRotation)).ToLeftTurnedNormal();
            return retval;
        }

        public override bool CollidesWith(Vector2 nextPos, Matrix nextRotation, ColliderBaseComponent collider, out MTV? point)
        {
            Vector2 smallestAxes = Vector2.Zero;
            float overlap = float.MaxValue;
            var colliding = false;
            point = null;
            switch (collider)
            {
                case BoxCollider other:
                    var axis = this.Normals(nextRotation);
                    var otherAxes = other.Normals(other.Entity.Transform.Rotation);
                    var thatpos = collider.Entity.Transform.Position;
                    for (int i = 0; i < axis.Length; i++)
                    {
                        var verts = this.Vertices(nextPos, nextRotation);
                        var otherVerts = other.Vertices(thatpos, other.Entity.Transform.Rotation);
                        var proj1 = GetProjectionOnAxis(verts, axis[i]);
                        var proj2 = GetProjectionOnAxis(otherVerts, axis[i]);

                        if (!proj1.Overlaps(proj2))
                        {
                            point = null; //todo;
                            return false;
                        }
                        else
                        {
                            var o = proj1.GetOverlap(proj2);
                            if (o < overlap)
                            {
                                overlap = o;
                                smallestAxes = axis[i];
                            }
                        }

                        proj1 = GetProjectionOnAxis(verts, otherAxes[i]);
                        proj2 = GetProjectionOnAxis(otherVerts, otherAxes[i]);

                        if (!proj1.Overlaps(proj2))
                        {
                            point = null; 
                            return false;
                        }
                        else
                        {
                            var o = proj1.GetOverlap(proj2);
                            if (o < overlap)
                            {
                                overlap = o;
                                smallestAxes = otherAxes[i];
                            }
                        }
                    }
                    //No collision returned false. we are colliding
                    point = new MTV(smallestAxes, overlap);
                    colliding = true;
                    break;
                case PolygonCollider other:
                    colliding = CollisionBoxToPolygon(other, this, other.Entity.Transform.Position, other.Entity.Transform.Rotation, nextPos, nextRotation, out point);
                    break;
                default:
                    break;
            }
            if (colliding)
            {
                //TODO: refactor this bad shiet.
                var delta = collider.Entity.Transform.Position - this.Entity.Transform.Position;
                var dot = Vector2.Dot(point.Value.Axis, delta);
                point = new MTV(point.Value.Axis * Math.Sign(dot), point.Value.Magnitude);
            }
            return colliding;
        }
        public bool CollisionBoxToPolygon(
            PolygonCollider polygonCollider, BoxCollider boxCollider, Vector2 polygondNextPos, Matrix polygonNextRotation, Vector2 boxNextPos, Matrix BoxNextRot, out MTV? mtv)
        {
            var polygonAxes = polygonCollider.Normals(polygonNextRotation);
            var boxNormals = boxCollider.Normals(polygonNextRotation);
            Vector2 smallestAxes = Vector2.Zero;
            float overlap = float.MaxValue;
            for (int i = 0; i < polygonAxes.Length; i++)
            {
                var proj1 = GetProjectionOnAxis(polygonCollider.Vertices(polygondNextPos, polygonNextRotation), polygonAxes[i]);
                var proj2 = GetProjectionOnAxis(boxCollider.Vertices(boxNextPos, BoxNextRot), polygonAxes[i]);
                if (!proj1.Overlaps(proj2))
                {
                    mtv = null;
                    return false;
                }
                else
                {
                    var o = proj1.GetOverlap(proj2);
                    if (o < overlap)
                    {
                        overlap = o;
                        smallestAxes = polygonAxes[i];
                    }
                }
            }
            for (int i = 0; i < boxNormals.Length; i++)
            {

                var proj1 = GetProjectionOnAxis(polygonCollider.Vertices(polygondNextPos, polygonNextRotation), boxNormals[i]);
                var proj2 = GetProjectionOnAxis(boxCollider.Vertices(boxNextPos, BoxNextRot), boxNormals[i]);
                if (!proj1.Overlaps(proj2))
                {
                    mtv = null;
                    return false;
                }
                else
                {
                    var o = proj1.GetOverlap(proj2);
                    if (o < overlap)
                    {
                        overlap = o;
                        smallestAxes = boxNormals[i];
                    }
                }
            }
            mtv = new MTV(smallestAxes, overlap);
            return true;
        }

    }
}
