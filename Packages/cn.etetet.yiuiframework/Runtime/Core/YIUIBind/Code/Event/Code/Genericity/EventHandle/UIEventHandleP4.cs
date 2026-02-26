using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP4<P1, P2, P3, P4>
    {
        public static readonly ObjectPool<UIEventHandleP4<P1, P2, P3, P4>> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 4个泛型参数
    /// </summary>
    public sealed class UIEventHandleP4<P1, P2, P3, P4>
    {
        private LinkedList<UIEventHandleP4<P1, P2, P3, P4>>     m_UIEventList;
        private LinkedListNode<UIEventHandleP4<P1, P2, P3, P4>> m_UIEventNode;

        public UIEventDelegate<P1, P2, P3, P4> UIEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public string OnEventInvokeType { get; private set; }

        public UIEventHandleP4()
        {
        }

        internal UIEventHandleP4<P1, P2, P3, P4> Init(LinkedList<UIEventHandleP4<P1, P2, P3, P4>> uiEventList, LinkedListNode<UIEventHandleP4<P1, P2, P3, P4>> uiEventNode, Entity trigger, string onEventInvokeType)
        {
            m_UIEventList     = uiEventList;
            m_UIEventNode     = uiEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UIEventHandleP4<P1, P2, P3, P4> Init(LinkedList<UIEventHandleP4<P1, P2, P3, P4>> uiEventList, LinkedListNode<UIEventHandleP4<P1, P2, P3, P4>> uiEventNode, UIEventDelegate<P1, P2, P3, P4> uiEventDelegate)
        {
            m_UIEventList        = uiEventList;
            m_UIEventNode        = uiEventNode;
            UIEventParamDelegate = uiEventDelegate;
            return this;
        }

        internal bool Invoke(P1 p1, P2 p2, P3 p3, P4 p4)
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType} Trigger == null");
                    return false;
                }

                YIUIInvokeSystem.Instance.Invoke(Trigger, OnEventInvokeType, p1, p2, p3, p4);
                return true;
            }
            else if (UIEventParamDelegate != null)
            {
                try
                {
                    UIEventParamDelegate.Invoke(p1, p2, p3, p4);
                    return true;
                }
                catch (Exception e)
                {
                    Logger.LogError($"委托:{UIEventParamDelegate.GetType().Name} 委托回调错误: {e}");
                }
            }
            else
            {
                Logger.LogError($"没有实现事件 也没有实现委托 请检查");
            }

            return false;
        }

        public void Dispose()
        {
            OnEventInvokeType    = null;
            UIEventParamDelegate = null;
            m_Trigger            = default;
            if (m_UIEventList == null || m_UIEventNode == null) return;
            m_UIEventList.Remove(m_UIEventNode);
            m_UIEventNode = null;
            m_UIEventList = null;
        }
    }
}