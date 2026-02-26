using System;
using YIUIFramework;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [FriendOf(typeof(GMParamItemComponent))]
    public static partial class GMParamItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMParamItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this GMParamItemComponent self)
        {
        }

        public static void ResetItem(this GMParamItemComponent self, GMParamInfo info)
        {
            self.ParamInfo = info;
            self.u_DataParamDesc.SetValue(info.Desc);
            switch (info.ParamType)
            {
                case EGMParamType.String:
                    self.u_DataTypeValue.SetValue(1);
                    self.u_ComInputField.contentType = InputField.ContentType.Standard;//TMP_InputField.ContentType.Alphanumeric;
                    break;
                case EGMParamType.Bool:
                    self.u_DataTypeValue.SetValue(2);
                    var toggleResult = false;
                    if (!string.IsNullOrEmpty(info.Value))
                    {
                        bool.TryParse(info.Value, out toggleResult);
                    }

                    self.u_ComToggle.isOn = toggleResult;
                    break;
                case EGMParamType.Float:
                    self.u_DataTypeValue.SetValue(1);
                    self.u_ComInputField.contentType = InputField.ContentType.DecimalNumber;//TMP_InputField.ContentType.DecimalNumber;
                    break;
                case EGMParamType.Int:
                case EGMParamType.Long:
                    self.u_DataTypeValue.SetValue(1);
                    self.u_ComInputField.contentType = InputField.ContentType.IntegerNumber;//TMP_InputField.ContentType.IntegerNumber;
                    break;
                case EGMParamType.Enum:
                    self.u_DataTypeValue.SetValue(3);
                    self.RefreshDropdownInfo(info);
                    break;
                default:
                    Debug.LogError($"没有实现这个类型 请检查 {info.ParamType}");
                    self.u_DataTypeValue.SetValue(0);
                    return;
            }
            self.u_ComInputField.lineType = InputField.LineType.MultiLineNewline;
            self.u_ComInputField.text = info.Value;
        }

        private static void RefreshDropdownInfo(this GMParamItemComponent self, GMParamInfo info)
        {
            self.OptionList = new();
            self.OptionDic  = new();
            self.u_ComDropdown.ClearOptions();

            var enumType = CodeTypes.Instance.GetType(info.EnumFullName);
            if (enumType == null) return;
            var dropdownIndex = 0;
            var index         = 0;
            if (enumType is { IsEnum: true })
            {
                foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var showName  = field.Name;
                    var fieldName = field.Name;
                    var attribute = field.GetCustomAttribute<LabelTextAttribute>();
                    if (attribute != null)
                        showName = attribute.Text;
                    self.OptionList.Add(new Dropdown.OptionData(showName));
                    self.OptionDic.Add(showName, fieldName);
                    if (fieldName == info.Value)
                    {
                        dropdownIndex = index;
                    }
                    index++;
                }
            }
            else
            {
                Log.Info($"[{info.EnumFullName}] 不是枚举类型 请检查");
            }

            self.u_ComDropdown.AddOptions(self.OptionList);
            self.u_ComDropdown.value = dropdownIndex;
            self.OnEventDropdownAction(dropdownIndex);
        }

        private static void OnEventDropdownAction(this GMParamItemComponent self, int p1)
        {
            if (p1 < 0 || p1 >= self.OptionList.Count)
            {
                Log.Error($"下拉框索引错误 {p1}");
                return;
            }

            self.ParamInfo.Value = self.OptionDic[self.OptionList[p1].text];
        }

        #region YIUIEvent开始

        [YIUIInvoke(GMParamItemComponent.OnEventDropdownInvoke)]
        private static void OnEventDropdownInvoke(this GMParamItemComponent self, int p1)
        {
            self.OnEventDropdownAction(p1);
        }

        [YIUIInvoke(GMParamItemComponent.OnEventToggleInvoke)]
        private static void OnEventToggleInvoke(this GMParamItemComponent self, bool p1)
        {
            self.ParamInfo.Value = p1 ? "1" : "0";
        }

        [YIUIInvoke(GMParamItemComponent.OnEventInputInvoke)]
        private static void OnEventInputInvoke(this GMParamItemComponent self, string p1)
        {
            self.ParamInfo.Value = p1;
        }

        #endregion YIUIEvent结束
    }
}