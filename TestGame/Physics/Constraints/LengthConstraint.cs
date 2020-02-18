using Microsoft.Xna.Framework;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.Constraints
{
    /// <summary>
    /// a constraint describing a desired length between two rigidbodies
    /// </summary>
    public class LengthConstraint : IConstraint
    {
        public float Length { get; set; }
        public RigidBodyComponent ObjA { get; set; }
        public RigidBodyComponent ObjB { get; set; }
        public LengthConstraint(RigidBodyComponent objA, RigidBodyComponent objB, float length)
        {
            this.ObjA = objA;
            this.ObjB = objB;
            this.Length = length;
                
        }
        public void SatisfyConstraint()
        {
            var direction = ObjB.Entity.Transform.Position - ObjA.Entity.Transform.Position;
            var currentLength = direction.Length();

            if (direction != Vector3.Zero)
            {
                direction.Normalize();
                //half of the desired lenght
                var moveVector = .5f * (currentLength - Length) * direction;
                ObjA.Entity.Transform.Position += moveVector;
                ObjB.Entity.Transform.Position -= moveVector;
            }
        }
    }
}
