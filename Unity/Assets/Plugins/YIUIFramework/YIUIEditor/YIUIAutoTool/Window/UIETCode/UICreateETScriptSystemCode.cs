using System;
using UnityEngine;

#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreateETScriptSystemCode: BaseTemplate
    {
        private         string m_EventName = "ET-System 代码创建";
        public override string EventName => m_EventName;

        public override bool Cover => false;

        private         bool m_AutoRefresh = false;
        public override bool AutoRefresh => m_AutoRefresh;

        private         bool m_ShowTips = false;
        public override bool ShowTips => m_ShowTips;

        public UICreateETScriptSystemCode(out bool result, string authorName, UICreateETScriptData codeData): base(authorName)
        {
            var path     = $"{codeData.SystemPath}/{codeData.Name}ComponentSystem.cs";
            var template = $"{UIStaticHelper.UITemplatePath}/ETScript/UICreateETScriptSystemTemplate.txt";
            CreateVo = new CreateVo(template, path);

            m_EventName              = $"{codeData.Name} ET-System 自动生成";
            m_AutoRefresh            = codeData.AutoRefresh;
            m_ShowTips               = codeData.ShowTips;
            ValueDic["Namespace"]    = codeData.Namespace;
            ValueDic["Name"]         = codeData.Name;
            ValueDic["Desc"]         = codeData.Desc;
            var (system, life)       = GetLife(codeData);
            ValueDic["ObjectSystem"] = system;
            ValueDic["Life"]         = life;

            result = CreateNewFile();
        }

        private (string system, string life) GetLife(UICreateETScriptData codeData)
        {
            var sbA = SbPool.Get();
            var sbB = SbPool.Get();
            foreach (EETLifeTpye lifeEnum in Enum.GetValues(typeof (EETLifeTpye)))
            {
                if (codeData.LifeTpye.HasFlag(lifeEnum))
                {
                    var (contentSystem,contentLife) = SwitchLife(codeData, lifeEnum);
                    if (!string.IsNullOrEmpty(contentSystem))
                    {
                        sbA.Append(contentSystem);
                        sbA.AppendLine();
                        sbB.Append(contentLife);
                        sbB.AppendLine();
                    }
                }
            }

            return (SbPool.PutAndToStr(sbA), SbPool.PutAndToStr(sbB));
        }

        private (string system, string life) SwitchLife(UICreateETScriptData codeData, EETLifeTpye life)
        {
            switch (life)
            {
                case EETLifeTpye.All:
                    break;
                case EETLifeTpye.Def:
                    break;
                case EETLifeTpye.None:
                    break;
                case EETLifeTpye.IAwake:
                    return (string.Format(systemTemp, codeData.Name, "Awake"),
                        string.Format(lifeTemp, codeData.Name, "Awake"));
                case EETLifeTpye.IUpdate:
                    return (string.Format(systemTemp, codeData.Name, "Update"),
                        string.Format(lifeTemp, codeData.Name, "Update"));
                case EETLifeTpye.IDestroy:
                    return (string.Format(systemTemp, codeData.Name, "Destroy"),
                        string.Format(lifeTemp, codeData.Name, "Destroy"));
                default:
                    Debug.LogError($"是否新增了类型 请检查 {life}");
                    break;
            }

            return ("","");
        }

        private const string systemTemp = @"
        [ObjectSystem]
        public class {0}Component{1}System: {1}System<{0}Component>
        {{
            protected override void {1}({0}Component self)
            {{
                self.{1}();
            }}
        }}";
        
        private const string lifeTemp = @"
        private static void {1}(this {0}Component self)
        {{
        }}";
        
    }
}
#endif