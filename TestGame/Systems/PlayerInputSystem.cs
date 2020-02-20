using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyGame.ECS.Systems;
using MyGame.TestGame.Components;
using MyGame.TestGame.Components.ColliderComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Systems
{
    public class PlayerInputSystem : BaseSystem
    {
        float angleRotationalSpeed = 5000;
        float angle = 0;
        float speed = 1000;
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
                var rig = comp.Entity.GetComponent<RigidBodyComponent>();
                var state = Keyboard.GetState();
                float newAngle = angle;

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
                    rig.AddForce(-Vector2.UnitX * speed);
                }
                if (state.IsKeyDown(Keys.D))
                {
                    translationVector += new Vector3(0.1f, 0, 0) * speed;
                    rig.AddForce(Vector2.UnitX * speed);
                }
                if (state.IsKeyDown(Keys.W))
                {
                    translationVector += new Vector3(0, 0.1f, 0) * speed;
                    rig.AddForce(-Vector2.UnitY * speed);
                }
                if (state.IsKeyDown(Keys.S))
                {
                    translationVector -= new Vector3(0, 0.1f, 0) * speed;
                    rig.AddForce(Vector2.UnitY * speed);
                }
                if (state.IsKeyDown(Keys.Q))
                {
                    var angle = angleRotationalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    rig.AddAngularForce(angle);
                    //rig.AddForceAtPoint(Vector2.UnitY * speed, (rig.CenterOfMass + Vector2.UnitX * 1f));
                }
                else
                if (state.IsKeyDown(Keys.E))
                {
                    var angle = -angleRotationalSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    rig.AddAngularForce(angle);
                    //rig.AddForceAtPoint(Vector2.UnitY * speed, (rig.CenterOfMass + Vector2.UnitX * -1f));
                }
                //var nextPos = comp.Entity.Position + Vector3.Transform(translationVector, comp.Entity.Rotation);
                //var nextRotation = Matrix.CreateRotationZ(newAngle);

                //var collider = comp.Entity.GetComponent<BoxCollider>();
                //if (collider != null)
                //{
                //    for (int j = 0; j < ColliderBaseComponent.Instances.Count; j++)
                //    {
                //        var other = ColliderBaseComponent.Instances[j];
                //        if (collider.Entity == other.Entity)
                //        {
                //            continue;
                //        }
                //        if (collider.CollidesWith(nextPos.ToVector2(), nextRotation, other, out _))
                //        {
                //            nextPos = comp.Entity.Position;
                //            nextRotation = comp.Entity.Rotation;
                //            newAngle = angle;
                //        }
                //    }
                //}

                //angle = newAngle;

                //comp.Entity.Rotation = nextRotation;
                //comp.Entity.Position = nextPos;




                oldState = state;
            }
        }
    }
}
