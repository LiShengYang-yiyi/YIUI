using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// GM参数类型
    /// 目前支持的类型
    /// </summary>
    public enum EGMParamType
    {
        [LabelText("字符串")]
        String,

        [LabelText("布尔")]
        Bool,

        [LabelText("小数")]
        Float,

        [LabelText("整数")]
        Int,

        [LabelText("整数64")]
        Long,

        [LabelText("枚举")]
        Enum,
    }

    public static class GMParamExtend
    {
        public static object TryToValue(this EGMParamType self, GMParamInfo info)
        {
            var value = info.Value;
            switch (self)
            {
                case EGMParamType.String:
                    return value;
                case EGMParamType.Bool:
                    value = value?.ToLower();
                    if (string.IsNullOrEmpty(value) || value == "0" || value == "false")
                        return false;
                    return true;
                case EGMParamType.Float:
                    if (string.IsNullOrEmpty(value))
                        return 0f;
                    if (!float.TryParse(value, out var floatValue))
                        Debug.LogError($"参数转换失败 {value} 无法转换 float 请检查");
                    return floatValue;
                case EGMParamType.Int:
                    if (string.IsNullOrEmpty(value))
                        return 0;
                    if (!int.TryParse(value, out var intValue))
                        Debug.LogError($"参数转换失败 {value} 无法转换 int 请检查");
                    return intValue;
                case EGMParamType.Long:
                    if (string.IsNullOrEmpty(value))
                        return 0;
                    if (!long.TryParse(value, out var longValue))
                        Debug.LogError($"参数转换失败 {value} 无法转换 long 请检查");
                    return longValue;
                case EGMParamType.Enum:
                    var enumType = CodeTypes.Instance.GetType(info.EnumFullName);
                    if (enumType == null)
                    {
                        Debug.LogError($"参数转换失败 {info.EnumFullName} 没有找到这个枚举");
                        return null;
                    }

                    try
                    {
                        return Enum.Parse(enumType, value);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"参数转换失败 值:[{value}] 无法转换成 [{enumType}] 的值  {e} ");
                        return null;
                    }
                default:
                    Debug.LogError($"未知类型 无法转换");
                    break;
            }

            return value;
        }
    }
}