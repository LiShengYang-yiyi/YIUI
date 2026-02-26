using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET.Client
{
    public static class GMKeyHelper
    {
        [StaticField]
        private static List<int> m_AllGMKey;

        [StaticField]
        private static Dictionary<int, string> m_GMKeyDesc;

        /// <summary>
        /// 反射获取枚举列表
        /// 注意: 运行时使用的是ET编译过的dll
        /// 如果新增了没有编译 则无法获取到最新数据 请注意一定要编译后运行
        /// </summary>
        public static List<int> GetKeys(bool force = false)
        {
            if (m_AllGMKey != null && !force)
            {
                return m_AllGMKey;
            }

            HashSet<int> hashKeys = new();
            m_AllGMKey  = new();
            m_GMKeyDesc = new();

            var assembly = YIUIFramework.AssemblyHelper.GetAssembly("ET.ModelView");
            if (assembly == null)
            {
                Log.Error($"没有找到ET.ModelView程序集");
                return m_AllGMKey;
            }

            Type gmKeyType = assembly.GetType("ET.Client.EGMType");
            if (gmKeyType == null)
            {
                Log.Error($"没有找到ET.Client.EGMType类型");
                return m_AllGMKey;
            }

            FieldInfo[] fields = gmKeyType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral && field.FieldType == typeof(int))
                {
                    int key = (int)field.GetValue(null);

                    if (!hashKeys.Add(key))
                    {
                        Log.Error($"GMKey重复: {key}");
                        continue;
                    }

                    m_AllGMKey.Add(key);

                    var keyDesc            = "";
                    var labelTextAttribute = field.GetCustomAttribute<GMGroupAttribute>();
                    if (labelTextAttribute != null)
                    {
                        keyDesc = labelTextAttribute.Name;
                    }

                    m_GMKeyDesc[key] = keyDesc;
                }
            }

            m_AllGMKey.Sort();

            return m_AllGMKey;
        }

        // 获取GM描述
        public static string GetDesc(int key)
        {
            return m_GMKeyDesc?.GetValueOrDefault(key, "");
        }

        // 统一显示GM描述
        public static string GetDisplayDesc(int key)
        {
            return $"{key}_{m_GMKeyDesc?.GetValueOrDefault(key, "")}";
        }
    }
}