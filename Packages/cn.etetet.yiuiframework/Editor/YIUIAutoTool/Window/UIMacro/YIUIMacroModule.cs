#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    ///  宏
    /// </summary>
    [YIUIAutoMenu("宏设置", 200100)]
    public class YIUIMacroModule : BaseYIUIToolModule
    {
        [Button("文档", 30, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/F1cDwqKSliIiBIkWMwmcfsj6nUe");
        }

        [BoxGroup("宏", centerLabel: true)]
        [HideLabel]
        [EnumToggleButtons]
        [OnValueChanged("OnMacroTypeChanged")]
        public YIUIMacroType MacroType = YIUIMacroType.ET;

        [HideLabel]
        public BaseYIUIToolModule CurrentMacroModule;

        [HideLabel]
        private EnumPrefs<YIUIMacroType> YIUIMacroPrefs = new("YIUIAutoTool_YIUIMacroModule_MacroType", null, YIUIMacroType.Unity);

        private void OnMacroTypeChanged()
        {
            SelectMacroType(MacroType);
        }

        private void SelectMacroType(YIUIMacroType macroType)
        {
            switch (macroType)
            {
                case YIUIMacroType.ET:
                    CurrentMacroModule = new ETMacroModule();
                    break;
                case YIUIMacroType.Unity:
                    CurrentMacroModule = new UnityMacroModule();
                    break;
                default:
                    UnityTipsHelper.ShowError($"未知的宏类型:{macroType}");
                    CurrentMacroModule = null;
                    break;
            }

            if (CurrentMacroModule != null)
            {
                CurrentMacroModule.Initialize();
            }

            MacroType = macroType;
        }

        public override void Initialize()
        {
            SelectMacroType(YIUIMacroPrefs.Value);
        }

        public override void OnDestroy()
        {
            YIUIMacroPrefs.Value = MacroType;
        }
    }

    public enum YIUIMacroType
    {
        Unity,
        ET
    }
}
#endif
