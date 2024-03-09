using System;

namespace ET.Client
{
    public interface IYIUIBackClose
    {
    }

    public interface IYIUIBackCloseSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIPanelInfo addPanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackCloseSystem<T,_> : SystemObject, IYIUIBackCloseSystem where T : Entity, IYIUIBackClose
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackCloseSystem);
        }

        int ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        async ETTask IYIUIBackCloseSystem.Run(Entity o, YIUIPanelInfo addPanelInfo)
        {
            await this.YIUIBackClose((T)o, addPanelInfo);
        }

        protected abstract ETTask YIUIBackClose(T self, YIUIPanelInfo addPanelInfo);
    }
}