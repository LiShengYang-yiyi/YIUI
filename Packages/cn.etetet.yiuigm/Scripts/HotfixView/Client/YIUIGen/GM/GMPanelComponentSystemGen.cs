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
    [EntitySystemOf(typeof(GMPanelComponent))]
    public static partial class GMPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GMPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this GMPanelComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this GMPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanOpenTween|EWindowOption.BanCloseTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween;
            self.UIPanel.Layer = EPanelLayer.Top;
            self.UIPanel.PanelOption = EPanelOption.ForeverCache|EPanelOption.DisClose|EPanelOption.IgnoreBack|EPanelOption.IgnoreClose;
            self.UIPanel.StackOption = EPanelStackOption.None;
            self.UIPanel.Priority = 99999;

            self.u_ComGMButton = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComGMButton");
            self.u_ComLimitRange = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComLimitRange");
            self.u_EventOpenGMView = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventOpenGMView");
            self.u_EventOpenGMViewHandle = self.u_EventOpenGMView.Add(self,GMPanelComponent.OnEventOpenGMViewInvoke);
            self.u_EventBeginDrag = self.UIBase.EventTable.FindEvent<UIEventP1<object>>("u_EventBeginDrag");
            self.u_EventBeginDragHandle = self.u_EventBeginDrag.Add(self,GMPanelComponent.OnEventBeginDragInvoke);
            self.u_EventEndDrag = self.UIBase.EventTable.FindEvent<UIEventP1<object>>("u_EventEndDrag");
            self.u_EventEndDragHandle = self.u_EventEndDrag.Add(self,GMPanelComponent.OnEventEndDragInvoke);
            self.u_EventDrag = self.UIBase.EventTable.FindEvent<UIEventP1<object>>("u_EventDrag");
            self.u_EventDragHandle = self.u_EventDrag.Add(self,GMPanelComponent.OnEventDragInvoke);

        }
    }
}
