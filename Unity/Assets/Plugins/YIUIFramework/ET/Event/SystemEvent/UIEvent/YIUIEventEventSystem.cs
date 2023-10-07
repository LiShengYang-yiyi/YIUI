using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// UI各种事件
    /// </summary>
    public static partial class YIUIEventSystem
    {
        //P1 = 消息类型 = 如:YIUIEventPanelOpenBefore
        public static async ETTask Event<P1>(P1 message) where P1 : struct
        {
            Queue<long>                 queue = EventSystem.Instance.queues[(int)InstanceQueueIndex.UIEvent];
            int                         count = queue.Count;
            using ListComponent<ETTask> list  = ListComponent<ETTask>.Create();

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