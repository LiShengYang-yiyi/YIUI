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
    public static partial class LoginPanelComponentSystem
    {
        [ObjectSystem]
        public class LoginPanelComponentYIUIBindSystem: YIUIBindSystem<LoginPanelComponent>
        {
            protected override void YIUIBind(LoginPanelComponent self)
            {
                self.UIBind();
            }
        }
        
        private static void UIBind(this LoginPanelComponent self)
        {
            self.UIBase = self.GetParent<YIUIComponent>();
            self.UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Popup;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;
            self.UIPanel.CachePanelTime = 10;

            self.u_EventLogin = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventLogin");
            self.u_EventLoginHandle = self.u_EventLogin.Add(self.OnEventLoginAction);
            self.u_EventAccount = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventAccount");
            self.u_EventAccountHandle = self.u_EventAccount.Add(self.OnEventAccountAction);
            self.u_EventPassword = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventPassword");
            self.u_EventPasswordHandle = self.u_EventPassword.Add(self.OnEventPasswordAction);

        }
    }
}