using System;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [EntitySystemOf(typeof(GMParamItemComponent))]
    public static partial class GMParamItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GMParamItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this GMParamItemComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this GMParamItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();

            self.u_ComInputField = self.UIBase.ComponentTable.FindComponent<TMPro.TMP_InputField>("u_ComInputField");
            self.u_ComToggle = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.Toggle>("u_ComToggle");
            self.u_ComDropdown = self.UIBase.ComponentTable.FindComponent<TMPro.TMP_Dropdown>("u_ComDropdown");
            self.u_DataParamDesc = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataParamDesc");
            self.u_DataTypeValue = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueInt>("u_DataTypeValue");
            self.u_EventInput = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventInput");
            self.u_EventInputHandle = self.u_EventInput.Add(self.OnEventInputAction);
            self.u_EventToggle = self.UIBase.EventTable.FindEvent<UIEventP1<bool>>("u_EventToggle");
            self.u_EventToggleHandle = self.u_EventToggle.Add(self.OnEventToggleAction);
            self.u_EventDropdown = self.UIBase.EventTable.FindEvent<UIEventP1<int>>("u_EventDropdown");
            self.u_EventDropdownHandle = self.u_EventDropdown.Add(self.OnEventDropdownAction);

        }
    }
}