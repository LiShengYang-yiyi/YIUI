using System;
using System.Collections.Generic;

namespace ET
{
    public partial class EventSystem
    {
        public async ETTask DynamicEvent<P1>(P1 message) where P1 : struct
        {
            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();

            Queue<long> queue = this.queues[(int)InstanceQueueIndex.Dynamic];

            int count = queue.Count;

            if (count <= 0) return;

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

                List<object> iEventSystems = this.typeSystems.GetSystems(component.GetType(), typeof (IDynamicEventSystem<P1>));
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
}