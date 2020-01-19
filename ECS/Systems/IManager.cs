using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.ECS.Systems
{
    public interface IManager
    {
        IEnumerable<IEntity> Entities { get; }
        IEnumerable<ISystem> Systems { get; }
        void AddEntity(IEntity entity);
        void AddSystem(ISystem system);
        void Initialize();
        void Update(GameTime gameTime);
        void Draw();
        //TODO: Add initialize?

    }
}
