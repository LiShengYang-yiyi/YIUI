using System;

namespace ET.Client
{
    public interface IYIUIBackClose
    {
    }

    public interface IYIUIBackCloseSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIEventPanelInfo addPanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackCloseSystem<T, _> : SystemObject, IYIUIBackCloseSystem where T : Entity, IYIUIBackClose
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackCloseSystem);
        }

        async ETTask IYIUIBackCloseSystem.Run(Entity o, YIUIEventPanelInfo addPanelInfo)
        {
            await this.YIUIBackClose((T)o, addPanelInfo);
        }

        protected abstract ETTask YIUIBackClose(T self, YIUIEventPanelInfo addPanelInfo);
    }
}
