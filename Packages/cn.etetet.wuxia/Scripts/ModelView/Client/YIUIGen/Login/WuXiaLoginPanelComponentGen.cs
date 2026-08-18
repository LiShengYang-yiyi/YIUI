// 【武侠迁移 2026-08-18】项目自有登录面板生成代码占位文件。
// 作用：固定 WuXia 登录面板的生成类型，后续由 YIUI 工具重新生成。
// 原因：保留 Demo 资源的同时，避免生成器覆盖项目自有类型。
using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Panel)]
    [ComponentOf(typeof(YIUIChild))]
    public partial class WuXiaLoginPanelComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Login";
        public const string ResName = "WuXiaLoginPanel";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel => u_UIPanel;
        public UnityEngine.UI.InputField u_ComAccount;
        public UnityEngine.UI.InputField u_ComPassword;
        public UITaskEventP0 u_EventLogin;
        public UITaskEventHandleP0 u_EventLoginHandle;
        public const string OnEventLoginInvoke = "WuXiaLoginPanelComponent.OnEventLoginInvoke";

    }
}
