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

        public abstract bool CollidesWith(Vector2 nextPosition, Matrix nextRotation, ColliderBaseComponent collider, out Vector2? point);

        protected bool CollisionOnAxis(Vector2[] box1Points, Vector2[] box2Points, Vector2 axis)
        {
            float box1MinDot = Vector2.Dot(box1Points[0], axis);
            float box1MaxDot = Vector2.Dot(box1Points[0], axis);

            float box2MinDot = Vector2.Dot(box2Points[0], axis);
            float box2MaxDot = Vector2.Dot(box2Points[0], axis);
            for (int i = 1; i < box1Points.Length; i++)
            {
                var box1Proj = Vector2.Dot(box1Points[i], axis);
                if (box1Proj < box1MinDot)
                {
                    box1MinDot = box1Proj;
                }
                else
                if (box1Proj > box1MaxDot)
                {
                    box1MaxDot = box1Proj;
                }
            }
            for (int i = 1; i < box2Points.Length; i++)
            {
                var box2Proj = Vector2.Dot(box2Points[i], axis);
                if (box2Proj < box2MinDot)
                {
                    box2MinDot = box2Proj;
                }
                else
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
