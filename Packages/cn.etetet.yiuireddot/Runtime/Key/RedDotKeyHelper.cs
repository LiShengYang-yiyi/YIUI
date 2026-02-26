using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

namespace YIUIFramework
{
    public static class RedDotKeyHelper
    {
        private static List<int> m_AllRedDotKey;

        private static List<int> AllRedDotKey
        {
            get
            {
                if (m_AllRedDotKey == null)
                {
                    GetKeys();
                }

                return m_AllRedDotKey;
            }
        }

        private static HashSet<int> m_HashAllRedDotKey;

        /// <summary>
        /// 检查这个Key 是否存在
        /// </summary>
        public static bool ContainsKey(int key)
        {
            if (m_HashAllRedDotKey == null)
            {
                m_HashAllRedDotKey = new();
                m_HashAllRedDotKey.AddRange(AllRedDotKey);
            }

            return m_HashAllRedDotKey.Contains(key);
        }

        /// <summary>
        /// 反射获取枚举列表
        /// 注意: 运行时使用的是ET编译过的dll
        /// 如果新增了没有编译 则无法获取到最新数据 请注意一定要编译后运行
        /// </summary>
        public static List<int> GetKeys(bool force = false)
        {
            if (m_AllRedDotKey != null && !force)
            {
                return m_AllRedDotKey;
            }

            m_AllRedDotKey     = new();
            m_HashAllRedDotKey = null;
            #if UNITY_EDITOR
            m_RedDotKeyDesc = new();
            #endif

            var assembly = AssemblyHelper.GetAssembly("ET.Model");
            if (assembly == null)
            {
                Logger.LogError($"没有找到ET.Model程序集");
                return m_AllRedDotKey;
            }

            Type redDotKeyType = assembly.GetType("ET.ERedDotKeyType");
            if (redDotKeyType == null)
            {
                Logger.LogError($"没有找到ET.ERedDotKeyType类型");
                return m_AllRedDotKey;
            }

            FieldInfo[] fields = redDotKeyType.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (FieldInfo field in fields)
            {
                if (field.IsLiteral && field.FieldType == typeof(int))
                {
                    int key = (int)field.GetValue(null);
                    m_AllRedDotKey.Add(key);

                    #if UNITY_EDITOR
                    var keyDesc            = "";
                    var labelTextAttribute = field.GetCustomAttribute<LabelTextAttribute>();
                    if (labelTextAttribute != null)
                    {
                        keyDesc = labelTextAttribute.Text;
                    }

                    m_RedDotKeyDesc[key] = keyDesc;
                    #endif
                }
            }

            return m_AllRedDotKey;
        }

        #if UNITY_EDITOR
        private static Dictionary<int, string> m_RedDotKeyDesc;

        // 获取红点描述
        // Editor 时可用
        internal static string GetDesc(int key)
        {
            return m_RedDotKeyDesc?.GetValueOrDefault(key, "");
        }

        // 统一显示红点描述
        internal static string GetDisplayDesc(int key)
        {
            return $"{key}_{m_RedDotKeyDesc?.GetValueOrDefault(key, "")}";
        }

        private const int MAX_GROUP_SIZE = 10;

        // 显示红点描述的下拉列表 超过10个时分组显示
        internal static ValueDropdownList<int> SubDisplayValueDropdownList(ValueDropdownList<int> keys)
        {
            //排序
            keys.Sort((a, b) =>
            {
                var aId = a.Value;
                var bId = b.Value;
                return aId.CompareTo(bId);
            });

            if (keys.Count <= MAX_GROUP_SIZE)
            {
                return keys;
            }

            return GetGroupValueDropdownList(keys, MAX_GROUP_SIZE);
        }

        private static ValueDropdownList<int> GetGroupValueDropdownList(ValueDropdownList<int> numbers, int groupSize)
        {
            ValueDropdownList<int> allGroups = new ValueDropdownList<int>();
            int                    count     = numbers.Count;

            while (count > 0)
            {
                int groupCount = Math.Min(groupSize, count);

                var startItem   = numbers[0];
                var startItemId = startItem.Value;
                var endItem     = numbers[groupCount - 1];
                var endItemId   = endItem.Value;
                var groupText   = $"{startItemId} - {endItemId} ";

                for (int i = 0; i < groupCount; i++)
                {
                    var item    = numbers[0];
                    var newItem = new ValueDropdownItem<int>();
                    newItem.Text  = $"{groupText}/{item.Text}";
                    newItem.Value = item.Value;
                    allGroups.Add(newItem);
                    numbers.RemoveAt(0);
                }

                count -= groupCount;
            }

            int nextGroupSize = groupSize * MAX_GROUP_SIZE;

            if (allGroups.Count > nextGroupSize)
            {
                return GetGroupValueDropdownList(allGroups, nextGroupSize);
            }

            return allGroups;
        }

        #endif
    }
}