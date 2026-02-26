//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

#if UNITY_EDITOR
using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [YIUIAutoMenu("红点", 100000)]
    internal class UIRedDotModule : BaseYIUIToolModule
    {
        [Button("文档", 30, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/XzyawmryHitNVNk9QVtcDAftn5O");
        }

        private EnumPrefs<EUIRedDotViewType> m_EUIRedDotViewTypePrefs =
                new EnumPrefs<EUIRedDotViewType>("AutoUIRedDotModule_EUIRedDotViewTypePrefs", null, EUIRedDotViewType.Key);

        [LabelText("红点包路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        public const string UIRedDotPackagePath = "Packages/cn.etetet.yiuireddot";

        private const string UIRedDotAssetFolderPath = UIRedDotPackagePath + "/Assets/GameRes/RedDot";

        [LabelText("红点枚举资源路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        public const string UIRedDotKeyAssetPath = UIRedDotAssetFolderPath + "/RedDotKeyAsset.asset";

        [ShowInInspector]
        [ReadOnly]
        internal RedDotKeyAsset m_RedDotKeyAsset;

        [LabelText("红点枚举类路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        public string UIRedDotKeyClassPath = UIRedDotPackagePath + "/Scripts/Model/Share/ERedDotKeyType.cs";

        [LabelText("红点关系配置资源路径")]
        [FolderPath]
        [ShowInInspector]
        [ReadOnly]
        public const string UIRedDotConfigAssetPath = UIRedDotAssetFolderPath + "/RedDotConfigAsset.asset";

        [ShowInInspector]
        [ReadOnly]
        internal RedDotConfigAsset m_RedDotConfigAsset;

        [EnumToggleButtons]
        [HideLabel]
        [ShowInInspector]
        [BoxGroup("红点设置", centerLabel: true)]
        [OnValueChanged("OnValueChangedViewSetIndex")]
        private EUIRedDotViewType m_EUIRedDotViewType = EUIRedDotViewType.Key;

        internal Action<EUIRedDotViewType> OnChangeViewSetIndex;

        private void OnValueChangedViewSetIndex()
        {
            m_EUIRedDotViewTypePrefs.Value = m_EUIRedDotViewType;
            OnChangeViewSetIndex?.Invoke(m_EUIRedDotViewType);
        }

        //key界面 (增删改查)
        [ShowInInspector]
        [ShowIf("m_EUIRedDotViewType", EUIRedDotViewType.Key)]
        private UIRedDotKeyView m_KeyView;

        //配置界面 所有key之间的关系配置
        [ShowInInspector]
        [ShowIf("m_EUIRedDotViewType", EUIRedDotViewType.Config)]
        private UIRedDotConfigView m_ConfigView;

        public override void Initialize()
        {
            m_EUIRedDotViewType = m_EUIRedDotViewTypePrefs.Value;
            LoadRedDotKeyAsset();
            LoadRedDotConfigAsset();
            m_KeyView = new UIRedDotKeyView(this);
            m_KeyView.Initialize();
            m_ConfigView = new UIRedDotConfigView(this);
            m_ConfigView.Initialize();
        }

        public override void OnDestroy()
        {
            m_KeyView.OnDestroy();
            m_ConfigView.OnDestroy();
            AssetDatabase.SaveAssets();
        }

        #region Key

        private void LoadRedDotKeyAsset()
        {
            m_RedDotKeyAsset = AssetDatabase.LoadAssetAtPath<RedDotKeyAsset>(UIRedDotKeyAssetPath);

            if (m_RedDotKeyAsset == null)
            {
                CreateRedDotKeyAsset();
            }

            if (m_RedDotKeyAsset == null)
            {
                Debug.LogError($"没有找到 Key 配置资源 且自动创建失败 请检查");
            }
        }

        private void CreateRedDotKeyAsset()
        {
            m_RedDotKeyAsset = ScriptableObject.CreateInstance<RedDotKeyAsset>();

            var assetFolder = $"{Application.dataPath}/../{UIRedDotAssetFolderPath}";
            if (!Directory.Exists(assetFolder))
                Directory.CreateDirectory(assetFolder);

            AssetDatabase.CreateAsset(m_RedDotKeyAsset, UIRedDotKeyAssetPath);
        }

        #endregion

        #region Config

        private void LoadRedDotConfigAsset()
        {
            m_RedDotConfigAsset = AssetDatabase.LoadAssetAtPath<RedDotConfigAsset>(UIRedDotConfigAssetPath);
            if (m_RedDotConfigAsset == null)
            {
                CreateRedDotConfigAsset();
            }

            if (m_RedDotConfigAsset == null)
            {
                Debug.LogError($"没有找到 Config 配置资源 且自动创建失败 请检查");
            }
        }

        private void CreateRedDotConfigAsset()
        {
            m_RedDotConfigAsset = ScriptableObject.CreateInstance<RedDotConfigAsset>();

            var assetFolder = $"{Application.dataPath}/../{UIRedDotAssetFolderPath}";
            if (!Directory.Exists(assetFolder))
                Directory.CreateDirectory(assetFolder);

            AssetDatabase.CreateAsset(m_RedDotConfigAsset, UIRedDotConfigAssetPath);
        }

        #endregion

        [HideLabel]
        internal enum EUIRedDotViewType
        {
            [LabelText("枚举 Key")]
            Key = 1,

            [LabelText("配置 Config")]
            Config = 2,
        }
    }
}
#endif
