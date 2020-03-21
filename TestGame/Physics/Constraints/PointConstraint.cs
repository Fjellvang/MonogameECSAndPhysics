using Microsoft.Xna.Framework;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.Constraints
{
    /// <summary>
    /// A Constraint Describing where a given object should be
    /// </summary>
    public class PointConstraint : IConstraint
    {
        public Vector2 Point { get; set; }
        public RigidBodyComponent Rig { get; set; }
        public PointConstraint(Vector2 point, RigidBodyComponent rig)
        {
            this.Rig = rig;
            this.Point = point;
        }
        public void SatisfyConstraint()
        {
            Rig.Entity.Transform.Position = Point;
        }
    }
}
