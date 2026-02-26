using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.View)]
    [ComponentOf(typeof(YIUIChild))]
    public partial class TipsMessageViewComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Tips";
        public const string ResName = "TipsMessageView";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIViewComponent> u_UIView;
        public YIUIViewComponent UIView => u_UIView;
        public YIUIFramework.UIDataValueString u_DataMessageContent;
        public YIUIFramework.UIDataValueBool u_DataShowClose;
        public YIUIFramework.UIDataValueBool u_DataShowCancel;
        public YIUIFramework.UIDataValueString u_DataConfirmName;
        public YIUIFramework.UIDataValueString u_DataCancelName;
        public UIEventP0 u_EventClose;
        public UIEventHandleP0 u_EventCloseHandle;
        public const string OnEventCloseInvoke = "TipsMessageViewComponent.OnEventCloseInvoke";
        public UIEventP0 u_EventCancel;
        public UIEventHandleP0 u_EventCancelHandle;
        public const string OnEventCancelInvoke = "TipsMessageViewComponent.OnEventCancelInvoke";
        public UIEventP0 u_EventConfirm;
        public UIEventHandleP0 u_EventConfirmHandle;
        public const string OnEventConfirmInvoke = "TipsMessageViewComponent.OnEventConfirmInvoke";

    }
}