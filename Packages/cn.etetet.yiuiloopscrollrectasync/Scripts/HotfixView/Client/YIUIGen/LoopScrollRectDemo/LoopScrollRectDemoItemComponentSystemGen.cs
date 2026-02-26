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
    [EntitySystemOf(typeof(LoopScrollRectDemoItemComponent))]
    public static partial class LoopScrollRectDemoItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LoopScrollRectDemoItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LoopScrollRectDemoItemComponent self)
        {
            self.UIBind();
        }

        private static void UIBind(this LoopScrollRectDemoItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIChild>();

            self.u_DataSelect = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataSelect");
            self.u_DataIndex = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueInt>("u_DataIndex");
            self.u_EventSelect = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventSelect");
            self.u_EventSelectHandle = self.u_EventSelect.Add(self,LoopScrollRectDemoItemComponent.OnEventSelectInvoke);

        }
    }
}
