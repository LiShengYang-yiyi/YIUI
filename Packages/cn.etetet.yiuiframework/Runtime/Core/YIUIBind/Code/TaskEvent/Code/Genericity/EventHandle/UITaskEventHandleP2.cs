using ET;
using System;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// UI事件全局对象池
    /// </summary>
    public static class PublicUITaskEventP2<P1, P2>
    {
        public static readonly ObjectPool<UITaskEventHandleP2<P1, P2>> HandlerPool = new(null, handler => handler.Dispose());
    }

    /// <summary>
    /// UI事件 2个泛型参数
    /// </summary>
    public sealed class UITaskEventHandleP2<P1, P2>
    {
        private LinkedList<UITaskEventHandleP2<P1, P2>>     m_UITaskEventList;
        private LinkedListNode<UITaskEventHandleP2<P1, P2>> m_UITaskEventNode;

        public UITaskEventDelegate<P1, P2> UITaskEventParamDelegate { get; private set; }

        private EntityRef<Entity> m_Trigger;
        public  Entity            Trigger => m_Trigger;

        public string OnEventInvokeType { get; private set; }

        public UITaskEventHandleP2()
        {
        }

        internal UITaskEventHandleP2<P1, P2> Init(LinkedList<UITaskEventHandleP2<P1, P2>> uiTaskEventList, LinkedListNode<UITaskEventHandleP2<P1, P2>> uiTaskEventNode, Entity trigger, string onEventInvokeType)
        {
            m_UITaskEventList = uiTaskEventList;
            m_UITaskEventNode = uiTaskEventNode;
            m_Trigger         = trigger;
            OnEventInvokeType = onEventInvokeType;
            return this;
        }

        internal UITaskEventHandleP2<P1, P2> Init(LinkedList<UITaskEventHandleP2<P1, P2>> uiTaskEventList, LinkedListNode<UITaskEventHandleP2<P1, P2>> uiTaskEventNode, UITaskEventDelegate<P1, P2> uiTaskEventDelegate)
        {
            m_UITaskEventList        = uiTaskEventList;
            m_UITaskEventNode        = uiTaskEventNode;
            UITaskEventParamDelegate = uiTaskEventDelegate;
            return this;
        }

        internal async ETTask Invoke(P1 p1, P2 p2)
        {
            if (OnEventInvokeType != null)
            {
                if (Trigger == null)
                {
                    Log.Error($"事件:{OnEventInvokeType} Trigger == null");
                    return;
                }

                await YIUIInvokeSystem.Instance.InvokeTask(Trigger, OnEventInvokeType, p1, p2);
            }
            else if (UITaskEventParamDelegate != null)
            {
                try
                {
                    await UITaskEventParamDelegate.Invoke(p1, p2);
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