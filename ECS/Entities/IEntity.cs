using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Systems;

namespace MyGame.ECS.Entities
{
    public interface IEntity
    {
        //Vector3 Position { get; set; }
        //Matrix Rotation { get; set; }
        Transform Transform { get; set; }
        IManager Manager { get; }
        IList<IComponent> Components { get; }  //TODO: Could be dict? NameofComponent, should be faster lookup ?
        T GetComponent<T>() where T : IComponent;
        void SetComponent(IComponent component);
        void RemoveComponent(IComponent component);
    }
}
