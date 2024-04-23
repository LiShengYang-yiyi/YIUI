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
        public static async ETTask Event<P1>(P1 message) where P1 : struct
        {
            await Event(SceneType.None, message);
        }

        //向与传入的实体相同的场景发送UI事件
        public static async ETTask Event<P1>(Entity entity, P1 message) where P1 : struct
        {
            await Event(entity.DomainScene().SceneType, message);
        }

        //向目标场景发送UI事件
        public static async ETTask Event<P1>(Scene scene, P1 message) where P1 : struct
        {
            await Event(scene.SceneType, message);
        }

        //P1 = 消息类型 = 如:YIUIEventPanelOpenBefore
        public static async ETTask Event<P1>(SceneType sceneType, P1 message) where P1 : struct
        {
            Queue<long> queue = EventSystem.Instance.queues[(int)InstanceQueueIndex.UIEvent];
            int         count = queue.Count;
            if (count <= 0) return;
            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();

            while (count-- > 0)
            {
                long   instanceId = queue.Dequeue();
                Entity component  = Root.Instance.Get(instanceId);
                if (component == null)
                {
                    continue;
                }

                if (component.IsDisposed)
                {
                    continue;
                }

                queue.Enqueue(instanceId);

                if (sceneType != SceneType.None && sceneType != component.DomainScene().SceneType)
                {
                    continue;
                }

                List<object> iEventSystems = EventSystem.Instance.typeSystems.GetSystems(component.GetType(), typeof (IYIUIEventSystem<P1>));
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