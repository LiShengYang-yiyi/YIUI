using System;

namespace ET.Client
{
    public interface IYIUIPreLoad
    {
    }

    public interface IYIUIPreLoadSystem : ISystemType
    {
        ETTask Run(Entity o);
    }

    [EntitySystem]
    public abstract class YIUIPreLoadSystem<T> : SystemObject, IYIUIPreLoadSystem where T : Entity, IYIUIPreLoad
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIPreLoadSystem);
        }

        async ETTask IYIUIPreLoadSystem.Run(Entity o)
        {
            await this.YIUIPreLoad((T)o);
        }

        protected abstract ETTask YIUIPreLoad(T self);
    }
}