using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Tips)]
    [ComponentOf(typeof(YIUIChild))]
    public partial class RedDotPanelComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "RedDot";
        public const string ResName = "RedDotPanel";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel => u_UIPanel;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComSearchScroll;
        public UnityEngine.UI.Dropdown u_ComDropdownSearch;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComStackScroll;
        public UnityEngine.UI.InputField u_ComInputChangeCount;
        public YIUIFramework.UIDataValueBool u_DataDropdownSearch;
        public YIUIFramework.UIDataValueString u_DataInfoName;
        public YIUIFramework.UIDataValueBool u_DataToggleUnityEngine;
        public YIUIFramework.UIDataValueBool u_DataToggleYIUIBind;
        public YIUIFramework.UIDataValueBool u_DataToggleYIUIFramework;
        public YIUIFramework.UIDataValueBool u_DataToggleShowIndex;
        public YIUIFramework.UIDataValueBool u_DataToggleShowFileName;
        public YIUIFramework.UIDataValueBool u_DataToggleShowFilePath;
        public EntityRef<ET.Client.YIUICloseCommonComponent> u_UIYIUIClose_White;
        public ET.Client.YIUICloseCommonComponent UIYIUIClose_White => u_UIYIUIClose_White;
        public UIEventP1<string> u_EventChangeCount;
        public UIEventHandleP1<string> u_EventChangeCountHandle;
        public const string OnEventChangeCountInvoke = "RedDotPanelComponent.OnEventChangeCountInvoke";
        public UIEventP1<bool> u_EventChangeToggleShowFileName;
        public UIEventHandleP1<bool> u_EventChangeToggleShowFileNameHandle;
        public const string OnEventChangeToggleShowFileNameInvoke = "RedDotPanelComponent.OnEventChangeToggleShowFileNameInvoke";
        public UIEventP1<bool> u_EventChangeToggleShowFilePath;
        public UIEventHandleP1<bool> u_EventChangeToggleShowFilePathHandle;
        public const string OnEventChangeToggleShowFilePathInvoke = "RedDotPanelComponent.OnEventChangeToggleShowFilePathInvoke";
        public UIEventP1<bool> u_EventChangeToggleShowStackIndex;
        public UIEventHandleP1<bool> u_EventChangeToggleShowStackIndexHandle;
        public const string OnEventChangeToggleShowStackIndexInvoke = "RedDotPanelComponent.OnEventChangeToggleShowStackIndexInvoke";
        public UIEventP1<bool> u_EventChangeToggleUnityEngine;
        public UIEventHandleP1<bool> u_EventChangeToggleUnityEngineHandle;
        public const string OnEventChangeToggleUnityEngineInvoke = "RedDotPanelComponent.OnEventChangeToggleUnityEngineInvoke";
        public UIEventP1<bool> u_EventChangeToggleYIUIBind;
        public UIEventHandleP1<bool> u_EventChangeToggleYIUIBindHandle;
        public const string OnEventChangeToggleYIUIBindInvoke = "RedDotPanelComponent.OnEventChangeToggleYIUIBindInvoke";
        public UIEventP1<bool> u_EventChangeToggleYIUIFramework;
        public UIEventHandleP1<bool> u_EventChangeToggleYIUIFrameworkHandle;
        public const string OnEventChangeToggleYIUIFrameworkInvoke = "RedDotPanelComponent.OnEventChangeToggleYIUIFrameworkInvoke";
        public UIEventP1<int> u_EventDropdownSearch;
        public UIEventHandleP1<int> u_EventDropdownSearchHandle;
        public const string OnEventDropdownSearchInvoke = "RedDotPanelComponent.OnEventDropdownSearchInvoke";
        public UIEventP1<string> u_EventInputSearchEnd;
        public UIEventHandleP1<string> u_EventInputSearchEndHandle;
        public const string OnEventInputSearchEndInvoke = "RedDotPanelComponent.OnEventInputSearchEndInvoke";

    }
}