// 【武侠迁移 2026-08-18】项目自有登录面板系统生成代码占位文件。
// 作用：固定 WuXia 系统生成类型，后续由 YIUI 工具更新。
// 原因：避免生成器重新生成 Demo 同名系统。
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
    [EntitySystemOf(typeof(WuXiaLoginPanelComponent))]
    public static partial class WuXiaLoginPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this WuXiaLoginPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this WuXiaLoginPanelComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this WuXiaLoginPanelComponent self)
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

            self.u_ComAccount = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.InputField>("u_ComAccount");
            self.u_ComPassword = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.InputField>("u_ComPassword");
            self.u_EventLogin = self.UIBase.EventTable.FindEvent<UITaskEventP0>("u_EventLogin");
            self.u_EventLoginHandle = self.u_EventLogin.Add(self,WuXiaLoginPanelComponent.OnEventLoginInvoke);

        }
    }
}
