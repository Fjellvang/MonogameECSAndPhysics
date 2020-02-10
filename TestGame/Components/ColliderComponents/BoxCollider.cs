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

        public override bool CollidesWith(Vector2 nextPos, Matrix nextRotation, ColliderBaseComponent collider, out MTV? point)
        {
            switch (collider)
            {
                case BoxCollider other:
                    var axis = this.Normals(nextRotation);
                    var otherAxes = other.Normals(other.Entity.Rotation);
                    var thatpos = collider.Entity.Position.ToVector2();
                    float overlap = float.MaxValue;
                    Vector2 smallestAxes = Vector2.Zero;
                    for (int i = 0; i < 2; i++)
                    {
                        var verts = this.Vertices(nextPos, nextRotation);
                        var otherVerts = other.Vertices(thatpos, other.Entity.Rotation);
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

    }
}
