using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.ECS.Systems
{
    public interface ISystem
    {
        void Update(GameTime gameTime);
        void Draw();
        void Initialize();
    }
}
