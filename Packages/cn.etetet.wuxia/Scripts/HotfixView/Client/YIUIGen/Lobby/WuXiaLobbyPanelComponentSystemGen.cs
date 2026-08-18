// 【武侠迁移 2026-08-18】项目自有大厅面板系统生成代码占位文件。
// 作用：固定 WuXia 大厅系统生成类型，后续由 YIUI 工具更新。
// 原因：大厅生成代码与 Demo 分离，避免互相覆盖。
using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIChild))]
    [FriendOf(typeof(YIUIWindowComponent))]
    [FriendOf(typeof(YIUIPanelComponent))]
    [EntitySystemOf(typeof(WuXiaLobbyPanelComponent))]
    public static partial class WuXiaLobbyPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this WuXiaLobbyPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this WuXiaLobbyPanelComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this WuXiaLobbyPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Panel;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;
            self.UIPanel.CachePanelTime = 10;

            self.u_EventEnterMap = self.UIBase.EventTable.FindEvent<UITaskEventP0>("u_EventEnterMap");
            self.u_EventEnterMapHandle = self.u_EventEnterMap.Add(self,WuXiaLobbyPanelComponent.OnEventEnterMapInvoke);

        }
    }
}
