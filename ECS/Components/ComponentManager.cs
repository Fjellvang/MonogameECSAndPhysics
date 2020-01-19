using MyGame.ECS.Entities;
using System;

namespace MyGame.ECS.Components
{
    public class ComponentManager
    {
        public void CreateComponent<T>(EntityId id, Type component) where T : IComponent
        {
            //TODO
            throw new NotImplementedException();
        }

        public T GetComponent<T>(EntityId id) where T : IComponent
        {
            throw new NotImplementedException();
        }
        public void RemoveComponent<T>(EntityId id) where T : IComponent
        {
            throw new NotImplementedException();
        }


    }
}
