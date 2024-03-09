using System;

namespace ET.Client
{
    public interface IYIUIBackOpen
    {
    }

    public interface IYIUIBackOpenSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIPanelInfo closePanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackOpenSystem<T,_> : SystemObject, IYIUIBackOpenSystem where T : Entity, IYIUIBackOpen
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackOpenSystem);
        }

        int ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        async ETTask IYIUIBackOpenSystem.Run(Entity o, YIUIPanelInfo closePanelInfo)
        {
            await this.YIUIBackOpen((T)o, closePanelInfo);
        }

        protected abstract ETTask YIUIBackOpen(T self, YIUIPanelInfo closePanelInfo);
    }
}