using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [FriendOf(typeof(YIUIWindowComponent))]
    [FriendOf(typeof(YIUIPanelComponent))]
    public static partial class LobbyPanelComponentSystem
    {
        [ObjectSystem]
        public class LobbyPanelComponentYIUIBindSystem: YIUIBindSystem<LobbyPanelComponent>
        {
            protected override void YIUIBind(LobbyPanelComponent self)
            {
                self.UIBind();
            }
        }
        
        private static void UIBind(this LobbyPanelComponent self)
        {
            self.UIBase = self.GetParent<YIUIComponent>();
            self.UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Panel;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;
            self.UIPanel.CachePanelTime = 10;

            self.u_EventEnter = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventEnter");
            self.u_EventEnterHandle = self.u_EventEnter.Add(self.OnEventEnterAction);

        }
    }
}