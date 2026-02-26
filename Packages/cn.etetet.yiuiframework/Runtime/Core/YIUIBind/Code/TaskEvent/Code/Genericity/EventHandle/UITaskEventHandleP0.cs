using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUITaskEventP0
    {
        public static readonly ObjectPool<UITaskEventHandleP0> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 无参数
    /// </summary>
    public sealed class UITaskEventHandleP0
    {
        private LinkedList<UITaskEventHandleP0>     m_UITaskEventList;
        private LinkedListNode<UITaskEventHandleP0> m_UITaskEventNode;

        public UITaskEventDelegate UITaskEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public string OnEventInvokeType { get; private set; }

        public UITaskEventHandleP0()
        {
        }

        internal UITaskEventHandleP0 Init(LinkedList<UITaskEventHandleP0> uiTaskEventList, LinkedListNode<UITaskEventHandleP0> uiTaskEventNode, Entity trigger, string onEventInvokeType)
        {
            m_UITaskEventList = uiTaskEventList;
            m_UITaskEventNode = uiTaskEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UITaskEventHandleP0 Init(LinkedList<UITaskEventHandleP0> uiTaskList, LinkedListNode<UITaskEventHandleP0> uiTaskNode, UITaskEventDelegate uiTaskDelegate)
        {
            m_UITaskEventList        = uiTaskList;
            m_UITaskEventNode        = uiTaskNode;
            UITaskEventParamDelegate = uiTaskDelegate;
            return this;
        }

        internal async ETTask Invoke()
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType} Trigger == null");
                    return;
                }

                await YIUIInvokeSystem.Instance.InvokeTask(Trigger, OnEventInvokeType);
            }
            else if (UITaskEventParamDelegate != null)
            {
                try
                {
                    await UITaskEventParamDelegate.Invoke();
                }
                catch (Exception e)
                {
                    Logger.LogError($"委托:{UITaskEventParamDelegate.GetType().Name} 委托回调错误: {e}");
                }
            }
            else
            {
                Logger.LogError($"没有实现事件 也没有实现委托 请检查");
            }
        }

        public void Dispose()
        {
            OnEventInvokeType        = null;
            UITaskEventParamDelegate = null;
            m_Trigger                = default;
            if (m_UITaskEventList == null || m_UITaskEventNode == null) return;
            m_UITaskEventList.Remove(m_UITaskEventNode);
            m_UITaskEventNode = null;
            m_UITaskEventList = null;
        }
    }
}