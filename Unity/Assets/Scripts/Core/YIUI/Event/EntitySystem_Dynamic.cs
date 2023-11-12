using System;
using System.Collections.Generic;

namespace ET
{
    public partial class EntitySystem
    {
        public Queue<EntityRef<Entity>>[] Queues => queues;
        
        public async ETTask DynamicEvent<P1>(P1 message) where P1 : struct
        {
            using ListComponent<ETTask> list = ListComponent<ETTask>.Create();

            var queue = queues[InstanceQueueIndex.Dynamic];
            int count = queue.Count;

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
                
                var componentType = component.GetType();

                List<object> iBaseEventSystems = 
                        EntitySystemSingleton.Instance.TypeSystems.GetSystems(componentType, typeof (IDynamicEvent));
                if (iBaseEventSystems == null)
                {
                    continue;
                }
                
                queue.Enqueue(component);

                List<object> iEventSystems =
                        EntitySystemSingleton.Instance.TypeSystems.GetSystems(componentType, typeof (IDynamicEvent<P1>));
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