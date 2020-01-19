using System;
using System.Collections.Generic;
using System.Text;
using MyGame.ECS.Components;
using MyGame.ECS.Systems;

namespace MyGame.ECS.Entities
{
    public interface IEntity
    {
        IManager Manager { get; }
        IList<IComponent> Components { get; }  //TODO: Could be dict? NameofComponent, should be faster lookup ?
        T GetComponent<T>() where T : IComponent;
        void SetComponent(IComponent component);
        void RemoveComponent(IComponent component);
    }
}
