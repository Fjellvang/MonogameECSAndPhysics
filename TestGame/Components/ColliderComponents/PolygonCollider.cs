using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;

namespace MyGame.TestGame.Components.ColliderComponents
{
    public class PolygonCollider : ColliderBaseComponent
    {
        public PolygonCollider(IEntity entity, Vector2[] vertices) : base(entity)
        {
            this.vertices = vertices;
        }

        public override bool CollidesWith(Vector2 nextPosition, Matrix nextRotation, ColliderBaseComponent collider, out MTV? point)
        {
            switch (collider)
            {
                case PolygonCollider other:
                    var thisnormals = this.Normals(nextRotation);
                    var thatNormals = collider.Normals(collider.Entity.Rotation);
                    var thatPos = other.Entity.Position.ToVector2();
                    for (int i = 0; i < thisnormals.Length; i++)
                    {
                        if (!CollisionOnAxis(this.Vertices(nextPosition,nextRotation), other.Vertices(thatPos, other.Entity.Rotation), thisnormals[i]))
                        {
                            point = null;
                            return false;
                        }
                    }
                    for (int i = 0; i < thatNormals.Length; i++)
                    {
                        if (!CollisionOnAxis(this.Vertices(nextPosition,nextRotation), other.Vertices(thatPos, other.Entity.Rotation), thatNormals[i]))
                        {
                            point = null;
                            return false;
                        }
                    }
                    point = null;
                    return true;
                case BoxCollider other:
                    return other.CollisionBoxToPolygon(this, other, nextPosition, nextRotation, other.Entity.Position.ToVector2(), other.Entity.Rotation, out point);
            }
            point = null;
            return false;
        }

        public override Vector2[] Normals(Matrix nextRotation)
        {
            var retval = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length-1; i++)
            {
                retval[i] = (Vector2.Transform(vertices[i+1] - vertices[i], nextRotation)).ToLeftTurnedNormal();
            }
            retval[vertices.Length-1] = (Vector2.Transform(vertices[vertices.Length - 1] - vertices[0], nextRotation)).ToLeftTurnedNormal();
            return retval;
        }
    }
}
