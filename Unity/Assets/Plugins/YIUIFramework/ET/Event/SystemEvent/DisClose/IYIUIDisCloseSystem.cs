using System;

namespace ET.Client
{
    public interface IYIUIDisClose
    {
    }

    public interface IYIUIDisCloseSystem: ISystemType
    {
        ETTask<bool> Run(Entity o);
    }

    [ObjectSystem]
    public abstract class YIUIDisCloseSystem<T>: IYIUIDisCloseSystem where T : Entity, IYIUIDisClose
    {
        Type ISystemType.Type()
        {
            return typeof (T);
        }

        Type ISystemType.SystemType()
        {
            return typeof (IYIUIDisCloseSystem);
        }

        InstanceQueueIndex ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        async ETTask<bool> IYIUIDisCloseSystem.Run(Entity o)
        {
            return await this.YIUIDisClose((T)o);
        }

        protected abstract ETTask<bool> YIUIDisClose(T self);
    }
}