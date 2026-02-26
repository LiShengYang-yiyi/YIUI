using System;

namespace ET.Client
{
    public interface IYIUIWindowClose
    {
    }

    public interface IYIUIWindowCloseSystem : ISystemType
    {
        ETTask Run(Entity self, bool viewCloseResult);
    }

    [EntitySystem]
    public abstract class YIUIWindowCloseSystem<T, _> : SystemObject, IYIUIWindowCloseSystem
            where T : Entity, IYIUIWindowClose
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUIWindowCloseSystem);
        }

        async ETTask IYIUIWindowCloseSystem.Run(Entity self, bool viewCloseResult)
        {
            await this.YIUIWindowClose((T)self, viewCloseResult);
        }

        protected abstract ETTask YIUIWindowClose(T self, bool viewCloseResult);
    }
}