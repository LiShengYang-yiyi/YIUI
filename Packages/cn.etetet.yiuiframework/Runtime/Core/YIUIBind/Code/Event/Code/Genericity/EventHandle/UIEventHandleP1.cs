using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP1<P1>
    {
        public static readonly ObjectPool<UIEventHandleP1<P1>> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 1个泛型参数
    /// </summary>
    public sealed class UIEventHandleP1<P1>
    {
        private LinkedList<UIEventHandleP1<P1>>     m_UIEventList;
        private LinkedListNode<UIEventHandleP1<P1>> m_UIEventNode;

        public UIEventDelegate<P1> UIEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public string OnEventInvokeType { get; private set; }

        public UIEventHandleP1()
        {
        }

        internal UIEventHandleP1<P1> Init(LinkedList<UIEventHandleP1<P1>> uiEventList, LinkedListNode<UIEventHandleP1<P1>> uiEventNode, Entity trigger, string onEventInvokeType)
        {
            m_UIEventList     = uiEventList;
            m_UIEventNode     = uiEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UIEventHandleP1<P1> Init(LinkedList<UIEventHandleP1<P1>> uiEventList, LinkedListNode<UIEventHandleP1<P1>> uiEventNode, UIEventDelegate<P1> uiEventDelegate)
        {
            m_UIEventList        = uiEventList;
            m_UIEventNode        = uiEventNode;
            UIEventParamDelegate = uiEventDelegate;
            return this;
        }

        internal bool Invoke(P1 p1)
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType} Trigger == null");
                    return false;
                }

                YIUIInvokeSystem.Instance.Invoke(Trigger, OnEventInvokeType, p1);
                return true;
            }
            else if (UIEventParamDelegate != null)
            {
                try
                {
                    UIEventParamDelegate.Invoke(p1);
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