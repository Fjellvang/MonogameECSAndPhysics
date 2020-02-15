using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
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
        public Vector3 CenterOfMass { get; set; }
        public SimulationObjectType ObjectType { get; set; }
        public Vector3 PreviousPosition { get; set; }
        public Vector3 CurrentPosition { get; set; }
        public Vector3 CurrentVelocity { get; set; }
        /// <summary>
        /// All forces acting on the object summed up.
        /// </summary>
        public Vector3 ResultantForce { get; set; }

        public float PreviousAngle { get; set; }
        public float CurrentAngle { get; set; }
        public float CurrentAngularVelocity { get; set; }
        public float ResultantAngularForce { get; set; }
        public Matrix NextRotation { get; set; } //TODO: finde something better????

        public RigidBodyComponent(IEntity entity, float mass, Vector3 center, SimulationObjectType objectType) : base(entity)
        {
            this.Mass = mass;
            this.ObjectType = objectType;
            CurrentPosition = entity.Position;
            PreviousPosition = entity.Position;
            CurrentVelocity = Vector3.Zero;
            this.CenterOfMass = center;
        }

        public void ResetForces()
        {
            this.ResultantForce = Vector3.Zero;
            this.ResultantAngularForce = 0f;
        }
        public void UpdateEntityPosition()
        {
            this.Entity.Position = CurrentPosition;
            this.Entity.Rotation = Matrix.CreateRotationZ(CurrentAngle);
        }
        public void AddRelativeForce(Vector3 force)
        {
            this.ResultantForce += Vector3.Transform(force, this.Entity.Rotation);
        }
        public void AddForce(Vector3 force)
        {
            this.ResultantForce += force;
        }
        public void AddForceAtPoint(Vector2 force, Vector2 point)
        {
            var torqu = (point - this.CenterOfMass.ToVector2()).Cross(force);
            this.ResultantForce += new Vector3(force, 0);
            //Toque cannot be angular force?!
            this.ResultantAngularForce += torqu;

        }

        public void AddAngularForce(float force)
        {
            this.ResultantAngularForce += force;
        }
    }
}
