using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UIEventP0 : UIEventBase, IUIEventInvoke
    {
        private LinkedList<UIEventHandleP0> m_UIEventHandles;
        public  LinkedList<UIEventHandleP0> UIEventHandles => m_UIEventHandles;

        public UIEventP0()
        {
        }

        public UIEventP0(string name) : base(name)
        {
        }

        public void Invoke()
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
                    if (!value.Invoke())
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
                PublicUIEventP0.HandlerPool.Release(first.Value);
                first = m_UIEventHandles.First;
            }

            LinkedListPool<UIEventHandleP0>.Release(m_UIEventHandles);
            m_UIEventHandles = null;
            return true;
        }

        public UIEventHandleP0 Add(Entity trigger, string onEventInvokeType)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP0>.Get();
            var handler = PublicUIEventP0.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, trigger, onEventInvokeType);
        }

        public UIEventHandleP0 Add(UIEventDelegate callback)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP0>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUIEventP0.HandlerPool.Get();
            var node    = m_UIEventHandles.AddLast(handler);
            return handler.Init(m_UIEventHandles, node, callback);
        }

        public bool Remove(UIEventHandleP0 handle)
        {
            m_UIEventHandles ??= LinkedListPool<UIEventHandleP0>.Get();

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
            return "UIEventP0";
        }

        public override string GetEventHandleType()
        {
            return "UIEventHandleP0";
        }
        #endif
    }
}