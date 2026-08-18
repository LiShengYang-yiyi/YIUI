using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using YooAsset.Editor;

namespace ET.WuXia.Editor
{
    // 【武侠包建设 2026-08-18】提供可重复执行的 WuXia 包初始化入口。
    // 作用：安装包后统一接入资源收集、YIUI 项目设置、ET 四层程序集引用和 Demo 冲突保护。
    // 原因：WuXia 必须能像标准依赖包一样导入，不能要求使用者手工修改多个第三方包。
    public static class WuXiaPackageInitializer
    {
        private const string SettingAssetPath = "Packages/cn.etetet.wuxia/Settings/WuXiaYooAssetSetting.asset";
        private const string ConstTemplatePath = "Packages/cn.etetet.wuxia/Settings/Templates/YIUIConstAsset.txt";
        private const string AtlasTemplatePath = "Packages/cn.etetet.wuxia/Settings/Templates/YIUIAtlasData.asset";
        private const string ProjectSettingDirectory = "Assets/GameRes/YIUI/YIUISettings";
        private const string ProjectConstPath = ProjectSettingDirectory + "/YIUIConstAsset.txt";
        private const string ProjectAtlasPath = ProjectSettingDirectory + "/YIUIAtlasData.asset";
        private const string DisabledGroupName = "WuXia_Conflicts_Disabled";
        private const string DemoCollectorPath = "Packages/cn.etetet.yiuistatesync/Assets/GameRes/YIUI";

        private static readonly string[] LegacyStateSyncUiFiles =
        {
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/EntryEvent3_InitClient.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UILogin/AppStartInitFinish_CreateLoginUI.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UILogin/LoginFinish_RemoveLoginUI.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UILobby/LoginFinish_CreateLobbyUI.cs",
            "Packages/cn.etetet.statesync/Scripts/HotfixView/Client/UI/UIHelp/SceneChangeFinishEvent_CreateUIHelp.cs",
        };

        private static readonly string[] YIUIStateSyncDemoEventFiles =
        {
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/EntryEvent3_InitClient.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UILogin/AppStartInitFinish_CreateLoginUI.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UILogin/LoginFinish_RemoveLoginUI.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UILobby/LoginFinish_CreateLobbyUI.cs",
            "Packages/cn.etetet.yiuistatesync/Scripts/HotfixView/Client/UI/UIHelp/SceneChangeFinishEvent_CreateUIHelp.cs",
        };

        [MenuItem("ET/WuXia/Init")]
        public static void Init()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorUtility.DisplayDialog("WuXia 初始化", "请先退出运行模式。", "确定");
                return;
            }

