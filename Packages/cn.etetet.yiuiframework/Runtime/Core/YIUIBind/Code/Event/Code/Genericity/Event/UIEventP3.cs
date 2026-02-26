using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UIEventP3<P1, P2, P3> : UIEventBase, IUIEventInvoke<P1, P2, P3>
    {
        private LinkedList<UIEventHandleP3<P1, P2, P3>> m_UIEventHandles;
        public  LinkedList<UIEventHandleP3<P1, P2, P3>> UIEventHandles => m_UIEventHandles;

        public UIEventP3()
        {
        }

        public UIEventP3(string name) : base(name)
        {
        }

        public void Invoke(P1 p1, P2 p2, P3 p3)
        {
            if (m_UIEventHandles == null)
            {
                Logger.LogWarning($"{EventName} 未绑定任何事件");
                return;
            }

            var handle = m_UIEventHandles.First;
            while (handle != null)
            {
                var next  = handle.Next;
                var value = handle.Value;

                if (value != null)
                {
                    if (!value.Invoke(p1, p2, p3))
                    {
                        Logger.LogError($"UI事件名称:{EventName} 执行错误 请配合上面报错信息排查");
                    }
                }

                handle = next;
            }
        }

        public override bool IsTaskEvent => false;

        public override bool Clear()
        {
            if (m_UIEventHandles == null) return false;

            var first = m_UIEventHandles.First;
            while (first != null)
            {
                PublicUIEventP3<P1, P2, P3>.HandlerPool.Release(first.Value);
                first = m_UIEventHandles.First;
            }

            LinkedListPool<UIEventHandleP3<P1, P2, P3>>.Release(m_UIEventHandles);
            m_UIEventHandles = null;
            return true;
        }

        public UIEventHandleP3<P1, P2, P3> Add(Entity trigger, string onEventInvokeType)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP3<P1, P2, P3>>.Get();
            var handler = PublicUIEventP3<P1, P2, P3>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, trigger, onEventInvokeType);
        }

        public UIEventHandleP3<P1, P2, P3> Add(UIEventDelegate<P1, P2, P3> callback)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP3<P1, P2, P3>>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP3<P1, P2, P3>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, callback);
        }

        public bool Remove(UIEventHandleP3<P1, P2, P3> handle)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP3<P1, P2, P3>>.Get();

            if (handle == null)
            {
                Logger.LogError($"{EventName} UIEventParamHandle == null");
                return false;
            }

            return m_UIEventHandles.Remove(handle);
        }

        #if UNITY_EDITOR
        public override string GetEventType()
        {
            return $"UIEventP3<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP3<{GetParamTypeString(0)},{GetParamTypeString(1)},{GetParamTypeString(2)}>";
        }
        #endif
    }
}