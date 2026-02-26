using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace YIUIFramework
{
    /// <summary>
    /// 面板拆分数据
    /// 主要做分块加载
    /// </summary>
    [HideReferenceObjectPicker]
    [HideLabel]
    public sealed partial class UIPanelSplitData
    {
        [HideInInspector]
        public GameObject Panel;

        [BoxGroup("通用界面", centerLabel: true)]
        [LabelText("所有子界面父对象")]
        [ReadOnly]
        #if UNITY_EDITOR
        [ShowIf("ShowIfAllView")]
        [DisableIf("DisableIf")]
        #endif
        public RectTransform AllViewParent;

        [BoxGroup("通用界面", centerLabel: true)]
        [LabelText("所有通用界面(已存在不创建的)")]
        #if UNITY_EDITOR
        [ShowIf("ShowIfAllView")]
        [DisableIf("DisableIf")]
        #endif
        public List<RectTransform> AllCommonView = new();

        [BoxGroup("通用界面", centerLabel: true)]
        [LabelText("所有需要被创建的界面")]
        #if UNITY_EDITOR
        [ShowIf("ShowIfAllView")]
        [DisableIf("DisableIf")]
        #endif
        public List<RectTransform> AllCreateView = new();

        [BoxGroup("弹窗界面", centerLabel: true)]
        [LabelText("所有弹出界面父级")]
        [ReadOnly]
        #if UNITY_EDITOR
        [ShowIf("ShowIfAllPopup")]
        [DisableIf("DisableIf")]
        #endif
        public RectTransform AllPopupViewParent;

        [BoxGroup("弹窗界面", centerLabel: true)]
        [LabelText("所有弹出界面")]
        #if UNITY_EDITOR
        [ShowIf("ShowIfAllPopup")]
        [DisableIf("DisableIf")]
        #endif
        public List<RectTransform> AllPopupView = new();

        #if UNITY_EDITOR

        private bool ShowIfAllView()
        {
            return AllViewParent != null;
        }

        private bool ShowIfAllPopup()
        {
            return AllPopupViewParent != null;
        }

        private bool DisableIf()
        {
            return UIOperationHelper.CheckUIOperationAll(Panel, false);
        }
        #endif
    }
}