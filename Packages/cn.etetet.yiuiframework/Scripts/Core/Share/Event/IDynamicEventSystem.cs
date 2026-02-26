using System;

namespace ET
{
    #if ET10
    public interface IDynamicEvent<A> : IEvent<A> where A : struct
    #else
    public interface IDynamicEvent<A> : IClassEvent<A> where A : struct
    #endif
    {
    }

    public interface IDynamicEventSystem<A> : ISystemType where A : struct
    {
        ETTask Run(Entity o, A message);
    }

    [EntitySystem]
    #if ET10
    public abstract class DynamicEventSystem<T, A> : EventSystem<T, A>, IDynamicEventSystem<A>
    #else
    public abstract class DynamicEventSystem<T, A> : ClassEventSystem<T, A>, IDynamicEventSystem<A>
    #endif
            where T : Entity, IDynamicEvent<A> where A : struct
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IDynamicEventSystem<A>);
        }

        #if ET10
        public new async ETTask Run(Entity o, A message)
        {
            await DynamicEvent((T)o, message);
        }

        protected override void Event(T e, A t)
        {
            throw new NotImplementedException();
        }

        #else
        public new async ETTask Run(Entity o, A message)
        {
            await DynamicEvent((T)o, message);
        }

        protected override void Handle(Entity e, A t)
        {
            throw new NotImplementedException();
        }
        #endif

        protected abstract ETTask DynamicEvent(T self, A message);
    }
}