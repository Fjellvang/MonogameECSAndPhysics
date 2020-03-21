using Microsoft.Xna.Framework;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components.ColliderComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Systems
{
    public class CollisionSystem : BaseSystem
    {
        public CollisionSystem(IManager manager) : base(manager)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < ColliderBaseComponent.Instances.Count; i++)
            {
                var collider = ColliderBaseComponent.Instances[i];
                for (int j = 0; j < ColliderBaseComponent.Instances.Count; j++)
                {
                    var other = ColliderBaseComponent.Instances[j];
                    if (collider.Entity == other.Entity)
                    {
                        continue;
                    }
                    collider.CollidesWith(collider.Entity.Transform.Position, collider.Entity.Transform.Rotation, other, out _);
                }
            }
            base.Update(gameTime);
        }
    }
}
