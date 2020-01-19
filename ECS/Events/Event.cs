using MyGame.ECS.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.ECS.Events
{
    public abstract class Event
    {
        public EntityId EntityId { get; }
        public Event(EntityId entityId)
        {
            EntityId = entityId;
        }
    }
}
