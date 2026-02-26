using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Common)]
    [ComponentOf(typeof(YIUIChild))]
    public partial class RedDotDataItemComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "RedDot";
        public const string ResName = "RedDotDataItem";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public YIUIFramework.UIDataValueInt u_DataKeyId;
        public YIUIFramework.UIDataValueInt u_DataCount;
        public YIUIFramework.UIDataValueString u_DataName;
        public YIUIFramework.UIDataValueBool u_DataTips;
        public YIUIFramework.UIDataValueInt u_DataParentCount;
        public YIUIFramework.UIDataValueInt u_DataChildCount;
        public YIUIFramework.UIDataValueBool u_DataShowType;
        public YIUIFramework.UIDataValueBool u_DataSwitchTips;
        public UIEventP0 u_EventChild;
        public UIEventHandleP0 u_EventChildHandle;
        public const string OnEventChildInvoke = "RedDotDataItemComponent.OnEventChildInvoke";
        public UIEventP0 u_EventClickItem;
        public UIEventHandleP0 u_EventClickItemHandle;
        public const string OnEventClickItemInvoke = "RedDotDataItemComponent.OnEventClickItemInvoke";
        public UIEventP0 u_EventParent;
        public UIEventHandleP0 u_EventParentHandle;
        public const string OnEventParentInvoke = "RedDotDataItemComponent.OnEventParentInvoke";
        public UIEventP1<bool> u_EventTips;
        public UIEventHandleP1<bool> u_EventTipsHandle;
        public const string OnEventTipsInvoke = "RedDotDataItemComponent.OnEventTipsInvoke";

    }
}