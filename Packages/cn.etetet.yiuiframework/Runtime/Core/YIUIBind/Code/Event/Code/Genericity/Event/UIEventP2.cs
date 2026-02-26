using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UIEventP2<P1, P2> : UIEventBase, IUIEventInvoke<P1, P2>
    {
        private LinkedList<UIEventHandleP2<P1, P2>> m_UIEventHandles;
        public  LinkedList<UIEventHandleP2<P1, P2>> UIEventHandles => m_UIEventHandles;

        public UIEventP2()
        {
        }

        public UIEventP2(string name) : base(name)
        {
        }

        public void Invoke(P1 p1, P2 p2)
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
                    if (!value.Invoke(p1, p2))
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
                PublicUIEventP2<P1, P2>.HandlerPool.Release(first.Value);
                first = m_UIEventHandles.First;
            }

            LinkedListPool<UIEventHandleP2<P1, P2>>.Release(m_UIEventHandles);
            m_UIEventHandles = null;
            return true;
        }

        public UIEventHandleP2<P1, P2> Add(Entity trigger, string onEventInvokeType)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP2<P1, P2>>.Get();
            var handler = PublicUIEventP2<P1, P2>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, trigger, onEventInvokeType);
        }

        public UIEventHandleP2<P1, P2> Add(UIEventDelegate<P1, P2> callback)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP2<P1, P2>>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP2<P1, P2>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, callback);
        }

        public bool Remove(UIEventHandleP2<P1, P2> handle)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP2<P1, P2>>.Get();

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
            return $"UIEventP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP2<{GetParamTypeString(0)},{GetParamTypeString(1)}>";
        }
        #endif
    }
}