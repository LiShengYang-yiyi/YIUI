using System;
using UnityEngine;
using System.Collections;
using ET;

namespace UnityEngine.UI
{
    public interface LoopScrollPrefabAsyncSource
    {
        ETTask<GameObject> GetObject(int index);

        void ReturnObject(Transform trans);
    }

    public interface IYIUILoopScrollPrefabAsyncSource
    {
    }

    public interface IYIUILoopScrollPrefabAsyncSourceSystem : ISystemType
    {
        ETTask<GameObject> GetObject(Entity self, int index);

        void ReturnObject(Entity self, Transform trans);
    }

    [EntitySystem]
    public abstract class YIUILoopScrollPrefabAsyncSourceSystem<T> : SystemObject, IYIUILoopScrollPrefabAsyncSourceSystem
            where T : Entity, IYIUILoopScrollPrefabAsyncSource
    {
        Type ISystemType.Type()
        {
            return typeof(T);
        }

        Type ISystemType.SystemType()
        {
            return typeof(IYIUILoopScrollPrefabAsyncSourceSystem);
        }

        async ETTask<GameObject> IYIUILoopScrollPrefabAsyncSourceSystem.GetObject(Entity self, int index)
        {
            return await GetObject((T)self, index);
        }

        void IYIUILoopScrollPrefabAsyncSourceSystem.ReturnObject(Entity self, Transform trans)
        {
            ReturnObject((T)self, trans);
        }

        protected abstract ETTask<GameObject> GetObject(T self, int index);

        protected abstract void ReturnObject(T self, Transform trans);
    }

    public static class LoopScrollPrefabAsyncSourceExtensions
    {
        public static async ETTask<GameObject> GetObject(this IYIUILoopScrollPrefabAsyncSource source, int index)
        {
            var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(source.GetType(), typeof(IYIUILoopScrollPrefabAsyncSourceSystem));
            if (iEventSystems is not { Count: 1 })
            {
                Log.Error($"类:{source.GetType()} 没有具体实现的事件 或有多个实现的事件 [{iEventSystems?.Count}] IYIUILoopScrollPrefabAsyncSourceSystem 请检查");
                return default;
            }

            foreach (IYIUILoopScrollPrefabAsyncSourceSystem eventSystem in iEventSystems)
            {
                try
                {
                    return await eventSystem.GetObject((Entity)source, index);
                }
                catch (Exception e)
                {
                    Log.Error($"类:{source.GetType()} 事件错误: {e}");
                    return default;
                }
            }

            Log.Error($"类:{source.GetType()} 存在多个实现的事件 IYIUILoopScrollPrefabAsyncSourceSystem 请检查");
            return default;
        }

        public static void ReturnObject(this IYIUILoopScrollPrefabAsyncSource source, Transform trans)
        {
            var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(source.GetType(), typeof(IYIUILoopScrollPrefabAsyncSourceSystem));
            if (iEventSystems is not { Count: 1 })
            {
                Log.Error($"类:{source.GetType()} 没有具体实现的事件 或有多个实现的事件 [{iEventSystems?.Count}] IYIUILoopScrollPrefabAsyncSourceSystem 请检查");
                return;
            }

            foreach (IYIUILoopScrollPrefabAsyncSourceSystem eventSystem in iEventSystems)
            {
                try
                {
                    eventSystem.ReturnObject((Entity)source, trans);
                }
                catch (Exception e)
                {
                    Log.Error($"类:{source.GetType()} 事件错误: {e}");
                }
            }
        }
    }
}