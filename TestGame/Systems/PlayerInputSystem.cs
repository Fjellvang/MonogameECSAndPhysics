using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Systems
{
    public class PlayerInputSystem : BaseSystem
    {
        float speed = 1f;
        public PlayerInputSystem(IManager manager) : base(manager)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0;i< PlayerInputComponent.Instances.Count; i++)
            {
                var comp = PlayerInputComponent.Instances[i];
                var transform = comp.Entity.GetComponent<SimpleRigidbodyComponent>();
                var state = Keyboard.GetState();

                if (state.IsKeyDown(Keys.A))
                {
                    transform.Acceleration = Vector2.UnitX * (speed);
                }
                if (state.IsKeyDown(Keys.D))
                {
                    transform.Acceleration = -Vector2.UnitX * (speed);
                }

            }
        }
    }
}
