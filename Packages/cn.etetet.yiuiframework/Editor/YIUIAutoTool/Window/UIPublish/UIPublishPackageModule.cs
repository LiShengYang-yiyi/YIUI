#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace YIUIFramework.Editor
{
    public enum EUIPublishPackageData
    {
        [LabelText("组件")]
        CDETable,

        [LabelText("精灵")]
        Sprites,

        [LabelText("图集")]
        Atlas,
    }

    public class UIPublishPackageModuleData
    {
        public UIPublishModule PublishModule;
        public string PublishPath;
        public string ResPath;
        public string PkgName;
    }

    public partial class UIPublishPackageModule : BaseYIUIToolModule
    {
        private UIPublishModule m_UIPublishModule;

        private UIAtlasModule m_UIAtlasModule;

        [OnInspectorGUI]
        [PropertyOrder(999)]
        private void BaseSpace1()
        {
            GUILayout.Space(20);
        }

        [PropertyOrder(1000)]
        [BoxGroup("基础信息", centerLabel: true)]
        [LabelText("模块名")]
        [ReadOnly]
        public string PkgName;

        [PropertyOrder(1000)]
        [BoxGroup("基础信息", centerLabel: true)]
        [FolderPath]
        [LabelText("模块路径")]
        [ReadOnly]
        public string PkgPath;

        private UIPublishPackageModuleData Data;

        [PropertyOrder(1000)]
        [BoxGroup("基础信息", centerLabel: true)]
        [EnumToggleButtons]
        [HideLabel]
        public EUIPublishPackageData m_UIPublishPackageData = EUIPublishPackageData.CDETable;

        [PropertyOrder(1000)]
        [BoxGroup("基础信息", centerLabel: true)]
        [LabelText("当前模块所有组件")]
        [ReadOnly]
        [ShowInInspector]
        [OnStateUpdate("@$property.State.Expanded = true")]
        [ShowIf("m_UIPublishPackageData", EUIPublishPackageData.CDETable)]
        private List<UIBindCDETable> m_AllCDETable = new List<UIBindCDETable>();

        [PropertyOrder(1000)]
        [BoxGroup("基础信息", centerLabel: true)]
        [LabelText("当前模块所有精灵")]
        [ReadOnly]
        [ShowInInspector]
        [OnStateUpdate("@$property.State.Expanded = true")]
        [ShowIf("m_UIPublishPackageData", EUIPublishPackageData.Sprites)]
        private List<TextureImporter> m_AllTextureImporter = new List<TextureImporter>();

        //根据精灵文件夹创建对应的图集数量
        [LabelText("所有图集名称")]
        [PropertyOrder(1000)]
        [BoxGroup("基础信息", centerLabel: true)]
        [ReadOnly]
        [HideInInspector]
        [OnStateUpdate("@$property.State.Expanded = true")]
        [ShowIf("m_UIPublishPackageData", EUIPublishPackageData.Atlas)]
        public HashSet<string> m_AtlasName = new HashSet<string>();

        [PropertyOrder(1000)]
        [BoxGroup("基础信息", centerLabel: true)]
        [LabelText("当前模块所有图集")]
        [ReadOnly]
        [ShowInInspector]
        [OnStateUpdate("@$property.State.Expanded = true")]
        [ShowIf("m_UIPublishPackageData", EUIPublishPackageData.Atlas)]
        private List<SpriteAtlas> m_AllSpriteAtlas = new List<SpriteAtlas>();

        [GUIColor(0.4f, 0.8f, 1)]
        [PropertyOrder(-999)]
        [Button("发布当前模块", 50, Icon = SdfIconType.Upload, IconAlignment = IconAlignment.LeftOfText)]
        private void PublishCurrent()
        {
            PublishCurrent(true);
        }

        public void PublishCurrent(bool showTips)
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            foreach (var current in m_AllCDETable)
            {
                if (string.IsNullOrEmpty(current.PackagesName))
                {
                    UICreateModule.CreateCommon(current, false, false);
                }
                else
                {
                    UICreateModule.CreatePackages(current, false, false, current.PackagesName);
                }
            }

            //创建图集 不重置 需要重置需要手动
            CreateOrResetAtlas();

            if (showTips)
            {
                UnityTipsHelper.CallBackOk($"YIUI当前模块 {PkgName} 发布完毕", YIUIAutoTool.CloseWindowRefresh);
            }
        }

        [Button("创建or重置 文件结构", 30)]
        [PropertyOrder(-998)]
        public void ResetDirectory()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            UICreateResModule.Create(PkgName);
        }

        #region 初始化

        public UIPublishPackageModule()
        {
        }

        public UIPublishPackageModule(UIPublishModule publishModule, string resPath, string pkgName)
        {
            SetData(publishModule, resPath, pkgName);
            Refresh();
        }

        private void SetData(UIPublishModule publishModule, string resPath, string pkgName)
        {
            m_UIPublishModule = publishModule;
            m_UIAtlasModule = ((YIUIAutoTool)publishModule.AutoTool).AtlasModule;
            PkgName = pkgName;
            PkgPath = $"{resPath}/{pkgName}";
        }

        private void Refresh()
        {
            FindUIBindCDETableResources();
            FindUITextureResources();
            FindUISpriteAtlasResources();
        }

        public override void Initialize()
        {
            if (this.UserData is UIPublishPackageModuleData data)
            {
                Data = data;
                SetData(data.PublishModule, data.ResPath, data.PkgName);
                Refresh();
                AddAllAssetsAtPath();
                PartialInitialize();
            }
            else
            {
                Debug.LogError($"数据错误");
            }
        }

        partial void PartialInitialize();

        private void AddAllAssetsAtPath()
        {
            var publishPath = Data.PublishPath;
            var resPath = Data.ResPath;
            var pkgName = Data.PkgName;

            //1 图集
            Tree.AddAllAssetsAtPath($"{publishPath}/{YIUIConstHelper.Const.UIAtlasCN}",
                $"{resPath}/{pkgName}/{YIUIConstHelper.Const.UIAtlas}", typeof(SpriteAtlas), true, false);

            //2 预制体
            Tree.AddAllAssetsAtPath($"{publishPath}/{YIUIConstHelper.Const.UIPrefabsCN}",
                $"{resPath}/{pkgName}/{YIUIConstHelper.Const.UIPrefabs}", typeof(UIBindCDETable), true, false);

            //3 源文件
            Tree.AddAllAssetsAtPath($"{publishPath}/{YIUIConstHelper.Const.UISourceCN}",
                $"{resPath}/{pkgName}/{YIUIConstHelper.Const.UISource}", typeof(UIBindCDETable), true, false);

            //4 精灵
            Tree.AddAllAssetImporterAtPath($"{publishPath}/{YIUIConstHelper.Const.UISpritesCN}",
                $"{resPath}/{pkgName}/{YIUIConstHelper.Const.UISprites}", typeof(TextureImporter), true, false);
        }

        private void FindUIBindCDETableResources()
        {
            var strings = AssetDatabase.GetAllAssetPaths()
                                       .Where(x => x.StartsWith($"{PkgPath}/{YIUIConstHelper.Const.UIPrefabs}", StringComparison.InvariantCultureIgnoreCase));

            foreach (var path in strings)
            {
                var cdeTable = AssetDatabase.LoadAssetAtPath<UIBindCDETable>(path);
                if (cdeTable == null) continue;
                if (!cdeTable.IsSplitData)
                {
                    m_AllCDETable.Add(cdeTable);
                }
            }
        }

        private void FindUITextureResources()
        {
            var strings = AssetDatabase.GetAllAssetPaths().Where(x => x.StartsWith($"{PkgPath}/{YIUIConstHelper.Const.UISprites}", StringComparison.InvariantCultureIgnoreCase));

            m_AtlasName.Clear();

            foreach (var path in strings)
            {
                if (AssetImporter.GetAtPath(path) is TextureImporter texture)
                {
                    var atlasName = GetSpritesAtlasName(path);
                    if (string.IsNullOrEmpty(atlasName))
                    {
                        Logger.LogError(texture, $"此文件位置错误 {path}  必须在 {YIUIConstHelper.Const.UISprites}/XX 图集文件下 不可以直接在根目录");
                        continue;
                    }

                    m_AtlasName.Add(atlasName);
                    m_AllTextureImporter.Add(texture);
                }
            }
        }

        private string GetSpritesAtlasName(string path, string currentName = "")
        {
            if (!path.Replace("\\", "/").Contains($"{PkgPath}/{YIUIConstHelper.Const.UISprites}"))
            {
                return null;
            }

            var parentInfo = System.IO.Directory.GetParent(path);
            if (parentInfo == null)
            {
                return currentName;
            }

            if (parentInfo.Name == YIUIConstHelper.Const.UISprites)
            {
                return currentName;
            }

            return GetSpritesAtlasName(parentInfo.FullName, parentInfo.Name);
        }

        private void FindUISpriteAtlasResources()
        {
            var strings = AssetDatabase.GetAllAssetPaths().Where(x =>
                    x.StartsWith($"{PkgPath}/{YIUIConstHelper.Const.UIAtlas}",
                        StringComparison.InvariantCultureIgnoreCase));

            foreach (var path in strings)
            {
                var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(path);
                if (spriteAtlas != null)
                {
                    m_AllSpriteAtlas.Add(spriteAtlas);
                }
            }
        }

        #endregion

        #region 图集

        [Button("创建and强制更新 图集", 30)]
        [PropertyOrder(-997)]
        private void CurrentAtlas()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            CreateOrResetAtlas(true);
            YIUIAutoTool.CloseWindowRefresh();
        }

        public void CreateOrResetAtlas(bool reset = false)
        {
            foreach (var atlasName in m_AtlasName)
            {
                CreateAtlas(atlasName);
                if (reset)
                {
                    ResetAtlas(atlasName);
                }
            }
        }

        public void ResetAtlas(string atlasName)
        {
            if (atlasName == YIUIConstHelper.Const.UIAtlasIgnore) return;

            var atlasFillName = $"{PkgPath}/{YIUIConstHelper.Const.UIAtlas}/Atlas_{PkgName}_{atlasName}.spriteatlasv2";

            if (!File.Exists(atlasFillName)) return;

            var spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(atlasFillName);
            var spriteAtlasAsset = SpriteAtlasAsset.Load(atlasFillName);

            ResetAtlas(spriteAtlas, spriteAtlasAsset, atlasName, true);
        }

        public void CreateAtlas(string atlasName)
        {
            if (atlasName == YIUIConstHelper.Const.UIAtlasIgnore) return;

            var atlasFillName = $"{PkgPath}/{YIUIConstHelper.Const.UIAtlas}/Atlas_{PkgName}_{atlasName}.spriteatlasv2";

            if (File.Exists(atlasFillName)) return;

            var spriteAtlas = new SpriteAtlas();
            var spriteAtlasAsset = new SpriteAtlasAsset();
            ResetAtlas(spriteAtlas, spriteAtlasAsset, atlasName);

            if (!Directory.Exists(Path.GetDirectoryName(atlasFillName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(atlasFillName));
            }

            SpriteAtlasAsset.Save(spriteAtlasAsset, atlasFillName);
            AssetDatabase.Refresh();

            SpriteAtlasImporter sai = (SpriteAtlasImporter)AssetImporter.GetAtPath(atlasFillName);
            if (sai != null)
            {
                m_UIAtlasModule.ResetTargetBuildSetting(sai);
                AssetDatabase.WriteImportSettingsIfDirty(atlasFillName);
            }
        }

        private void ResetAtlas(SpriteAtlas spriteAtlas, SpriteAtlasAsset spriteAtlasAsset, string atlasName, bool reset = false)
        {
            if (spriteAtlas == null || spriteAtlasAsset == null) return;

            if (reset && spriteAtlas != null)
            {
                var packables = spriteAtlas.GetPackables();
                if (packables != null)
                {
                    spriteAtlasAsset.Remove(packables);
                    spriteAtlas.Remove(packables);
                }
            }

            var itemPath = $"{PkgPath}/{YIUIConstHelper.Const.UISprites}/{atlasName}";
            spriteAtlasAsset.Add(new[] { AssetDatabase.LoadMainAssetAtPath(itemPath) });
            spriteAtlas.Add(new[] { AssetDatabase.LoadMainAssetAtPath(itemPath) });
        }

        #endregion
    }
}
#endif