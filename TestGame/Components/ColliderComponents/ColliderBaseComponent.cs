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
        RigidBodyComponent attachedRigidbody;
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

        public void AttachRigidBody(RigidBodyComponent rig)
        {
            if (rig.Entity != this.Entity)
            {
                throw new Exception("attaching another entitis rig");
            }
            this.attachedRigidbody = rig;
        }
        
        public bool AttachedRigidBody(out RigidBodyComponent rig) {
            rig = attachedRigidbody;
            return rig != null;
        }

        public abstract bool CollidesWith(Vector2 nextPosition, Matrix nextRotation, ColliderBaseComponent collider, out MTV? point);

        public List<Vector2> CalculateContactManifold(Edge e1, Edge e2, Vector2 axis)
        {
            var flip = false;
            Edge referenceEdge; // most perp to seperationAxis
            Edge incidentEdge;
            if (Math.Abs(e1.Dot(axis)) <= Math.Abs(e2.Dot(axis)))
            {
                referenceEdge = e1;
                incidentEdge = e2;
            }
            else
            {
                referenceEdge = e2;
                incidentEdge = e1;
                flip = true;
            }
            var refv = Vector2.Normalize(referenceEdge.Line);

            var o1 = Vector2.Dot(refv, referenceEdge.V1);
            var clippedPoints = Clip(incidentEdge.V1, incidentEdge.V2, refv, o1);

            if (clippedPoints.Count < 2)
            {
                return clippedPoints; //TODO: Should be null?
            }

            var o2 = Vector2.Dot(refv, referenceEdge.V2);
            clippedPoints = Clip(clippedPoints[0], clippedPoints[1], -refv, -o2);
            if (clippedPoints.Count < 2)
            {
                return clippedPoints; //TODO: Should be null?
            }

            var referenceNormal = refv.ToLeftTurnedNormal();
            if (flip)
            {
                referenceNormal = refv.ToRightTurnedNormal();
                referenceNormal *= -1;
            }

            var max = Vector2.Dot(referenceNormal, referenceEdge.Max);
            var val0 = clippedPoints[0];
            var val1 = clippedPoints[1];

            if (referenceNormal.Dot(val0) - max < 0)
            {
                clippedPoints.Remove(val0);
            }
            if (referenceNormal.Dot(val1) - max < 0)
            {
                clippedPoints.Remove(val1);
            }

            return clippedPoints;
        }

        private List<Vector2> Clip(Vector2 v1, Vector2 v2, Vector2 axis, float o)
        {
            var clippedPoints = new List<Vector2>();
            var d1 = Vector2.Dot(axis, v1) - o; // subtract o to do the clipping
            var d2 = Vector2.Dot(axis, v2) - o;

            // if either point is past o, along n, then we keep em
            if (d1 >= 0)
            {
                clippedPoints.Add(v1);
            }
            if (d2 >= 0)
            {
                clippedPoints.Add(v2);
            }

            if (d1*d2 < 0)
            {
                // they are on different sides. one is pos one is neg.
                var e = v2 - v1;
                //compute location along e.
                var u = d1 / (d1 - d2);
                e *= u;
                e += v1;
                clippedPoints.Add(e);
            }
            return clippedPoints;
        }
        public Edge FindBestCollisionEdge(Vector2 axis, Vector2 nextPostion)
        {
            int length = vertices.Length;//TODO: do we need vertices in worldspacce??? Probably not.
            var max = float.MinValue;
            var index = -1;
            var vert = this.Vertices(nextPostion, this.Entity.Rotation);

            // get the farthest point
            for (int i = 0; i < length; i++)
            {
                var proj = Vector2.Dot(axis, vert[i]);
                if (proj > max)
                {
                    max = proj;
                    index = i;
                }
            }

            //use edge which is most perpendicular.
            var v  = vert[index];
            var v1 = vert[(index + 1) % length];
            var v2 = vert[index == 0 ? length-1 : index - 1];

            // V1 to V;
            var l = v - v1;
            // V2 to V;
            var r = v - v2;
            l.Normalize();
            r.Normalize();

            //Find edge most perpendicular to seperation axis.
            if (Vector2.Dot(r, axis) <= Vector2.Dot(l, axis))
            {
                return new Edge(v, v2, v);
            } else
            {
                //select left
                return new Edge(v, v, v1);
            }

        }

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
    public struct Edge
    {
        public Edge(Vector2 max, Vector2 v1, Vector2 v2)
        {
            Max = max;
            V1 = v1;
            V2 = v2;
            Line = V2 - V1;
        }

        public Vector2 Max { get; }
        public Vector2 V1 { get; }
        public Vector2 V2 { get; }

        public Vector2 Line { get; }

        public float Dot(Vector2 other)
        {
            return Vector2.Dot(Line, other);
        }
    }
    /// <summary>
    /// Minimum translation vector...
    /// </summary>
    public struct MTV
    {
        public MTV(Vector2 axis, float magnitude)
        {
            axis.Normalize();
            Axis = axis;
            Magnitude = magnitude;
        }

        public Vector2 Axis { get; set; }
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
            return Math.Min(other.max - min, max - other.min);
        }
    }
}
