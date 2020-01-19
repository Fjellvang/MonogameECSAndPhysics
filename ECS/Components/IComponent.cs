using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.ECS.Components
{
    public interface IComponent : IDisposable
    {
        IEntity Entity {get;} 
    }
}
