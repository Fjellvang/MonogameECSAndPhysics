using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using MyGame.ECS.Components;
using MyGame.ECS.Systems;

namespace MyGame.ECS.Entities
{
    public class Entity : IEntity
    {
        //public Matrix Rotation { get; set; }
        public Transform Transform { get; set; }

        public Entity(IManager manager, Vector3 position, float rotation = 0)
        {
            this.manager = manager;
            var rotationMatrix = Matrix.CreateRotationZ(rotation);

            this.Transform = new Transform() { Rotation = rotationMatrix, Position = position, Angle = rotation, Id = Guid.NewGuid() };
            //this.Position = position;   
            this.manager.AddEntity(this);
            Id = Guid.NewGuid();
        }
        private readonly IManager manager;
        private readonly List<IComponent> components = new List<IComponent>();
        public Guid Id { get; set; }

        public IManager Manager => manager;

        public IList<IComponent> Components => components;


        public T GetComponent<T>() where T : IComponent
        {
            //TODO: Since this implementation only allows for a single component of each type, maybe components SHOULD be stored as hashset for faster lookup ?
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i] is T result)
                {
                    return result;
                }
            }
            return default;
        }

        public void RemoveComponent(IComponent component)
        {
            components.Remove(component);
        }

        public void SetComponent(IComponent component) 
        {
            components.Add(component);
        }
    }
    public class Transform
    {
        public Vector3 Position { get; set; } //TODO: In the future this could be a matrix with pos, scale and rot ?..
        public Matrix Rotation { get;
            set; }
        public Guid Id { get; set; }
        public float Angle { get; set; }
    }

    public struct EntityId
    {
        int id;
        public EntityId(int id)
        {
            this.id = id;
        }
    }
}
