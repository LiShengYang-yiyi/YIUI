using System;

namespace ET.Client
{
    public interface IYIUIBackHomeOpen
    {
    }

    public interface IYIUIBackHomeOpenSystem : ISystemType
    {
        ETTask Run(Entity o);
    }

    [EntitySystem]
    public abstract class YIUIBackHomeOpenSystem<T> : SystemObject, IYIUIBackHomeOpenSystem where T : Entity, IYIUIBackHomeOpen
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackHomeOpenSystem);
        }

        async ETTask IYIUIBackHomeOpenSystem.Run(Entity o)
        {
            await this.YIUIBackHomeOpen((T)o);
        }

        protected abstract ETTask YIUIBackHomeOpen(T self);
    }
}
