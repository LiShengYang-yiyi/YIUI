using System;

namespace ET
{
    public interface IEntityDrawer
    {
        void Drawer(Entity entity);
    }

    public abstract class EntityDrawerSystem<T> : IEntityDrawer where T : Entity
    {
        void IEntityDrawer.Drawer(Entity entity)
        {
            Drawer((T)entity);
        }

        protected abstract void Drawer(T entity);
    }

    public class EntityDrawer
    {
        public Type          EntityType;
        public bool          SkipTypeDrawer;
        public int           Order;
        public IEntityDrawer Drawer;
    }
}