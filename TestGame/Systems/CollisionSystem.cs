using Microsoft.Xna.Framework;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
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
            for (int i = 0; i < ColliderComponent.Instances.Count; i++)
            {
                var collider = ColliderComponent.Instances[i];
                for (int j = 0; j < ColliderComponent.Instances.Count; j++)
                {
                    var other = ColliderComponent.Instances[j];
                    if (collider.Entity == other.Entity)
                    {
                        continue;
                    }
                    collider.CollidesWith(other, out _);
                }
            }
            base.Update(gameTime);
        }
    }
}
