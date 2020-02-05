using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using MyGame.TestGame.Components.SpriteComponents;
using MyGame.TestGame.Physics.ForceGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class SpringComponent : BaseComponent<SpringComponent>
    {
        public Spring spring { get; set; }
        public SpringComponent(float stiffness, float damping, IEntity entity, IEntity other, float? restLength = null, bool addLine = true) : base(entity)
        {
            var rigA = entity.GetComponent<RigidBodyComponent>();
            var rigB = other.GetComponent<RigidBodyComponent>();
            if (rigA is null || rigB is null)
            {
                throw new ArgumentNullException("a springs entities needs a rigidbody!");
            }
            if (restLength is null)
            {
                spring = new Spring(stiffness, damping, rigA, rigB);
            }
            else
            {
                spring = new Spring(stiffness, damping, rigA, rigB, restLength.Value);
            }
            if (addLine)
            {
                new LineEntityToEntityComponent(entity, other, Color.Red);
            }
        }
    }
}
