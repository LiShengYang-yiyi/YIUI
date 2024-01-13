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
    public static partial class TipsPanelComponentSystem
    {
        [ObjectSystem]
        public class TipsPanelComponentYIUIBindSystem: YIUIBindSystem<TipsPanelComponent>
        {
            protected override void YIUIBind(TipsPanelComponent self)
            {
                self.UIBind();
            }
        }
        
        private static void UIBind(this TipsPanelComponent self)
        {
            self.UIBase = self.GetParent<YIUIComponent>();
            self.UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween|EWindowOption.SkipHomeOpenTween|EWindowOption.AllowOptionByTween;
            self.UIPanel.Layer = EPanelLayer.Tips;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.None;
            self.UIPanel.Priority = 9999;
            self.UIPanel.CachePanelTime = 10;


        }
    }
}