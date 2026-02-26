using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    [Serializable]
    [HideReferenceObjectPicker]
    public abstract partial class UIEventBase
    {
        [LabelText("同步事件")]
        [SerializeField]
        [ReadOnly]
        [HideIf("IsTaskEvent")]
        [PropertyOrder(-10)]
        #if UNITY_EDITOR
        [InfoBox("此事件没有任何关联", InfoMessageType.Warning, "ShowIfBindsTips")]
        #endif
        private string m_EventName;

        [LabelText("异步事件")]
        [ReadOnly]
        [ShowInInspector]
        [ShowIf("IsTaskEvent")]
        [PropertyOrder(-10)]
        #if UNITY_EDITOR
        [InfoBox("此事件没有任何关联", InfoMessageType.Warning, "ShowIfBindsTips")]
        #endif
        public string EventName => m_EventName;

        [SerializeField]
        [ReadOnly]
        [LabelText("当前所有参数列表")]
        #if UNITY_EDITOR
        [ShowIf("ShowIfAllEventParamType")]
        #endif
        public List<EUIEventParamType> AllEventParamType = new List<EUIEventParamType>();

        public abstract bool IsTaskEvent { get; }

        public void RefreshAllEventParamType(List<EUIEventParamType> targetType)
        {
            AllEventParamType.Clear();
            AllEventParamType.AddRange(targetType);
        }

        public void SetName(string name)
        {
            m_EventName = name;
        }

        protected UIEventBase()
        {
        }

        protected UIEventBase(string name)
        {
            m_EventName = name;
        }

        public abstract bool Clear();
    }
}
