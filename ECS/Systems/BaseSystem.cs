using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyGame.ECS.Systems
{
    public class BaseSystem : ISystem
    {
        public IManager Manager { get; }
        public BaseSystem(IManager manager)
        {
            Manager = manager ?? throw new ArgumentNullException();
        }
        public virtual void Draw()
        {
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}
