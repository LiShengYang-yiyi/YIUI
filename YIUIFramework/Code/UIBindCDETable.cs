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
    // [DetailedInfoBox("UI CDE总表 点击展开详细介绍", @"李胜扬")]
    [Serializable]
    [LabelText("UI CDE总表")]
    [AddComponentMenu("YIUIBind/★★★★★UI CDE Table 总表★★★★★")]
    public sealed partial class UIBindCDETable : SerializedMonoBehaviour
    {    
#if UNITY_EDITOR
        private bool Enable => UIOperationHelper.CommonShowIf();
        
        [InlineButton(nameof(AddComponentTable), "Add"), EnableIf(nameof(Enable))]
#endif // UNITY_EDITOR
        public UIBindComponentTable ComponentTable;

#if UNITY_EDITOR
        [InlineButton(nameof(AddDataTable), "Add"), EnableIf(nameof(Enable))]
#endif // UNITY_EDITOR
        public UIBindDataTable DataTable;

#if UNITY_EDITOR
        [InlineButton(nameof(AddEventTable), "Add"), EnableIf(nameof(Enable))]
#endif // UNITY_EDITOR
        public UIBindEventTable EventTable;

        [LabelText("UI包名"), ReadOnly]
        public string PkgName;

        [LabelText("UI资源名"), ReadOnly]
        public string ResName;

        #region 关联

        //关联的UI
        private UIBase m_UIBase;

        [OdinSerialize, LabelText("编辑时所有公共组件"), ReadOnly, PropertyOrder(1000)] //生成UI类时使用
#if UNITY_EDITOR
        [ShowIf(nameof(Enable))]
#endif // UNITY_EDITOR
        internal List<UIBindCDETable> AllChildCdeTable = new List<UIBindCDETable>();

        [LabelText("运行时所有公共组件")] //动态生成后的子类(公共组件) 运行时使用
        [OdinSerialize, NonSerialized, ShowInInspector, ReadOnly, PropertyOrder(1000)]
#if UNITY_EDITOR
        [HideIf(nameof(Enable))]
#endif // UNITY_EDITOR
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