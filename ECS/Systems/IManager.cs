using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.ECS.Systems
{
    public interface IManager
    {
        Random GetRandom { get; }// TODO: Consider if its smart here???
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
