using System;

namespace ET
{
    /// <summary>
    /// 每个Config单例的基类
    /// 用于生命周期管理
    /// </summary>
    public interface IConfig
    {
        //Model中的所有配置初始化完毕后的调用
        //Ref的初始化
        void ResolveRef();
    }

    public interface IConfigSystem: ISystemType
    {
        //所有配置初始化完毕后的调用
        //解决需要在Hotfix中初始化其他数据时使用
        void Initialized(IConfig data);
    }

    [EntitySystem]
    public abstract class ConfigSystem<T>: SystemObject, IConfigSystem where T : IConfig
    {
        Type ISystemType.Type()
        {
            return typeof (T);
        }

        Type ISystemType.SystemType()
        {
            return typeof (IConfigSystem);
        }

        int ISystemType.GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.None;
        }

        void IConfigSystem.Initialized(IConfig data)
        {
            this.Initialized((T)data);
        }

        protected abstract void Initialized(T self);
    }
}