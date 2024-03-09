using System;

namespace ET.Client
{
    public interface IYIUIBackAdd
    {
    }

    public interface IYIUIBackAddSystem : ISystemType
    {
        ETTask Run(Entity o, YIUIPanelInfo closePanelInfo);
    }

    [EntitySystem]
    public abstract class YIUIBackAddSystem<T,_> : SystemObject, IYIUIBackAddSystem where T : Entity, IYIUIBackAdd
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIBackAddSystem);
        }

        int ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        async ETTask IYIUIBackAddSystem.Run(Entity o, YIUIPanelInfo closePanelInfo)
        {
            await this.YIUIBackAdd((T)o, closePanelInfo);
        }

        protected abstract ETTask YIUIBackAdd(T self, YIUIPanelInfo closePanelInfo);
    }
}