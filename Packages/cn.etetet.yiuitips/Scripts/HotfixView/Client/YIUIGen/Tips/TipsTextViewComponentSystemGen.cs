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
    [EntitySystemOf(typeof(TipsTextViewComponent))]
    public static partial class TipsTextViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TipsTextViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this TipsTextViewComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this TipsTextViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanOpenTween|EWindowOption.BanCloseTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween;
            self.UIView.ViewWindowType = EViewWindowType.Popup;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_ComContent = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComContent");
            self.u_ComAnimation = self.UIBase.ComponentTable.FindComponent<UnityEngine.Animation>("u_ComAnimation");
            self.u_DataMessageContent = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataMessageContent");

        }
    }
}
