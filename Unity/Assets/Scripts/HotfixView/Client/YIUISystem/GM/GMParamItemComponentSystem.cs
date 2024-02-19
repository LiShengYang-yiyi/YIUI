using System;
using YIUIFramework;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (GMParamItemComponent))]
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
            self.u_ComInputField.text = info.Value;
            switch (info.ParamType)
            {
                case EGMParamType.String:
                    self.u_DataTypeValue.SetValue(1);
                    self.u_ComInputField.contentType = TMP_InputField.ContentType.Alphanumeric;
                    break;
                case EGMParamType.Bool:
                    self.u_DataTypeValue.SetValue(2);
                    self.u_ComToggle.isOn = !string.IsNullOrEmpty(info.Value) && info.Value != "0";
                    break;
                case EGMParamType.Float:
                    self.u_DataTypeValue.SetValue(1);
                    self.u_ComInputField.contentType = TMP_InputField.ContentType.DecimalNumber;
                    break;
                case EGMParamType.Int:
                case EGMParamType.Long:
                    self.u_DataTypeValue.SetValue(1);
                    self.u_ComInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
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
        }

        private static void RefreshDropdownInfo(this GMParamItemComponent self, GMParamInfo info)
        {
            self.OptionList = new();
            self.OptionDic  = new();
            self.u_ComDropdown.ClearOptions();

            var enumType = CodeTypes.Instance.GetType(info.EnumFullName, true);
            if (enumType == null) return;

            if (enumType is { IsEnum: true })
            {
                foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    var showName  = field.Name;
                    var fieldName = field.Name;
                    var attribute = field.GetCustomAttribute<LabelTextAttribute>();
                    if (attribute != null)
                        showName = attribute.Text;
                    self.OptionList.Add(new TMP_Dropdown.OptionData(showName));
                    self.OptionDic.Add(showName, fieldName);

                    //Log.Info($"枚举名称: {field.Name}, 显示文本: {showName}");
                }
            }
            else
            {
                Log.Info($"[{info.EnumFullName}] 不是枚举类型 请检查");
            }

            self.u_ComDropdown.AddOptions(self.OptionList);
            if (string.IsNullOrEmpty(self.ParamInfo.Value) && self.OptionList.Count >= 1)
            {
                self.u_ComDropdown.value = 0;
                self.OnEventDropdownAction(0);
            }
        }

        #region YIUIEvent开始

        private static void OnEventInputAction(this GMParamItemComponent self, string p1)
        {
            self.ParamInfo.Value = p1;
        }

        private static void OnEventToggleAction(this GMParamItemComponent self, bool p1)
        {
            self.ParamInfo.Value = p1? "1" : "0";
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

        #endregion YIUIEvent结束
    }
}