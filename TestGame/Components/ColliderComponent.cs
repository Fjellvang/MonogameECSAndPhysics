﻿using MyGame.ECS.Components;
using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.TestGame.Components
{
    public abstract class ColliderComponent : BaseComponent<ColliderComponent>
    {
        public ColliderComponent(IEntity entity) : base(entity)
        {
        }
    }
}