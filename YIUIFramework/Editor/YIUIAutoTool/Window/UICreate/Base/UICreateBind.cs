#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using YIUIBind;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 绑定与解绑
    /// </summary>
    public static class UICreateBind
    {
        public static string GetBind(UIBindCDETable cdeTable)
        {
            var sb = SbPool.Get();
            cdeTable.GetComponentTable(sb);
            cdeTable.GetDataTable(sb);
            cdeTable.GetEventTable(sb);
            cdeTable.GetCDETable(sb);
            return SbPool.PutAndToStr(sb);
        }

        private static void GetComponentTable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.ComponentTable;
            if (tab == null) return;

            foreach (var value in tab.AllBindDic)
            {
                var name = value.Key;
                if (string.IsNullOrEmpty(name)) continue;
                var bindCom = value.Value;
                if (bindCom == null) continue;
                sb.AppendFormat("            {1} = ComponentTable.FindComponent<{0}>(\"{1}\");\r\n", bindCom.GetType(),
                    name);
            }
        }

        private static void GetDataTable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.DataTable;
            if (tab == null) return;

            foreach (var value in tab.DataDic)
            {
                var name = value.Key;
                if (string.IsNullOrEmpty(name)) continue;
                var uiData    = value.Value;
                var dataValue = uiData?.DataValue;
                if (dataValue == null) continue;
                sb.AppendFormat("            {1} = DataTable.FindDataValue<{0}>(\"{1}\");\r\n", dataValue.GetType(),
                    name);
            }
        }

        private static void GetEventTable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.EventTable;
            if (tab == null) return;

            foreach (var value in tab.EventDic)
            {
                var name = value.Key;
                if (string.IsNullOrEmpty(name)) continue;
                var uiEventBase = value.Value;
                if (uiEventBase == null) continue;
                sb.AppendFormat("            {1} = EventTable.FindEvent<{0}>(\"{1}\");\r\n", uiEventBase.GetEventType(),
                    name);
                sb.AppendFormat("            {0} = {1}.Add({2});\r\n", $"{name}Handle", name,
                    $"OnEvent{name.Replace($"{NameUtility.FirstName}{NameUtility.EventName}", "")}Action");
            }
        }

        private static void GetCDETable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.AllChildCdeTable;
            if (tab == null) return;
            var existName = new HashSet<string>();
            foreach (var value in tab)
            {
                var name = value.name;
                if (string.IsNullOrEmpty(name)) continue;
                var pkgName = value.PkgName;
                var resName = value.ResName;
                if (string.IsNullOrEmpty(resName)) continue;
                var newName = UICreateVariables.GetCDEUIName(name);
                if (existName.Contains(newName))
                {
                    Debug.LogError($"{self.name} 内部公共组件存在同名 请修改 当前会被忽略");
                    continue;
                }

                existName.Add(newName);
                sb.AppendFormat("            {0} = CDETable.FindUIBase<{1}>(\"{2}\");\r\n",
                    newName,
                    $"{UIStaticHelper.UINamespace}.{pkgName}.{resName}",
                    name);
            }
        }

        public static string GetUnBind(UIBindCDETable cdeTable)
        {
            var sb = SbPool.Get();
            cdeTable.GetUnEventTable(sb);
            return SbPool.PutAndToStr(sb);
        }

        private static void GetUnEventTable(this UIBindCDETable self, StringBuilder sb)
        {
            var tab = self.EventTable;
            if (tab == null) return;

            foreach (var value in tab.EventDic)
            {
                var name = value.Key;
                if (string.IsNullOrEmpty(name)) continue;
                var uiEventBase = value.Value;
                if (uiEventBase == null) continue;
                sb.AppendFormat("            {0}.Remove({1});\r\n", name, $"{name}Handle");
            }
        }
    }
}
#endif