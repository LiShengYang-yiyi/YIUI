using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UITaskEventP1<P1> : UIEventBase, IUITaskEventInvoke<P1>
    {
        private LinkedList<UITaskEventHandleP1<P1>> m_UITaskEventHandles;
        public  LinkedList<UITaskEventHandleP1<P1>> UITaskEventHandles => m_UITaskEventHandles;

        public UITaskEventP1()
        {
        }

        public UITaskEventP1(string name) : base(name)
        {
        }

        public async ETTask Invoke(P1 p1)
        {
            if (m_UITaskEventHandles == null)
            {
                Logger.LogWarning($"{EventName} 未绑定任何事件");
                return;
            }

            using var list = ListComponent<ETTask>.Create();

            var handle = m_UITaskEventHandles.First;
            while (handle != null)
            {
                var next  = handle.Next;
                var value = handle.Value;

                if (value != null)
                {
                    list.Add(value.Invoke(p1));
                }

                handle = next;
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

        public override bool IsTaskEvent => true;

        public override bool Clear()
        {
            if (m_UITaskEventHandles == null) return false;

            var first = m_UITaskEventHandles.First;
            while (first != null)
            {
                PublicUITaskEventP1<P1>.HandlerPool.Release(first.Value);
                first = m_UITaskEventHandles.First;
            }

            LinkedListPool<UITaskEventHandleP1<P1>>.Release(m_UITaskEventHandles);
            m_UITaskEventHandles = null;
            return true;
        }

        public UITaskEventHandleP1<P1> Add(Entity trigger, string onEventInvokeType)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP1<P1>>.Get();
            var handler = PublicUITaskEventP1<P1>.HandlerPool.Get();
            var node    = m_UITaskEventHandles.AddLast(handler);
            return handler.Init(m_UITaskEventHandles, node, trigger, onEventInvokeType);
        }

        public UITaskEventHandleP1<P1> Add(UITaskEventDelegate<P1> callback)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP1<P1>>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUITaskEventP1<P1>.HandlerPool.Get();
            var node    = m_UITaskEventHandles.AddLast(handler);
            return handler.Init(m_UITaskEventHandles, node, callback);
        }

        public bool Remove(UITaskEventHandleP1<P1> handle)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP1<P1>>.Get();

            if (handle == null)
            {
                Logger.LogError($"{EventName} UITaskEventParamHandle == null");
                return false;
            }

            return m_UITaskEventHandles.Remove(handle);
        }

        #if UNITY_EDITOR
        public override string GetEventType()
        {
            return $"UITaskEventP1<{GetParamTypeString(0)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UITaskEventHandleP1<{GetParamTypeString(0)}>";
        }
        #endif
    }
}