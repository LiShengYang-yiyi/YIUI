#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using ET;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;

namespace YIUIFramework.Editor
{
    public class UIPublishModule : BaseYIUIToolModule
    {
        [ButtonGroup("文档")]
        [Button("YIUI文档", 50, Icon = SdfIconType.Link45deg, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99999)]
        public void OpenDocument1()
        {
            Application.OpenURL("https://lib9kmxvq7k.feishu.cn/wiki/ES7Gwz4EAiVGKSkotY5cRbTznuh");
        }

        internal const string m_PublishName = "发布";
        internal const string m_PublishCommonName = "通用";
        internal const string m_PublishPackageName = "ET包";

        [FolderPath]
        [LabelText("所有模块资源路径")]
        [ReadOnly]
        [ShowInInspector]
        private string m_AllPkgPath = YIUIConstHelper.Const.UIProjectResPath;

        //所有ET模块资源路径
        private const string m_AllETPkgPath = "Packages/";

        [BoxGroup("创建模块", centerLabel: true)]
        [ShowInInspector]
        internal UICreateResModule CreateResModule = new UICreateResModule();

        private readonly Dictionary<string, List<string>> m_AllInfo = new();

        private int m_AllCount;

        private void RefreshAllPkg()
        {
            EditorHelper.CreateExistsDirectory(m_AllPkgPath);
            m_AllInfo.Clear();
            m_AllCount = 0;
            try
            {
                foreach (var yiuiPkg in Directory.GetDirectories(EditorHelper.GetProjPath(m_AllPkgPath)))
                {
                    if (!m_AllInfo.ContainsKey(m_AllPkgPath))
                    {
                        m_AllInfo.Add(m_AllPkgPath, new List<string>());
                    }

                    var list = m_AllInfo[m_AllPkgPath];
                    list.Add(yiuiPkg);
                }

                foreach (var packagePath in Directory.GetDirectories(EditorHelper.GetProjPath(m_AllETPkgPath)))
                {
                    var packageRes = $"{packagePath}/{m_AllPkgPath}";
                    if (Directory.Exists(packageRes))
                    {
                        var etPackagesName = UIOperationHelper.GetETPackagesName(packageRes, false);
                        if (string.IsNullOrEmpty(etPackagesName))
                        {
                            Debug.LogError($"错误这里没有找到ET包名 请检查 {packageRes}");
                            continue;
                        }

                        var resPath = $"{m_AllETPkgPath}{YIUIConstHelper.Const.UIETPackagesFormat}{etPackagesName}/{m_AllPkgPath}";
                        if (!m_AllInfo.ContainsKey(resPath))
                        {
                            m_AllInfo.Add(resPath, new List<string>());
                        }

                        var list = m_AllInfo[resPath];
                        foreach (var yiuiPkg in Directory.GetDirectories(packageRes))
                        {
                            list.Add(yiuiPkg);
                            m_AllCount++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"获取所有模块错误 请检查 err={e}{e.StackTrace}");
                return;
            }

            var commonPublish = $"{m_PublishName}/{m_PublishCommonName}";
            Tree.AddMenuItemAtPath(m_PublishName, new OdinMenuItem(Tree, m_PublishCommonName, null)).AddIcon(EditorIcons.Folder);
            var packagePublish = $"{m_PublishName}/{m_PublishPackageName}";
            Tree.AddMenuItemAtPath(m_PublishName, new OdinMenuItem(Tree, m_PublishPackageName, null)).AddIcon(EditorIcons.Folder);

            foreach ((var resPath, var listInfo) in m_AllInfo)
            {
                var etPackagesName = UIOperationHelper.GetETPackagesName(resPath, false);

                foreach (var folder in listInfo)
                {
                    var pkgName = Path.GetFileName(folder);
                    var upperName = NameUtility.ToFirstUpper(pkgName);
                    if (upperName != pkgName)
                    {
                        Debug.LogError($"这是一个非法的模块[ {pkgName} ]请使用统一方法创建模块 或者满足指定要求");
                        continue;
                    }

                    var showPkgName = string.IsNullOrEmpty(etPackagesName) ? $"{pkgName}" : $"[{etPackagesName}]{pkgName}";

                    var publishName = string.IsNullOrEmpty(etPackagesName) ? $"{commonPublish}" : $"{packagePublish}";

                    var PublishPath = $"{publishName}/{showPkgName}";
                    var pkgMenu = new TreeMenuItem<UIPublishPackageModule>(this.AutoTool, this.Tree, PublishPath,
                        EditorIcons.Folder);
                    pkgMenu.UserData = new UIPublishPackageModuleData
                    {
                        PublishModule = this,
                        PublishPath = PublishPath,
                        ResPath = resPath,
                        PkgName = pkgName
                    };
                }
            }
        }

        [GUIColor(0.4f, 0.8f, 1)]
        [Button("全部发布", 50, Icon = SdfIconType.LayerForward, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-99)]
        public void PublishAll()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            UnityTipsHelper.CallBack("确定发布全部YIUI?", () =>
            {
                var index = 0;
                var total = 0;
                try
                {
                    foreach ((var resPath, var listInfo) in m_AllInfo)
                    {
                        total += listInfo.Count;
                        foreach (var folder in listInfo)
                        {
                            index++;
                            var pkgName = Path.GetFileName(folder);
                            var upperName = NameUtility.ToFirstUpper(pkgName);
                            if (upperName != pkgName)
                            {
                                Debug.LogError($"这是一个非法的模块[ {pkgName} ]请使用统一方法创建模块 或者满足指定要求");
                                continue;
                            }

                            var module = new UIPublishPackageModule(this, resPath, pkgName);
                            module.PublishCurrent(false); //不要默认重置所有图集设置 有的图集真的会有独立设置
                            EditorHelper.DisplayProgressBar("发布", $"正在发布 {pkgName} ...", index, total);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"发布全部时发生错误: {e}");
                }
                finally
                {
                    EditorHelper.ClearProgressBar();
                    UnityTipsHelper.CallBackOk("YIUI全部 发布完毕", YIUIAutoTool.CloseWindowRefresh);
                }
            });
        }

        [ButtonGroup("Atlas")]
        [GUIColor(0f, 1f, 0.8f)]
        [Button("更新图集数据", 40, Icon = SdfIconType.Images, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-98)]
        public void RefreshAtlasData()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            try
            {
                YIUIAtlasModule.RefreshAtlasData();
            }
            catch (Exception e)
            {
                Debug.LogError($"更新图集数据时发生错误: {e}");
            }
            finally
            {
                YIUIAutoTool.CloseWindow();
            }
        }

        [ButtonGroup("Atlas")]
        [GUIColor(1f, 1f, 0.5f)]
        [Button("全部发布图集", 40, Icon = SdfIconType.Images, IconAlignment = IconAlignment.LeftOfText)]
        [PropertyOrder(-98)]
        public void PublishAllAtlas()
        {
            if (!UIOperationHelper.CheckUIOperation()) return;

            UnityTipsHelper.CallBack("确定发布全部YIUI图集?", () =>
            {
                var index = 0;
                var total = 0;
                try
                {
                    foreach ((var resPath, var listInfo) in m_AllInfo)
                    {
                        total += listInfo.Count;
                        foreach (var folder in listInfo)
                        {
                            index++;
                            var pkgName = Path.GetFileName(folder);
                            var upperName = NameUtility.ToFirstUpper(pkgName);
                            if (upperName != pkgName)
                            {
                                Debug.LogError($"这是一个非法的模块[ {pkgName} ]请使用统一方法创建模块 或者满足指定要求");
                                continue;
                            }

                            var module = new UIPublishPackageModule(this, resPath, pkgName);
                            module.CreateOrResetAtlas(true);
                            EditorHelper.DisplayProgressBar("发布图集", $"正在发布图集 {pkgName} ...", index, total);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"发布全部图集时发生错误: {e}");
                }
                finally
                {
                    EditorHelper.ClearProgressBar();
                    UnityTipsHelper.CallBackOk("YIUI全部图集 发布完毕", YIUIAutoTool.CloseWindowRefresh);
                }
            });
        }

        public override void Initialize()
        {
            this.RefreshAllPkg();
            CreateResModule?.Initialize();
        }

        public override void OnDestroy()
        {
            CreateResModule?.OnDestroy();
        }
    }
}
#endif