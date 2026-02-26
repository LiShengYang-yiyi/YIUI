using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUIEventP0
    {
        public static readonly ObjectPool<UIEventHandleP0> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 无参数
    /// </summary>
    public sealed class UIEventHandleP0
    {
        private LinkedList<UIEventHandleP0>     m_UIEventList;
        private LinkedListNode<UIEventHandleP0> m_UIEventNode;

        public UIEventDelegate UIEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public string OnEventInvokeType { get; private set; }

        public UIEventHandleP0()
        {
        }

        internal UIEventHandleP0 Init(LinkedList<UIEventHandleP0> uiEventList, LinkedListNode<UIEventHandleP0> uiEventNode, Entity trigger, string onEventInvokeType)
        {
            m_UIEventList     = uiEventList;
            m_UIEventNode     = uiEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UIEventHandleP0 Init(LinkedList<UIEventHandleP0> uiEventList, LinkedListNode<UIEventHandleP0> uiEventNode, UIEventDelegate uiEventDelegate)
        {
            m_UIEventList        = uiEventList;
            m_UIEventNode        = uiEventNode;
            UIEventParamDelegate = uiEventDelegate;
            return this;
        }

        internal bool Invoke()
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType} Trigger == null");
                    return false;
                }

                YIUIInvokeSystem.Instance.Invoke(Trigger, OnEventInvokeType);
                return true;
            }
            else if (UIEventParamDelegate != null)
            {
                try
                {
                    UIEventParamDelegate.Invoke();
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