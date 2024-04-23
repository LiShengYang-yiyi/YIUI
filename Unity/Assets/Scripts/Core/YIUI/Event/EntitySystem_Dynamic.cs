using System;
using System.Collections.Generic;

namespace ET
{
    //动态事件系统
    //只能发往目标纤程中
    //同一个纤程中也会有不同的scene
    //所以还可区分场景
    public partial class EntitySystem
    {
        public Queue<EntityRef<Entity>>[] Queues => queues;

        //向任意场景发送动态事件
        public async ETTask DynamicEvent<P1>(P1 message) where P1 : struct
        {
            await DynamicEvent(SceneType.All, message);
        }

        //向与传入的实体相同的场景发送动态事件
        public async ETTask DynamicEvent<P1>(Entity entity, P1 message) where P1 : struct
        {
            await DynamicEvent(entity.IScene.SceneType, message);
        }

        //向目标场景发送动态事件
        public async ETTask DynamicEvent<P1>(Scene scene, P1 message) where P1 : struct
        {
            await DynamicEvent(scene.SceneType, message);
        }

        //向指定场景发送动态事件
        public async ETTask DynamicEvent<P1>(SceneType sceneType, P1 message) where P1 : struct
        {
            var queue = queues[InstanceQueueIndex.Dynamic];
            int count = queue.Count;
            if (count <= 0) return;

            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();
            while (count-- > 0)
            {
                Entity component = queue.Dequeue();
                if (component == null)
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                queue.Enqueue(component);

                if (!sceneType.HasSameFlag(component.IScene.SceneType))
                {
                    continue;
                }

                var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IDynamicEventSystem<P1>));
                if (iEventSystems == null)
                {
                    continue;
                }

                foreach (IDynamicEventSystem<P1> iEventSystem in iEventSystems)
                {
                    list.Add(iEventSystem.Run(component, message));
                }
            }

            try
            {
                await ETTaskHelper.WaitAll(list);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }

    //快捷静态扩展 可以直接任意entity.DynamicEvent 就可以发送动态事件
    //直接发送到对应的纤程中
    public static class EntitySystemHelper
    {
        //向任意场景发送动态事件
        public static async ETTask DynamicEvent<P1>(this Entity self, P1 message) where P1 : struct
        {
            await self.DynamicEvent(SceneType.All, message);
        }

        //向与传入的实体相同的场景发送动态事件
        public static async ETTask DynamicEvent<P1>(this Entity self, Entity entity, P1 message) where P1 : struct
        {
            await self.DynamicEvent(entity.IScene.SceneType, message);
        }

        //向目标场景发送动态事件
        public static async ETTask DynamicEvent<P1>(this Entity self, Scene scene, P1 message) where P1 : struct
        {
            await self.DynamicEvent(scene.SceneType, message);
        }

        //向指定场景发送动态事件
        public static async ETTask DynamicEvent<P1>(this Entity self, SceneType sceneType, P1 message) where P1 : struct
        {
            await self.Fiber().EntitySystem.DynamicEvent(sceneType, message);
        }
    }
}