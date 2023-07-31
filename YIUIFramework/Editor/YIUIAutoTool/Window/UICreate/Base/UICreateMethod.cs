#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using YIUIBind;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 事件方法
    /// </summary>
    public static class UICreateMethod
    {
        public static string Get(UIBindCDETable cdeTable)
        {
            var sb = SbPool.Get();
            cdeTable.GetEventTable(sb);
            return SbPool.PutAndToStr(sb);
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
                sb.AppendFormat("        protected virtual void {0}({1}){{}}\r\n",
                    $"OnEvent{name.Replace($"{NameUtility.FirstName}{NameUtility.EventName}", "")}Action",
                    GetEventMethodParam(uiEventBase));
            }
        }

        private static string GetEventMethodParam(UIEventBase uiEventBase)
        {
            var paramCount = uiEventBase.AllEventParamType.Count;
            if (paramCount <= 0)
            {
                return "";
            }

            var sb = SbPool.Get();

            for (int i = 0; i < paramCount; i++)
            {
                var paramType = uiEventBase.AllEventParamType[i];
                sb.AppendFormat("{0} p{1}", paramType.GetParamTypeString(), i + 1);
                if (i < paramCount - 1)
                {
                    sb.Append(",");
                }
            }

            return SbPool.PutAndToStr(sb);
        }

        /// <summary>
        /// 子类 帮助直接写上重写事件
        /// </summary>
        public static Dictionary<string, List<Dictionary<string, string>>> GetEventOverrideDic(UIBindCDETable cdeTable)
        {
            var overrideDic = new Dictionary<string, List<Dictionary<string, string>>>();

            #region Event开始

            var newList = new List<Dictionary<string, string>>();
            overrideDic.Add("Event", newList);

            var tab = cdeTable.EventTable;
            if (tab == null) return null;

            foreach (var value in tab.EventDic)
            {
                var name = value.Key;
                if (string.IsNullOrEmpty(name)) continue;
                var uiEventBase = value.Value;
                if (uiEventBase == null) continue;
                var onEvent      = $"OnEvent{name.Replace($"{NameUtility.FirstName}{NameUtility.EventName}", "")}";
                var methodParam  = $"Action({GetEventMethodParam(uiEventBase)})";
                var check        = $"void {onEvent}{methodParam}";
                var firstContent = $"\r\n        protected override void {onEvent}{methodParam}";
                var content      = firstContent + "\r\n        {\r\n            \r\n        }\r\n       ";
                newList.Add(new Dictionary<string, string> { { check, content } });
            }

            #endregion Event结束

            return overrideDic;
        }
    }
}
#endif