            try
            {
                EnsureProjectYIUISettings();
                MergeYooAssetSetting();
                ProtectThirdPartyDemoEntrances();

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                // 【武侠包建设 2026-08-18】由 WuXia 包自行同步 Rider 方案，不让 Loader 反向识别业务包。
                // 作用：把 ET.WuXia.Editor 幂等加入根 ET.sln，使包内编辑器代码可直接编辑和编译。
                // 原因：解决方案适配属于 WuXia 的安装职责，基础 Loader 必须保持通用且不依赖具体项目包。
                WuXiaRiderSolutionSynchronizer.Sync();

                // 【武侠包建设 2026-08-18】调用 ET 官方菜单重新汇总 packagegit.json 的四层程序集引用。
                // 作用：让 WuXia 的 ModelView 和 HotfixView 能引用 YIUI、TMP 等程序集。
                // 原因：ET9 的 Scripts 目录最终会并入 Demo 的四个程序集，不能由 WuXia 单独改宿主 asmdef。
                if (!EditorApplication.ExecuteMenuItem("ET/Loader/UpdateScriptsReferences"))
                {
                    Debug.LogWarning("未找到 ET/Loader/UpdateScriptsReferences，请在 ET 初始化完成后手动执行 ET -> Refresh。");
                }

                List<string> errors = WuXiaYIUIMigrationValidator.CollectErrors();
                if (errors.Count > 0)
                {
                    string message = string.Join(Environment.NewLine, errors);
                    Debug.LogError($"WuXia 初始化后校验失败：{Environment.NewLine}{message}");
                    EditorUtility.DisplayDialog("WuXia 初始化", $"初始化已执行，但仍有 {errors.Count} 项需要处理。请查看 Console。", "确定");
                    return;
                }

                Debug.Log("WuXia 初始化完成：资源收集、YIUI 设置、程序集引用和入口所有权均已接入。");
                EditorUtility.DisplayDialog("WuXia 初始化", "初始化完成。请重新编译 ET.sln，再从 Loader/Scenes/Init.unity 启动。", "确定");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog("WuXia 初始化失败", "初始化未完成，请根据 Console 错误处理后重试。", "确定");
            }
        }

        private static void EnsureProjectYIUISettings()
        {
            string absoluteDirectory = AbsolutePath(ProjectSettingDirectory);
            Directory.CreateDirectory(absoluteDirectory);

            CopyTemplateWhenMissing(ConstTemplatePath, ProjectConstPath);
            CopyTemplateWhenMissing(AtlasTemplatePath, ProjectAtlasPath);

            string constPath = AbsolutePath(ProjectConstPath);
            string content = File.ReadAllText(constPath, Encoding.UTF8);

            // 【武侠包建设 2026-08-18】只更新 YIUI 项目设置中的包选择字段，保留使用者的画布和安全区参数。
            // 作用：让 YIUI AutoTool 的生成代码和资源都落到 cn.etetet.wuxia。
            // 原因：YIUI 编辑器固定从 Assets/GameRes/YIUI/YIUISettings 读取配置，必须生成这一层宿主适配文件。
            content = ReplaceJsonString(content, "UIETCreatePackageName", "wuxia");
            content = ReplaceJsonString(content, "UIETCreatePackagePath", "Assets/../Packages/cn.etetet.wuxia");
            File.WriteAllText(constPath, content, new UTF8Encoding(false));
        }

        private static void CopyTemplateWhenMissing(string sourcePath, string targetPath)
        {
            string source = AbsolutePath(sourcePath);
            string target = AbsolutePath(targetPath);
            if (File.Exists(target))
            {
                return;
            }

            if (!File.Exists(source))
            {
                throw new FileNotFoundException($"WuXia 设置模板不存在：{sourcePath}", source);
            }

            File.Copy(source, target, false);
        }

        private static string ReplaceJsonString(string content, string propertyName, string value)
        {
            string pattern = $"(\"{Regex.Escape(propertyName)}\"\\s*:\\s*\")[^\"]*(\")";
            if (!Regex.IsMatch(content, pattern))
            {
                throw new InvalidDataException($"YIUIConstAsset 缺少字段：{propertyName}");
            }

            Regex regex = new(pattern);
            return regex.Replace(content, match => match.Groups[1].Value + value + match.Groups[2].Value);
        }

        private static void MergeYooAssetSetting()
        {
            AssetBundleCollectorSetting targetSetting = AssetBundleCollectorSettingData.Setting;
            if (targetSetting == null)
            {
                throw new InvalidOperationException("没有找到项目级 AssetBundleCollectorSetting。");
            }

            WuXiaYooAssetSetting sourceSetting = AssetDatabase.LoadAssetAtPath<WuXiaYooAssetSetting>(SettingAssetPath);
            AssetBundleCollectorPackage sourcePackage = sourceSetting?.GetPackage("DefaultPackage");
            AssetBundleCollectorPackage targetPackage = targetSetting.GetPackage("DefaultPackage");
            if (sourcePackage == null || targetPackage == null)
            {
                throw new InvalidOperationException("WuXia 或项目级 YooAsset 配置缺少 DefaultPackage。");
            }

            targetSetting.UniqueBundleName = true;
            targetPackage.EnableAddressable = true;

            foreach (AssetBundleCollectorGroup sourceGroup in sourcePackage.Groups)
            {
                AssetBundleCollectorGroup targetGroup = GetOrCreateGroup(targetPackage, sourceGroup.GroupName);
                targetGroup.GroupDesc = sourceGroup.GroupDesc;
                targetGroup.AssetTags = sourceGroup.AssetTags;
                targetGroup.ActiveRuleName = nameof(EnableGroup);

                foreach (AssetBundleCollector sourceCollector in sourceGroup.Collectors)
                {
                    AssetBundleCollector targetCollector = MoveOrCreateCollector(targetPackage, targetGroup, sourceCollector.CollectPath);
                    CopyCollector(sourceCollector, targetCollector);
                    targetCollector.CollectorGUID = AssetDatabase.AssetPathToGUID(targetCollector.CollectPath);
                }
            }

            DisableCollectorWhenPresent(targetPackage, DemoCollectorPath);
            EditorUtility.SetDirty(targetSetting);
        }

        private static AssetBundleCollectorGroup GetOrCreateGroup(AssetBundleCollectorPackage package, string groupName)
        {
            AssetBundleCollectorGroup group = package.Groups.FirstOrDefault(item => item.GroupName == groupName);
            if (group != null)
            {
                return group;
            }

            group = new AssetBundleCollectorGroup { GroupName = groupName };
            package.Groups.Add(group);
            return group;
        }

        private static AssetBundleCollector MoveOrCreateCollector(
            AssetBundleCollectorPackage package,
            AssetBundleCollectorGroup targetGroup,
            string collectPath)
        {
            foreach (AssetBundleCollectorGroup group in package.Groups)
            {
                AssetBundleCollector collector = group.Collectors.FirstOrDefault(item => item.CollectPath == collectPath);
                if (collector == null)
                {
                    continue;
                }

                if (group != targetGroup)
                {
                    group.Collectors.Remove(collector);
                    targetGroup.Collectors.Add(collector);
                }

                return collector;
            }

            AssetBundleCollector created = new();
            targetGroup.Collectors.Add(created);
            return created;
        }

        private static void CopyCollector(AssetBundleCollector source, AssetBundleCollector target)
        {
            target.CollectPath = source.CollectPath;
            target.CollectorType = source.CollectorType;
            target.AddressRuleName = source.AddressRuleName;
            target.PackRuleName = source.PackRuleName;
            target.FilterRuleName = source.FilterRuleName;
            target.AssetTags = source.AssetTags;
            target.UserData = source.UserData;
        }

        private static void DisableCollectorWhenPresent(AssetBundleCollectorPackage package, string collectPath)
        {
            AssetBundleCollectorGroup disabledGroup = GetOrCreateGroup(package, DisabledGroupName);
            disabledGroup.ActiveRuleName = nameof(DisableGroup);
            disabledGroup.GroupDesc = "【武侠包接入 2026-08-18】保留会造成地址或入口冲突的原收集器。作用：支持审计和回滚。原因：WuXia 正常运行只能启用一套 Login、Lobby、Main 和 YIUIRoot。";

            foreach (AssetBundleCollectorGroup group in package.Groups.ToArray())
            {
                if (group == disabledGroup || group.ActiveRuleName == nameof(DisableGroup))
                {
                    continue;
                }

                AssetBundleCollector collector = group.Collectors.FirstOrDefault(item => item.CollectPath == collectPath);
                if (collector == null)
                {
                    continue;
                }

                group.Collectors.Remove(collector);
                collector.UserData = "【武侠包接入 2026-08-18】原配置未删除，仅在 WuXia 模式下停用；回滚时可恢复到启用分组。";
                disabledGroup.Collectors.Add(collector);
            }
        }

        private static void ProtectThirdPartyDemoEntrances()
        {
            foreach (string path in LegacyStateSyncUiFiles)
            {
                EnsureConditionalGuard(
                    path,
                    "WUXIA_LEGACY_STATESYNC_UI",
                    "停用 StateSync 自带旧 UGUI 入口并保留完整源码。作用：防止旧 UI 与 WuXia YIUI 同时响应事件。原因：StateSync 是可运行 Demo，其表现层入口不是可并存的基础能力。");
            }

            foreach (string path in YIUIStateSyncDemoEventFiles)
            {
                EnsureConditionalGuard(
                    path,
                    "YIUI_STATESYNC_DEMO",
                    "停用 YIUI StateSync Demo 入口并保留完整源码。作用：允许通过编译符号回滚 Demo。原因：两个 YIUI 入口会重复初始化根组件并重复打开面板。");
            }
        }

        private static void EnsureConditionalGuard(string relativePath, string symbol, string explanation)
        {
            string path = AbsolutePath(relativePath);
            if (!File.Exists(path))
            {
                return;
            }

            string content = File.ReadAllText(path, Encoding.UTF8);
            string trimmed = content.TrimStart();
            bool fullyBlockCommented =
                trimmed.StartsWith("/*", StringComparison.Ordinal) &&
                trimmed.TrimEnd().EndsWith("*/", StringComparison.Ordinal);
            if (trimmed.StartsWith($"#if {symbol}", StringComparison.Ordinal) ||
                fullyBlockCommented)
            {
                return;
            }

            string guarded =
                $"#if {symbol}{Environment.NewLine}" +
                $"// 【武侠包接入 2026-08-18】{explanation}{Environment.NewLine}" +
                content.TrimEnd() + Environment.NewLine +
                $"#endif{Environment.NewLine}";
            File.WriteAllText(path, guarded, new UTF8Encoding(false));
        }

        private static string AbsolutePath(string relativePath)
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            return Path.Combine(projectRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
        }
    }
}
