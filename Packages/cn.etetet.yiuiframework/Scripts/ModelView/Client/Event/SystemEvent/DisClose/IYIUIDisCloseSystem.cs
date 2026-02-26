using System;

namespace ET.Client
{
    public interface IYIUIDisClose
    {
    }

    public interface IYIUIDisCloseSystem : ISystemType
    {
        ETTask<bool> Run(Entity o);
    }

    [EntitySystem]
    public abstract class YIUIDisCloseSystem<T> : SystemObject, IYIUIDisCloseSystem where T : Entity, IYIUIDisClose
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIDisCloseSystem);
        }

        async ETTask<bool> IYIUIDisCloseSystem.Run(Entity o)
        {
            return await this.YIUIDisClose((T)o);
        }

        protected abstract ETTask<bool> YIUIDisClose(T self);
    }
}
