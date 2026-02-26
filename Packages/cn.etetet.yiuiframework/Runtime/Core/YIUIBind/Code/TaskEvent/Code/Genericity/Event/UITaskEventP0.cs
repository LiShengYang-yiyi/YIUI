using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    public class UITaskEventP0 : UIEventBase, IUITaskEventInvoke
    {
        private LinkedList<UITaskEventHandleP0> m_UITaskEventHandles;
        public  LinkedList<UITaskEventHandleP0> UITaskEventHandles => m_UITaskEventHandles;

        public UITaskEventP0()
        {
        }

        public UITaskEventP0(string name) : base(name)
        {
        }

        public async ETTask Invoke()
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
                    list.Add(value.Invoke());
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
                PublicUITaskEventP0.HandlerPool.Release(first.Value);
                first = m_UITaskEventHandles.First;
            }

            LinkedListPool<UITaskEventHandleP0>.Release(m_UITaskEventHandles);
            m_UITaskEventHandles = null;
            return true;
        }

        public UITaskEventHandleP0 Add(Entity trigger, string onEventInvokeType)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP0>.Get();
            var handler = PublicUITaskEventP0.HandlerPool.Get();
            var node    = m_UITaskEventHandles.AddLast(handler);
            return handler.Init(m_UITaskEventHandles, node, trigger, onEventInvokeType);
        }

        public UITaskEventHandleP0 Add(UITaskEventDelegate callback)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP0>.Get();

            if (callback == null)
            {
                Logger.LogError($"{EventName} 添加了一个空回调");
            }

            var handler = PublicUITaskEventP0.HandlerPool.Get();
            var node    = m_UITaskEventHandles.AddLast(handler);
            return handler.Init(m_UITaskEventHandles, node, callback);
        }

        public bool Remove(UITaskEventHandleP0 handle)
        {
            m_UITaskEventHandles ??= LinkedListPool<UITaskEventHandleP0>.Get();

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
            return "UITaskEventP0";
        }

        public override string GetEventHandleType()
        {
            return "UITaskEventHandleP0";
        }
        #endif
    }
}