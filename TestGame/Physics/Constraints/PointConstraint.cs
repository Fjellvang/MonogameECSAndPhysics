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
        public Vector3 Point { get; set; }
        public RigidBodyComponent Rig { get; set; }
        public PointConstraint(Vector3 point, RigidBodyComponent rig)
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
