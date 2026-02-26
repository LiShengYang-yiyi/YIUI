using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UITaskEventP2<P1, P2> : UIEventBase, IUITaskEventInvoke<P1, P2>
    {
        private LinkedList<UITaskEventHandleP2<P1, P2>> m_UITaskEventHandles;
        public  LinkedList<UITaskEventHandleP2<P1, P2>> UITaskEventHandles => m_UITaskEventHandles;

        public UITaskEventP2()
        {
        }

        public UITaskEventP2(string name) : base(name)
        {
        }

        public async ETTask Invoke(P1 p1, P2 p2)
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
                    list.Add(value.Invoke(p1, p2));
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
                PublicUITaskEventP2<P1, P2>.HandlerPool.Release(first.Value);
                first = m_UITaskEventHandles.First;
            }

            LinkedListPool<UITaskEventHandleP2<P1, P2>>.Release(m_UITaskEventHandles);
            m_UITaskEventHandles = null;
            return true;
        }

        public UITaskEventHandleP2<P1, P2> Add(Entity trigger, string onEventInvokeType)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP2<P1, P2>>.Get();
            var handler = PublicUITaskEventP2<P1, P2>.HandlerPool.Get();
            var node    = m_UITaskEventHandles.AddLast(handler);
            return handler.Init(m_UITaskEventHandles, node, trigger, onEventInvokeType);
        }

        public UITaskEventHandleP2<P1, P2> Add(UITaskEventDelegate<P1, P2> callback)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP2<P1, P2>>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUITaskEventP2<P1, P2>.HandlerPool.Get();
            var node    = m_UITaskEventHandles.AddLast(handler);
            return handler.Init(m_UITaskEventHandles, node, callback);
        }

        public bool Remove(UITaskEventHandleP2<P1, P2> handle)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP2<P1, P2>>.Get();

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
            return $"UITaskEventP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UITaskEventHandleP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }
        #endif
    }
}