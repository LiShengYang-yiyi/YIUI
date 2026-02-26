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
    [FriendOf(typeof(YIUIViewComponent))]
    [EntitySystemOf(typeof(GMViewComponent))]
    public static partial class GMViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GMViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this GMViewComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this GMViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanOpenTween|EWindowOption.BanCloseTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.None;

            self.u_ComGMTypeLoop = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComGMTypeLoop");
            self.u_ComGMCommandLoop = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComGMCommandLoop");
            self.u_UIYIUIClose_White = self.UIBase.CDETable.FindUIOwner<ET.Client.YIUICloseCommonComponent>("YIUIClose_White");

        }
    }
}
