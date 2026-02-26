using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUITaskEventP1<P1>
    {
        public static readonly ObjectPool<UITaskEventHandleP1<P1>> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 1个泛型参数
    /// </summary>
    public sealed class UITaskEventHandleP1<P1>
    {
        private LinkedList<UITaskEventHandleP1<P1>>     m_UITaskEventList;
        private LinkedListNode<UITaskEventHandleP1<P1>> m_UITaskEventNode;

        public UITaskEventDelegate<P1> UITaskEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public string OnEventInvokeType { get; private set; }

        public UITaskEventHandleP1()
        {
        }

        internal UITaskEventHandleP1<P1> Init(LinkedList<UITaskEventHandleP1<P1>> uiTaskEventList, LinkedListNode<UITaskEventHandleP1<P1>> uiTaskEventNode, Entity trigger, string onEventInvokeType)
        {
            m_UITaskEventList = uiTaskEventList;
            m_UITaskEventNode = uiTaskEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UITaskEventHandleP1<P1> Init(LinkedList<UITaskEventHandleP1<P1>> uiTaskEventList, LinkedListNode<UITaskEventHandleP1<P1>> uiTaskEventNode, UITaskEventDelegate<P1> uiTaskEventDelegate)
        {
            m_UITaskEventList        = uiTaskEventList;
            m_UITaskEventNode        = uiTaskEventNode;
            UITaskEventParamDelegate = uiTaskEventDelegate;
            return this;
        }

        internal async ETTask Invoke(P1 p1)
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType} Trigger == null");
                    return;
                }

                await YIUIInvokeSystem.Instance.InvokeTask(Trigger, OnEventInvokeType, p1);
            }
            else if (UITaskEventParamDelegate != null)
            {
                try
                {
                    await UITaskEventParamDelegate.Invoke(p1);
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