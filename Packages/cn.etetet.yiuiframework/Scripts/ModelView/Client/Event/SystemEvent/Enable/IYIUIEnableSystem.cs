using System;

namespace ET.Client
{
    public interface IYIUIEnable
    {
    }

    public interface IYIUIEnableSystem : ISystemType
    {
        void Run(Entity o);
    }

    [EntitySystem]
    public abstract class YIUIEnableSystem<T> : SystemObject, IYIUIEnableSystem where T : Entity, IYIUIEnable
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIEnableSystem);
        }

        void IYIUIEnableSystem.Run(Entity o)
        {
            this.YIUIEnable((T)o);
        }

        protected abstract void YIUIEnable(T self);
    }
}
