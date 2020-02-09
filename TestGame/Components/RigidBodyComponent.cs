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
        public SimulationObjectType ObjectType { get; set; }
        public Vector3 PreviousPosition { get; set; }
        public Vector3 CurrentPosition { get; set; }
        public Vector3 CurrentVelocity { get; set; }
        /// <summary>
        /// All forces acting on the object summed up.
        /// </summary>
        public Vector3 ResultantForce { get; set; }
        public RigidBodyComponent(IEntity entity, float mass, SimulationObjectType objectType) : base(entity)
        {
            this.Mass = mass;
            this.ObjectType = objectType;
            CurrentPosition = entity.Position;
            PreviousPosition = entity.Position;
            CurrentVelocity = Vector3.Zero;
        }

        public void ResetForces()
        {
            this.ResultantForce = Vector3.Zero;
        }
        public void UpdateEntityPosition()
        {
            this.Entity.Position = CurrentPosition;
        }
        public void AddForce(Vector3 force)
        {
            this.ResultantForce = force;
        }
    }
}
