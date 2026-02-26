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
    [EntitySystemOf(typeof(ModelDisplayDemoViewComponent))]
    public static partial class ModelDisplayDemoViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ModelDisplayDemoViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this ModelDisplayDemoViewComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this ModelDisplayDemoViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_ComDisplay = self.UIBase.ComponentTable.FindComponent<YIUIFramework.UI3DDisplay>("u_ComDisplay");
            self.u_UIYIUICloseCommon = self.UIBase.CDETable.FindUIOwner<ET.Client.YIUICloseCommonComponent>("YIUICloseCommon");

        }
    }
}
