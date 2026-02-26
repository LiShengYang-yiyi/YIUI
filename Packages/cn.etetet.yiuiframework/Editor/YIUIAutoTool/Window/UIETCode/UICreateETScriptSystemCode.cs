using System;
using UnityEngine;

#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreateETScriptSystemCode : BaseTemplate
    {
        private         string m_EventName = "ET-System 代码创建";
        public override string EventName => m_EventName;

        public override bool Cover => false;

        private         bool m_AutoRefresh = false;
        public override bool AutoRefresh => m_AutoRefresh;

        private         bool m_ShowTips = false;
        public override bool ShowTips => m_ShowTips;

        public UICreateETScriptSystemCode(out bool result, string authorName, UICreateETScriptData codeData) : base(authorName)
        {
            var componentType = codeData.ComponentTpye.ToString();
            var path          = $"{codeData.SystemPath}/{codeData.Name}{componentType}System.cs";
            var template      = $"{YIUIConstHelper.Const.UITemplatePath}/ETScript/UICreateETScriptSystemTemplate.txt";
            CreateVo = new CreateVo(template, path);

            m_EventName               = $"{codeData.Name} ET-System 自动生成";
            m_AutoRefresh             = codeData.AutoRefresh;
            m_ShowTips                = codeData.ShowTips;
            ValueDic["Namespace"]     = codeData.Namespace;
            ValueDic["Name"]          = codeData.Name;
            ValueDic["Desc"]          = codeData.Desc;
            ValueDic["ComponentType"] = componentType;
            ValueDic["ObjectSystem"]  = GetLife(codeData);

            result = CreateNewFile();
        }

        private string GetLife(UICreateETScriptData codeData)
        {
            var sbA = SbPool.Get();
            foreach (EETLifeType lifeEnum in Enum.GetValues(typeof(EETLifeType)))
            {
                if (codeData.LifeTpye.HasFlag(lifeEnum))
                {
                    var contentSystem = SwitchLife(codeData, lifeEnum);
                    if (!string.IsNullOrEmpty(contentSystem))
                    {
                        sbA.Append(contentSystem);
                        sbA.AppendLine();
                    }
                }
            }

            return SbPool.PutAndToStr(sbA);
        }

        private string SwitchLife(UICreateETScriptData codeData, EETLifeType life)
        {
            var componentType = codeData.ComponentTpye.ToString();
            switch (life)
            {
                case EETLifeType.All:
                    break;
                case EETLifeType.Def:
                    break;
                case EETLifeType.None:
                    break;
                case EETLifeType.IAwake:
                    return string.Format(lifeTemp, codeData.Name, "Awake", componentType);
                case EETLifeType.IUpdate:
                    return string.Format(lifeTemp, codeData.Name, "Update", componentType);
                case EETLifeType.IDestroy:
                    return string.Format(lifeTemp, codeData.Name, "Destroy", componentType);
                default:
                    Debug.LogError($"是否新增了类型 请检查 {life}");
                    break;
            }

            return "";
        }

        private const string lifeTemp = @"
        [EntitySystem]
        private static void {1}(this {0}{2} self)
        {{
        }}";
    }
}
#endif