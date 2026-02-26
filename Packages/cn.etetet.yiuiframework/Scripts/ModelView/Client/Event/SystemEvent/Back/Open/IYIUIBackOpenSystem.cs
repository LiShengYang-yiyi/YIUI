using System;

namespace ET.Client
{
    public interface IYIUIBackOpen
    {
    }

    public interface IYIUIBackOpenSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIEventPanelInfo closePanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackOpenSystem<T, _> : SystemObject, IYIUIBackOpenSystem where T : Entity, IYIUIBackOpen
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackOpenSystem);
        }

        async ETTask IYIUIBackOpenSystem.Run(Entity o, YIUIEventPanelInfo closePanelInfo)
        {
            await this.YIUIBackOpen((T)o, closePanelInfo);
        }

        protected abstract ETTask YIUIBackOpen(T self, YIUIEventPanelInfo closePanelInfo);
    }
}
