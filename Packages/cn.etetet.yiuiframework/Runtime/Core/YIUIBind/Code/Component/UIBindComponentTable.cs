//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    //[DetailedInfoBox("UI 组件表 点击展开详细介绍", @"李胜扬")]
    //[AddComponentMenu("YIUIBind/★★★YIUI Component Table 组件表★★★")]
    [HideLabel]
    [Serializable]
    [HideMonoScript]
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    public sealed partial class UIBindComponentTable : SerializedMonoBehaviour
    {
        [OdinSerialize]
        [LabelText("最终数据")]
        [ReadOnly]
        [PropertyOrder(-9)]
        [OnStateUpdate("@$property.State.Expanded = true")]
        private Dictionary<string, Component> m_AllBindDic = new Dictionary<string, Component>();

        public IReadOnlyDictionary<string, Component> AllBindDic => m_AllBindDic;

        private bool m_Initialized;

        private void Awake()
        {
            InitComponentTable();
        }

        public void InitComponentTable()
        {
            if (m_Initialized) return;
            m_Initialized = true;

            //可能的扩展初始化内容
        }

        private Component FindComponent(string comName)
        {
            m_AllBindDic.TryGetValue(comName, out var value);
            if (value == null)
            {
                Logger.LogErrorContext(this, $" {name} 组件表中没有这个组件 {comName}");
            }

            return value;
        }

        public T FindComponent<T>(string comName) where T : Component
        {
            return (T)FindComponent(comName);
        }
    }
}