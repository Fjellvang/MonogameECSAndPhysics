using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components.ColliderComponents
{
    public abstract class ColliderBaseComponent : BaseComponent<ColliderBaseComponent>
    {
        public ColliderBaseComponent(IEntity entity) : base(entity)
        {
        }
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
        public abstract Vector2[] Normals(Matrix nextRotation);

        public abstract bool CollidesWith(Vector2 nextPosition, Matrix nextRotation, ColliderBaseComponent collider, out MTV? point);

        protected Projection GetProjectionOnAxis(Vector2[] vertices, Vector2 axis)
        {
            axis.Normalize();
            float min = Vector2.Dot(vertices[0], axis);
            float max = min;
            for (int i = 1; i < vertices.Length; i++)
            {
                var projection = Vector2.Dot(vertices[i], axis);
                if (projection < min)
                {
                    min = projection;
                }
                else if (projection > max)
                {
                    max = projection;
                }
            }
            return new Projection(min, max);
        }
        protected bool CollisionOnAxis(Vector2[] box1Points, Vector2[] box2Points, Vector2 axis)
        {
            var proj1 = GetProjectionOnAxis(box1Points, axis);
            var proj2 = GetProjectionOnAxis(box2Points, axis);
            return proj1.Overlaps(proj2);
        }
    }
    /// <summary>
    /// Minimum translation vector...
    /// </summary>
    public struct MTV
    {
        public MTV(Vector2 axis, float magnitude)
        {
            Axis = axis;
            Magnitude = magnitude;
        }

        public Vector2 Axis { get; }
        public float Magnitude { get; }
    }
    public struct Projection
    {
        private readonly float min;
        private readonly float max;

        public Projection(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public bool Overlaps(Projection other)
        {
            return !(min > other.max || other.min > max);
        }
        public float GetOverlap(Projection other)
        {
            if (!Overlaps(other))
            {
                return 0;
            }
            return min - other.max;
        }
    }
}
