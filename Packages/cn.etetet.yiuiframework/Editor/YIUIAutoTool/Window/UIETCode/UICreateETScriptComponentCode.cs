using System;
using UnityEngine;

#if UNITY_EDITOR

namespace YIUIFramework.Editor
{
    public class UICreateETScriptComponentCode : BaseTemplate
    {
        private         string m_EventName = "ET-Component 代码创建";
        public override string EventName => m_EventName;

        public override bool Cover => false;

        private         bool m_AutoRefresh = false;
        public override bool AutoRefresh => m_AutoRefresh;

        private         bool m_ShowTips = false;
        public override bool ShowTips => m_ShowTips;

        public UICreateETScriptComponentCode(out bool result, string authorName, UICreateETScriptData codeData) : base(authorName)
        {
            var componentType = codeData.ComponentTpye.ToString();
            var path          = $"{codeData.ComponentPath}/{codeData.Name}{componentType}.cs";
            var template      = $"{YIUIConstHelper.Const.UITemplatePath}/ETScript/UICreateETScriptCommonTemplate.txt";
            CreateVo = new CreateVo(template, path);

            m_EventName               = $"{codeData.Name} ET-Component 自动生成";
            m_AutoRefresh             = codeData.AutoRefresh;
            m_ShowTips                = codeData.ShowTips;
            ValueDic["Namespace"]     = codeData.Namespace;
            ValueDic["Name"]          = codeData.Name;
            ValueDic["Desc"]          = codeData.Desc;
            ValueDic["ComponentType"] = componentType;
            ValueDic["Life"]          = GetLife(codeData);

            result = CreateNewFile();
        }

        private string GetLife(UICreateETScriptData codeData)
        {
            var life = "";
            foreach (EETLifeType lifeEnum in Enum.GetValues(typeof(EETLifeType)))
            {
                if (codeData.LifeTpye.HasFlag(lifeEnum))
                {
                    life += SwitchLife(lifeEnum);
                }
            }

            return life;
        }

        private string SwitchLife(EETLifeType life)
        {
            switch (life)
            {
                case EETLifeType.All:
                    break;
                case EETLifeType.Def:
                    break;
                case EETLifeType.None:
                    break;
                case EETLifeType.IAwake:
                    return ", IAwake";
                case EETLifeType.IUpdate:
                    return ", IUpdate";
                case EETLifeType.IDestroy:
                    return ", IDestroy";
                default:
                    Debug.LogError($"是否新增了类型 请检查 {life}");
                    break;
            }

            return "";
        }
    }
}
#endif