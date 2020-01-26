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
        float speed = 4f;
        float origspeed;
        KeyboardState oldState;
        public PlayerInputSystem(IManager manager) : base(manager)
        {
            origspeed = speed;
        }

        public override void Initialize()
        {
            oldState = Keyboard.GetState();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0;i< PlayerInputComponent.Instances.Count; i++)
            {
                var comp = PlayerInputComponent.Instances[i];
                //var transform = comp.Entity.GetComponent<TransformComponent>();
                var state = Keyboard.GetState();

                if (state.IsKeyDown(Keys.LeftShift))
                {
                    speed = 2 * origspeed;
                }else
                {
                    speed = origspeed;
                }

                if (state.IsKeyDown(Keys.A))
                {
                    comp.Entity.Position -= new Vector3(0.1f, 0, 0) * speed;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    comp.Entity.Position += new Vector3(0.1f, 0, 0) * speed;
                }
                if (state.IsKeyDown(Keys.S))
                {
                    comp.Entity.Position += new Vector3(0, 0.1f, 0) * speed;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    comp.Entity.Position -= new Vector3(0, 0.1f, 0) * speed;
                }


                oldState = state;
            }
        }
    }
}
