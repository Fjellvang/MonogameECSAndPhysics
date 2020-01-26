using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using MyGame.TestGame.Physics.ForceGenerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class SpringComponent : BaseComponent<SpringComponent>
    {
        public Spring spring { get; set; }
        public SpringComponent(float stiffness, float damping, IEntity entity, IEntity other) : base(entity)
        {
            var rigA = entity.GetComponent<SimpleRigidbodyComponent>();
            var rigB = other.GetComponent<SimpleRigidbodyComponent>();
            if (rigA is null || rigB is null)
            {
                throw new ArgumentNullException("a springs entities needs a rigidbody!");
            }
            spring = new Spring(stiffness, damping, rigA.SimulationObject, rigB.SimulationObject);
        }
    }
}
