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
    [EntitySystemOf(typeof(TipsMessageViewComponent))]
    public static partial class TipsMessageViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TipsMessageViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this TipsMessageViewComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this TipsMessageViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.None;

            self.u_DataMessageContent = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataMessageContent");
            self.u_DataShowClose = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataShowClose");
            self.u_DataShowCancel = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataShowCancel");
            self.u_DataConfirmName = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataConfirmName");
            self.u_DataCancelName = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataCancelName");
            self.u_EventClose = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventClose");
            self.u_EventCloseHandle = self.u_EventClose.Add(self,TipsMessageViewComponent.OnEventCloseInvoke);
            self.u_EventCancel = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventCancel");
            self.u_EventCancelHandle = self.u_EventCancel.Add(self,TipsMessageViewComponent.OnEventCancelInvoke);
            self.u_EventConfirm = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventConfirm");
            self.u_EventConfirmHandle = self.u_EventConfirm.Add(self,TipsMessageViewComponent.OnEventConfirmInvoke);

        }
    }
}
