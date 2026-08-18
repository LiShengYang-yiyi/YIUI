// 【武侠迁移 2026-08-18】项目自有主界面系统生成代码占位文件。
// 作用：固定 WuXia 主界面系统生成类型，后续由 YIUI 工具更新。
// 原因：主界面生成代码与 Demo 分离，避免互相覆盖。
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
    [EntitySystemOf(typeof(WuXiaMainPanelComponent))]
    public static partial class WuXiaMainPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this WuXiaMainPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this WuXiaMainPanelComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this WuXiaMainPanelComponent self)
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


        }
    }
}
