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
        float angleRotationalSpeed = 2f;
        float angle = 0;
        float speed = 50f;
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

                Vector3 translationVector = Vector3.Zero;
                if (state.IsKeyDown(Keys.A))
                {
                    translationVector -= new Vector3(0.1f, 0, 0) * speed;
                }
                if (state.IsKeyDown(Keys.D))
                {
                    translationVector += new Vector3(0.1f, 0, 0) * speed;
                }
                if (state.IsKeyDown(Keys.W))
                {
                    translationVector += new Vector3(0, 0.1f, 0) * speed;
                }
                if (state.IsKeyDown(Keys.S))
                {
                    translationVector -= new Vector3(0, 0.1f, 0) * speed;
                }
                if (state.IsKeyDown(Keys.Q))
                {
                    angle += angleRotationalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    comp.Entity.Rotation = Matrix.CreateRotationZ(angle);
                }else
                if (state.IsKeyDown(Keys.E))
                {
                    angle -= angleRotationalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    comp.Entity.Rotation = Matrix.CreateRotationZ(angle);
                }
                comp.Entity.Position += Vector3.Transform(translationVector, comp.Entity.Rotation);
                



                oldState = state;
            }
        }
    }
}
