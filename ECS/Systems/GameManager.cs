using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Entities;

namespace MyGame.ECS.Systems
{
    public class GameManager : IManager
    {
        //TODO: Why linked?
        //private readonly LinkedList<IEntity> entities = new LinkedList<IEntity>();
        //private readonly LinkedList<ISystem> systems = new LinkedList<ISystem>();
        private readonly List<IEntity> entities = new List<IEntity>();
        private readonly List<ISystem> systems = new List<ISystem>();

        public IEnumerable<IEntity> Entities => entities;

        public IEnumerable<ISystem> Systems => systems;

        public void AddEntity(IEntity entity)
        {
            entities.Add(entity);
        }

        public void AddSystem(ISystem system)
        {
            systems.Add(system);
        }

        public void Draw()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                systems[i].Draw();
            }
        }

        public void Initialize()
        {
            for (int i = 0; i < systems.Count; i++)
            {
                systems[i].Initialize();
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < systems.Count; i++)
            {
                systems[i].Update(gameTime);
            }
        }
    }
}
