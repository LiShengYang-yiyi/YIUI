using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// UI各种事件
    /// </summary>
    public static partial class YIUIEventSystem
    {
        //向任意场景发送UI事件
        public static async ETTask UIEvent<P1>(this Fiber fiber, P1 message) where P1 : struct
        {
            await UIEvent(fiber, SceneType.All, message);
        }

        //向与传入的实体相同的场景发送UI事件
        public static async ETTask UIEvent<P1>(this Fiber fiber, Entity entity, P1 message) where P1 : struct
        {
            await UIEvent(fiber, entity.Root().SceneType, message);
        }

        //向目标场景发送UI事件
        public static async ETTask UIEvent<P1>(this Fiber fiber, Scene scene, P1 message) where P1 : struct
        {
            await UIEvent(fiber, scene.SceneType, message);
        }

        //P1 = 消息类型 = 如:YIUIEventPanelOpenBefore
        public static async ETTask UIEvent<P1>(this Fiber fiber, SceneType sceneType, P1 message) where P1 : struct
        {
            var queue = fiber.EntitySystem.Queues[InstanceQueueIndex.UIEvent];
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

                var iEventSystems = EntitySystemSingleton.Instance.TypeSystems.GetSystems(component.GetType(), typeof(IYIUIEventSystem<P1>));
                if (iEventSystems == null)
                {
                    continue;
                }

                foreach (IYIUIEventSystem<P1> iEventSystem in iEventSystems)
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
}