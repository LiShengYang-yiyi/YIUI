using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UIEventP1<P1> : UIEventBase, IUIEventInvoke<P1>
    {
        private LinkedList<UIEventHandleP1<P1>> m_UIEventHandles;
        public  LinkedList<UIEventHandleP1<P1>> UIEventHandles => m_UIEventHandles;

        public UIEventP1()
        {
        }

        public UIEventP1(string name) : base(name)
        {
        }

        public void Invoke(P1 p1)
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
                    if (!value.Invoke(p1))
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
                PublicUIEventP1<P1>.HandlerPool.Release(first.Value);
                first = m_UIEventHandles.First;
            }

            LinkedListPool<UIEventHandleP1<P1>>.Release(m_UIEventHandles);
            m_UIEventHandles = null;
            return true;
        }

        public UIEventHandleP1<P1> Add(Entity trigger, string onEventInvokeType)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP1<P1>>.Get();
            var handler = PublicUIEventP1<P1>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, trigger, onEventInvokeType);
        }

        public UIEventHandleP1<P1> Add(UIEventDelegate<P1> callback)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP1<P1>>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP1<P1>.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, callback);
        }

        public bool Remove(UIEventHandleP1<P1> handle)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP1<P1>>.Get();

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
            return $"UIEventP1<{GetParamTypeString(0)}>";
        }

        public override string GetEventHandleType()
        {
            return $"UIEventHandleP1<{GetParamTypeString(0)}>";
        }
        #endif
    }
}