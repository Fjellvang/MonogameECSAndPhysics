using System;
using System.Collections.Generic;
using System.Text;
using MyGame.ECS.Entities;

namespace MyGame.ECS.Components
{
    public abstract class BaseComponent<T> : IComponent, IDisposable where T : BaseComponent<T>
    {
        private static readonly List<T> instances = new List<T>();
        private bool isDisposed = false;

        public IEntity Entity { get; } 
        public static IList<T> Instances => BaseComponent<T>.instances;

        public BaseComponent(IEntity entity)
        {
            Entity = entity ?? throw new ArgumentNullException();

            this.Entity.SetComponent((T)this);
            BaseComponent<T>.instances.Add((T)this);

        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    BaseComponent<T>.instances.Remove((T)this);
                }

                this.isDisposed = true;
            }
        }

    }
}
