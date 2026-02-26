using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [YIUIAutoMenu("检查", int.MaxValue)]
    public class UICheckModule : BaseYIUIToolModule
    {
        [Button("文档", 30, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/GOBCwA1rdi7nBhkaBh8cLCjRnah");
        }

        [BoxGroup("检查类型", centerLabel: true)]
        [HideLabel]
        [EnumToggleButtons]
        public EYIUICheckViewType CheckViewType = EYIUICheckViewType.Script;

        [HideLabel]
        [ShowIf("CheckViewType", EYIUICheckViewType.Script)]
        public UICheckScriptModule CheckScript = new();

        [HideLabel]
        [ShowIf("CheckViewType", EYIUICheckViewType.Prefab)]
        public UICheckPrefabModule CheckPrefab = new();

        [HideLabel]
        private EnumPrefs<EYIUICheckViewType> YIUICheckViewPrefs = new("YIUIAutoTool_EYIUICheckViewType");

        public override void Initialize()
        {
            CheckViewType = YIUICheckViewPrefs.Value;
        }

        public override void OnDestroy()
        {
            YIUICheckViewPrefs.Value = CheckViewType;
        }
    }
}