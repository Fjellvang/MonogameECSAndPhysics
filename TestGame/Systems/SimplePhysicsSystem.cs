using Microsoft.Xna.Framework;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Systems
{
    public class SimplePhysicsSystem : BaseSystem
    {
        Rectangle bounds; //TODO: find something better
        public SimplePhysicsSystem(IManager manager, Rectangle? bounds) : base(manager)
        {
            this.bounds = bounds ?? new Rectangle(0, 0, 1280, 1280);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < SimpleRigidbodyComponent.Instances.Count; i++)
            {
                var rig = SimpleRigidbodyComponent.Instances[i];
                var transform = rig.Entity.GetComponent<TransformComponent>();
                rig.Velocity += rig.Acceleration;
                if (transform is null) return;
                var newPos = transform.Position + rig.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                var collider = rig.Entity.GetComponent<BoxColliderComponent>();
                if(collider is null)
                {
                    transform.Position = newPos;
                    return;
                }

                var newRect = new Rectangle((int)newPos.X, (int)newPos.Y,(int)collider.Bounds.Width,(int)collider.Bounds.Height);

                //bounds check
                //if (newRect.Left < 0 || newRect.Right > bounds.Width || newRect.Top < 0 || newRect.Bottom > bounds.Height)
                //{
                //    return;
                //}
                transform.Position = newPos;
            }
        }
    }
}
