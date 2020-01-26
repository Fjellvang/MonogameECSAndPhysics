using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Physics.Constraints
{
    /// <summary>
    /// interface to implements constraints on rigidbodies...
    /// </summary>
    public interface IConstraint
    {
        void SatisfyConstraint();
    }
}
