using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.ECS.Events
{
    public class EventManager
    {
        public void SendEvent<T>() where T : Event
        {
            throw new NotImplementedException();
        }
    }
}
