using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    public static partial class RedDotStackItemComponentSystem
    {
        [ObjectSystem]
        public class RedDotStackItemComponentYIUIBindSystem: YIUIBindSystem<RedDotStackItemComponent>
        {
            protected override void YIUIBind(RedDotStackItemComponent self)
            {
                self.UIBind();
            }
        }
        
        private static void UIBind(this RedDotStackItemComponent self)
        {
            self.UIBase = self.GetParent<YIUIComponent>();

            self.u_ComStackText = self.UIBase.ComponentTable.FindComponent<TMPro.TextMeshProUGUI>("u_ComStackText");
            self.u_DataShowStack = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataShowStack");
            self.u_DataId = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueInt>("u_DataId");
            self.u_DataTime = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataTime");
            self.u_DataOs = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataOs");
            self.u_DataSource = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataSource");
            self.u_EventShowStack = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventShowStack");
            self.u_EventShowStackHandle = self.u_EventShowStack.Add(self.OnEventShowStackAction);

        }
    }
}