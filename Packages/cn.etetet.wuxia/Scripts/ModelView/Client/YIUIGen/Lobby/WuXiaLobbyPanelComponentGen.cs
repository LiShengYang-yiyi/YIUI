// 【武侠迁移 2026-08-18】项目自有大厅面板生成代码占位文件。
// 作用：固定 WuXia 大厅面板的生成类型，后续由 YIUI 工具重新生成。
// 原因：大厅资源与 Demo 分开管理，避免生成代码相互覆盖。
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
    public partial class WuXiaLobbyPanelComponent : Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "Lobby";
        public const string ResName = "WuXiaLobbyPanel";

        public EntityRef<YIUIChild> u_UIBase;
        public YIUIChild UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel => u_UIPanel;
        public UITaskEventP0 u_EventEnterMap;
        public UITaskEventHandleP0 u_EventEnterMapHandle;
        public const string OnEventEnterMapInvoke = "WuXiaLobbyPanelComponent.OnEventEnterMapInvoke";

    }
}
