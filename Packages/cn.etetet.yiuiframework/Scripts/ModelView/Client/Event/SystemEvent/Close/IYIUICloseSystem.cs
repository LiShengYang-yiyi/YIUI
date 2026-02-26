using System;

namespace ET.Client
{
    public interface IYIUIClose
    {
    }

    public interface IYIUICloseSystem : ISystemType
    {
        ETTask<bool> Run(Entity o);
    }

    [EntitySystem]
    public abstract class YIUICloseSystem<T> : SystemObject, IYIUICloseSystem where T : Entity, IYIUIClose
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUICloseSystem);
        }

        async ETTask<bool> IYIUICloseSystem.Run(Entity o)
        {
            return await this.YIUIClose((T)o);
        }

        protected abstract ETTask<bool> YIUIClose(T self);
    }
}
