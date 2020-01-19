using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public class PlayerInputComponent : BaseComponent<PlayerInputComponent>
    {
        public PlayerInputComponent(IEntity entity) : base(entity)
        {
        }
    }
}
