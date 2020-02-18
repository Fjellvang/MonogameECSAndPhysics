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
            point = null;
            var colliding = false;
            switch (collider)
            {
                case PolygonCollider other:
                    var thisnormals = this.Normals(nextRotation);
                    var thatNormals = collider.Normals(collider.Entity.Transform.Rotation);
                    var thatPos = other.Entity.Transform.Position.ToVector2();
                    for (int i = 0; i < thisnormals.Length; i++)
                    {
                        if (!CollisionOnAxis(this.Vertices(nextPosition,nextRotation), other.Vertices(thatPos, other.Entity.Transform.Rotation), thisnormals[i]))
                        {
                            point = null;
                            return false;
                        }
                    }
                    for (int i = 0; i < thatNormals.Length; i++)
                    {
                        if (!CollisionOnAxis(this.Vertices(nextPosition,nextRotation), other.Vertices(thatPos, other.Entity.Transform.Rotation), thatNormals[i]))
                        {
                            point = null;
                            return false;
                        }
                    }
                    colliding = true;
                    throw new NotImplementedException();
                    //TODO:set point
                    point = new MTV();
                    break;
                case BoxCollider other:
                    colliding = other.CollisionBoxToPolygon(this, other, nextPosition, nextRotation, other.Entity.Transform.Position.ToVector2(), other.Entity.Transform.Rotation, out point);
                    break;
            }
            if (colliding)
            {
                //TODO: refactor this bad shiet.
                var delta = collider.Entity.Transform.Position - this.Entity.Transform.Position;
                var dot = Vector2.Dot(point.Value.Axis, delta.ToVector2());
                point = new MTV(point.Value.Axis * Math.Sign(dot), point.Value.Magnitude);
            }
            return colliding;
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
