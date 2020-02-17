using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using MyGame.TestGame.Components.ColliderComponents;
using MyGame.TestGame.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public enum SimulationObjectType { Passive, Active}
    public class RigidBodyComponent : BaseComponent<RigidBodyComponent>
    {
        //public Vector2 Velocity { get; set; }
        //public Vector2 Acceleration { get; set; }
        //public SimulationObject SimulationObject { get; set; }
        public float Mass { get; set; }
        public float InvMass { get; set; }
        public float Inertia { get; set; }
        public float InvInertia { get; set; }
        public Vector2 CenterOfMass { get; set; }
        public SimulationObjectType ObjectType { get; set; }
        public ColliderBaseComponent Collider { get; }
        public Vector2 PreviousPosition { get; set; }
        public Vector2 CurrentPosition { get; set; }
        public Vector2 CurrentVelocity { get; set; }
        /// <summary>
        /// All forces acting on the object summed up.
        /// </summary>
        public Vector2 ResultantForce { get; set; }

        public float PreviousAngle { get; set; }
        public float CurrentAngle { get; set; }
        public float CurrentAngularVelocity { get; set; }
        public float ResultantAngularForce { get; set; }
        public Matrix NextRotation { get; set; } //TODO: finde something better????

        public RigidBodyComponent(IEntity entity, float mass, float inertia, Vector2 center, SimulationObjectType objectType, ColliderBaseComponent collider) : base(entity)
        {
            this.Mass = mass;
            this.InvMass = 1 / mass;
            this.Inertia = inertia;
            this.InvInertia = 1 / inertia;
            this.ObjectType = objectType;
            Collider = collider;
            CurrentPosition = entity.Position.ToVector2();
            PreviousPosition = entity.Position.ToVector2();
            CurrentVelocity = Vector2.Zero;
            this.CenterOfMass = center;
            collider.AttachRigidBody(this);
        }

        public void ResetForces()
        {
            this.ResultantForce = Vector2.Zero;
            this.ResultantAngularForce = 0f;
        }
        public void UpdateEntityPosition()
        {
            this.Entity.Position = new Vector3(CurrentPosition, 0);
            this.Entity.Rotation = Matrix.CreateRotationZ(CurrentAngle);
        }
        public void AddRelativeForce(Vector2 force)
        {
            this.ResultantForce += Vector2.Transform(force, this.Entity.Rotation);
        }
        public void AddForce(Vector2 force)
        {
            this.ResultantForce += force;
        }
        public void AddForceAtPoint(Vector2 force, Vector2 point)
        {
            var torqu = (point - this.CenterOfMass).Cross(force);
            this.ResultantForce += force;
            //Toque cannot be angular force?!
            this.ResultantAngularForce += torqu;

        }

        public void AddAngularForce(float force)
        {
            this.ResultantAngularForce += force;
        }
    }
}
