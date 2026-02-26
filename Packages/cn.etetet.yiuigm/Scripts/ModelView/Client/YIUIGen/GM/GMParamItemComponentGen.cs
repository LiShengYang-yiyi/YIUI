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
    public partial class GMParamItemComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "GM";
        public const string ResName = "GMParamItem";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public UnityEngine.UI.InputField u_ComInputField;
        public UnityEngine.UI.Toggle u_ComToggle;
        public UnityEngine.UI.Dropdown u_ComDropdown;
        public YIUIFramework.UIDataValueString u_DataParamDesc;
        public YIUIFramework.UIDataValueInt u_DataTypeValue;
        public UIEventP1<string> u_EventInput;
        public UIEventHandleP1<string> u_EventInputHandle;
        public const string OnEventInputInvoke = "GMParamItemComponent.OnEventInputInvoke";
        public UIEventP1<bool> u_EventToggle;
        public UIEventHandleP1<bool> u_EventToggleHandle;
        public const string OnEventToggleInvoke = "GMParamItemComponent.OnEventToggleInvoke";
        public UIEventP1<int> u_EventDropdown;
        public UIEventHandleP1<int> u_EventDropdownHandle;
        public const string OnEventDropdownInvoke = "GMParamItemComponent.OnEventDropdownInvoke";

    }
}