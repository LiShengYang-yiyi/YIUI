using UnityEngine;
using System.Collections;
using System;
using ET;

namespace UnityEngine.UI
{
    public interface LoopScrollDataSource
    {
        void ProvideData(Transform transform, int idx);
    }

    public interface IYIUILoopScrollDataSource
    {
    }

    public interface IYIUILoopScrollDataSourceSystem : ISystemType
    {
        void ProvideData(Entity self, Transform transform, int idx);
    }

    [EntitySystem]
    public abstract class YIUILoopScrollDataSourceSystem<T> : SystemObject, IYIUILoopScrollDataSourceSystem
            where T : Entity, IYIUILoopScrollDataSource
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUILoopScrollDataSourceSystem);
        }

        void IYIUILoopScrollDataSourceSystem.ProvideData(Entity self, Transform transform, int idx)
        {
            ProvideData((T)self, transform, idx);
        }

        protected abstract void ProvideData(T self, Transform transform, int idx);
    }

    public static class YIUILoopScrollDataSourceExtensions
    {
        public static void ProvideData(this IYIUILoopScrollDataSource source, Transform transform, int idx)
        {
            var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(source.GetType(), typeof(IYIUILoopScrollDataSourceSystem));
            if (iEventSystems is not { Count: > 0 })
            {
                Log.Error($"类:{source.GetType()} 没有具体实现的事件 IYIUILoopScrollPrefabAsyncSourceSystem 请检查");
                return;
            }

            foreach (IYIUILoopScrollDataSourceSystem eventSystem in iEventSystems)
            {
                try
                {
                    eventSystem.ProvideData((Entity)source, transform, idx);
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"类:{source.GetType()} 事件错误: {e.Message}");
                    return;
                }
            }
        }
    }
}