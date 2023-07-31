//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using YIUIBind;
using UnityEngine;

namespace YIUIFramework
{
    //[DetailedInfoBox("UI CDE总表 点击展开详细介绍", @"李胜扬")]
    [Serializable]
    [LabelText("UI CDE总表")]
    [AddComponentMenu("YIUIBind/★★★★★UI CDE Table 总表★★★★★")]
    public sealed partial class UIBindCDETable : SerializedMonoBehaviour
    {
        #if UNITY_EDITOR
        [InlineButton("AddComponentTable", "Add")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        #endif
        public UIBindComponentTable ComponentTable;

        #if UNITY_EDITOR
        [InlineButton("AddDataTable", "Add")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        #endif
        public UIBindDataTable DataTable;

        #if UNITY_EDITOR
        [InlineButton("AddEventTable", "Add")]
        [EnableIf("@UIOperationHelper.CommonShowIf()")]
        #endif
        public UIBindEventTable EventTable;

        [LabelText("UI包名")]
        [ReadOnly]
        public string PkgName;

        [LabelText("UI资源名")]
        [ReadOnly]
        public string ResName;

        #region 关联

        //关联的UI
        private UIBase m_UIBase;

        [OdinSerialize]
        [LabelText("编辑时所有公共组件")]
        [ReadOnly]
        [PropertyOrder(1000)] //生成UI类时使用
        #if UNITY_EDITOR
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        #endif
        internal List<UIBindCDETable> AllChildCdeTable = new List<UIBindCDETable>();

        [OdinSerialize]
        [NonSerialized]
        [ShowInInspector]
        [ReadOnly]
        [PropertyOrder(1000)]
        [LabelText("运行时所有公共组件")] //动态生成后的子类(公共组件) 运行时使用
        #if UNITY_EDITOR
        [HideIf("@UIOperationHelper.CommonShowIf()")]
        #endif
        private Dictionary<string, UIBase> m_AllChildUIBase = new Dictionary<string, UIBase>();

        internal void AddUIBase(string uiName, UIBase uiBase)
        {
            if (m_AllChildUIBase.ContainsKey(uiName))
            {
                Debug.LogError($"{name} 已存在 {uiName} 请检查为何重复添加 是否存在同名组件");
                return;
            }

            m_AllChildUIBase.Add(uiName, uiBase);
        }

        internal UIBase FindUIBase(string uiName)
        {
            if (!m_AllChildUIBase.ContainsKey(uiName))
            {
                Debug.LogError($"{name} 不存在 {uiName} 请检查");
                return null;
            }

            return m_AllChildUIBase[uiName];
        }

        public T FindUIBase<T>(string uiName) where T : UIBase
        {
            return (T)FindUIBase(uiName);
        }

        #endregion
    }
}