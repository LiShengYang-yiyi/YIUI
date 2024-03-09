using System;

namespace ET.Client
{
    public interface IYIUIBackHomeClose
    {
    }

    public interface IYIUIBackHomeCloseSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIPanelInfo HomeClosePanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackHomeCloseSystem<T,_> : SystemObject, IYIUIBackHomeCloseSystem where T : Entity, IYIUIBackHomeClose
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackHomeCloseSystem);
        }

        int ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        async ETTask IYIUIBackHomeCloseSystem.Run(Entity o, YIUIPanelInfo HomeClosePanelInfo)
        {
            await this.YIUIBackHomeClose((T)o, HomeClosePanelInfo);
        }

        protected abstract ETTask YIUIBackHomeClose(T self, YIUIPanelInfo HomeClosePanelInfo);
    }
}