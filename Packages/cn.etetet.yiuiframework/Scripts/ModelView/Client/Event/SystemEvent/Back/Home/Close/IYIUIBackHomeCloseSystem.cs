using System;

namespace ET.Client
{
    public interface IYIUIBackHomeClose
    {
    }

    public interface IYIUIBackHomeCloseSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIEventPanelInfo HomeClosePanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackHomeCloseSystem<T, _> : SystemObject, IYIUIBackHomeCloseSystem where T : Entity, IYIUIBackHomeClose
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackHomeCloseSystem);
        }

        async ETTask IYIUIBackHomeCloseSystem.Run(Entity o, YIUIEventPanelInfo HomeClosePanelInfo)
        {
            await this.YIUIBackHomeClose((T)o, HomeClosePanelInfo);
        }

        protected abstract ETTask YIUIBackHomeClose(T self, YIUIEventPanelInfo HomeClosePanelInfo);
    }
}
