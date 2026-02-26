#if UNITY_EDITOR

using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [Flags]
    public enum EETLifeType
    {
        [LabelText("所有")]
        All = -1,

        [LabelText("默认")]
        Def = IAwake | IDestroy,

        [LabelText("无")]
        None = 0,

        [LabelText("IAwake")]
        IAwake = 1 << 1,

        [LabelText("IUpdate")]
        IUpdate = 1 << 2,

        [LabelText("IDestroy")]
        IDestroy = 1 << 3,
    }

    public enum EETComponentType
    {
        Component = 0,
        Child     = 1,
    }

    /// <summary>
    ///  自动生成ET脚本
    /// </summary>
    [YIUIAutoMenu("ET生成", 200000)]
    public class UIETScriptModule : BaseYIUIToolModule
    {
        [Button("文档", 30, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/XKHYwD7Ppi95f6kqz7IcijmKnQg");
        }

        [HideLabel]
        [BoxGroup("名称")]
        public string ComponentName;

        [HideLabel]
        [BoxGroup("描述")]
        public string ComponentDesc;

        [EnumToggleButtons, HideLabel]
        [BoxGroup("组件类型")]
        public EETComponentType ComponentType = EETComponentType.Component;

        [EnumToggleButtons, HideLabel]
        [BoxGroup("生命周期")]
        public EETLifeType LifeType = EETLifeType.Def;

        private const string ParentFolderPath = "Assets/";

        [BoxGroup("Component路径")]
        [HideLabel]
        [FolderPath(ParentFolder = ParentFolderPath)]
        public string ComponentPath;

        private StringPrefs ComponentPathPrefs = new StringPrefs("UIETScriptModule_ComponentPath", null, "Assets/Scripts/Codes/Model");

        [BoxGroup("System路径")]
        [HideLabel]
        [FolderPath(ParentFolder = ParentFolderPath)]
        public string SystemPath;

        private StringPrefs SystemPathPrefs = new StringPrefs("UIETScriptModule_SystemPath", null, "Assets/Scripts/Codes/Hotfix");

        [Button("生成", 50), GUIColor(0.4f, 0.8f, 1)]
        private void CreateCode()
        {
            if (string.IsNullOrEmpty(ComponentName))
            {
                UnityTipsHelper.ShowError("必须输入名称");
                return;
            }

            ComponentName = NameUtility.ToFirstUpper(ComponentName);

            var data = new UICreateETScriptData
            {
                Namespace     = GetNamespace(),
                Name          = ComponentName,
                Desc          = ComponentDesc,
                ComponentTpye = this.ComponentType,
                LifeTpye      = this.LifeType,
                ComponentPath = ParentFolderPath + ComponentPath,
                SystemPath    = ParentFolderPath + SystemPath,
            };

            new UICreateETScriptComponentCode(out var resultComponent, YIUIAutoTool.Author, data);
            if (resultComponent)
            {
                if ((int)this.LifeType != 0)
                {
                    new UICreateETScriptSystemCode(out var resultSystem, YIUIAutoTool.Author, data);
                }
            }

            YIUIAutoTool.CloseWindowRefresh();
        }

        private string GetNamespace()
        {
            string scriptNamespace = "ET";
            if (ComponentPath.Contains("Client") || ComponentPath.Contains("HotfixView") || ComponentPath.Contains("ModelView"))
            {
                scriptNamespace += ".Client";
            }
            else if (ComponentPath.Contains("Server"))
            {
                scriptNamespace += ".Server";
            }

            return scriptNamespace;
        }

        public override void Initialize()
        {
            base.Initialize();
            ComponentPath = ComponentPathPrefs.Value;
            SystemPath    = SystemPathPrefs.Value;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ComponentPathPrefs.Value = ComponentPath;
            SystemPathPrefs.Value    = SystemPath;
        }
    }
}
#endif
