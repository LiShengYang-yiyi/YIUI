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
    [EntitySystemOf(typeof(LoopScrollRectDemoPanelComponent))]
    public static partial class LoopScrollRectDemoPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LoopScrollRectDemoPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LoopScrollRectDemoPanelComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this LoopScrollRectDemoPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Panel;
            self.UIPanel.PanelOption = EPanelOption.None;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;

            self.u_EventTab = self.UIBase.EventTable.FindEvent<UITaskEventP1<int>>("u_EventTab");
            self.u_EventTabHandle = self.u_EventTab.Add(self,LoopScrollRectDemoPanelComponent.OnEventTabInvoke);
            self.u_UIYIUIClose_White = self.UIBase.CDETable.FindUIOwner<ET.Client.YIUICloseCommonComponent>("YIUIClose_White");

        }
    }
}
