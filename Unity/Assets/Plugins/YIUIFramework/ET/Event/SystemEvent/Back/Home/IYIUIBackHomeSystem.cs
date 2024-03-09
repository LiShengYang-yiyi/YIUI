using System;

namespace ET.Client
{
    public interface IYIUIBackHome
    {
    }

    public interface IYIUIBackHomeSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIPanelInfo homePanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackHomeSystem<T,_> : SystemObject, IYIUIBackHomeSystem where T : Entity, IYIUIBackHome
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackHomeSystem);
        }

        int ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        async ETTask IYIUIBackHomeSystem.Run(Entity o, YIUIPanelInfo homePanelInfo)
        {
            await this.YIUIBackHome((T)o, homePanelInfo);
        }

        protected abstract ETTask YIUIBackHome(T self, YIUIPanelInfo homePanelInfo);
    }
}